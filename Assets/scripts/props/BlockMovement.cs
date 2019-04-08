using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{

    private Block block;
    private bool touched;
    private Vector3 initialTouchDownPoint;
    [SerializeField]
    private Vector2 moveDirection;
    public enum MoveState { IDLE, TRYING_TO_MOVE, MOVING, CANT_MOVE, IN_PLACE, IN_GOAL, FALLING }
    public MoveState moveState;

    private GameObject currentLevel;
    private Level currentLevelScript;

    private float blockMoveSpeed;
    private float closeEnough;
    private bool moveHappend;

    private const int LEFT_MOUSE_BUTTON = 0;
    private const int GAME_PAUSED = 0;

    [SerializeField]
    private GameObject nextProp;
    

    private void Start() {

        block = GetComponent<Block>();
        initialTouchDownPoint = new Vector3();
        touched = false;
        currentLevel = transform.parent.gameObject;
        currentLevelScript = currentLevel.GetComponent<Level>();
        moveState = MoveState.IDLE;
        blockMoveSpeed = 18.0f;
        closeEnough = .04f;
        moveHappend = false;
        block.lastPosition = transform.position;
    }


    private void Update() {

        HandleInput();

        switch(moveState) {

            case MoveState.IDLE:

                moveDirection = Vector2.zero;

                if (Time.timeScale == 0) {

                }

                break;
            case MoveState.TRYING_TO_MOVE:

                CheckMoveDirection(block.x + (int)moveDirection.x, block.y + (int)moveDirection.y);

                break;

            case MoveState.MOVING:

                Move();

                break;

            case MoveState.IN_PLACE:

                Prop nextPropScript = nextProp.GetComponent<Prop>();

                if(nextPropScript.isBlank) {

                    Destroy(currentLevelScript.propLayer[ nextPropScript.y, nextPropScript.x ]);
                    currentLevelScript.propLayer[ nextPropScript.y, nextPropScript.x ] = block.gameObject;
                    UpdateBoardOfMovedBlock(ref nextPropScript);
                    moveState = MoveState.IDLE;

                } else if (nextPropScript.isHole) {

                    block.PlayAnimationAndDestroy("block_fall");
                    UpdateBoardOfMovedBlock(ref nextPropScript);
                    moveState = MoveState.IDLE;

                } else if (nextPropScript.isTrap) {

                    Trap trapScript = nextPropScript.GetComponent<Trap>();
                    moveDirection = trapScript.direction;
                    UpdateBoardOfMovedBlock(ref nextPropScript);
                    moveState = MoveState.TRYING_TO_MOVE;
                    moveHappend = false;
                    break;
                }

                if (block.CheckIfInGoal()) {
                    moveState = MoveState.IN_GOAL;
                }

                moveHappend = false;

                break;

            case MoveState.IN_GOAL:
                if(block.GetBlockInGoal() == false) {
                    block.PlayBlockInGoalSFX();
                    block.SetBlockInGoal(true);
                }
                break;

            case MoveState.CANT_MOVE:
                moveState = MoveState.IDLE;
                break;

            case MoveState.FALLING:
                break;
        }
    }

    private void HandleInput() {

        if (GameStateManager.gamePaused) {
            return;
        }

        if(Application.platform == RuntimePlatform.Android)
            GetMobileInput();
        else if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
            GetDesktopInput();
        }
    }

    private void GetMobileInput() {
        if(Input.touchCount > 0) {

            Touch touch = Input.touches[0];

            switch(touch.phase) {

                case TouchPhase.Began:

                    // Get Position of touch converted into world coords
                    initialTouchDownPoint = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));

                    Vector2 touchPos = new Vector2(initialTouchDownPoint.x, initialTouchDownPoint.y);

                    if(CheckForTouchDownHit(touchPos)) {
                        touched = true;
                    }

                    break;

                case TouchPhase.Ended:

                    if(touched) {

                        Vector3 touchUpPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));

                        moveDirection = GetDirection(touchUpPosition, initialTouchDownPoint);

                        if(moveDirection != Vector2.zero) {

                            if(moveState == MoveState.IDLE) {
                                moveState = MoveState.TRYING_TO_MOVE;
                            }
                        }

                        touched = false;
                    }

                    break;
            }
        }
    }

    private void GetDesktopInput() {

        if(Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON)) {

            initialTouchDownPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            Vector2 touchPos = new Vector2(initialTouchDownPoint.x, initialTouchDownPoint.y);

            if(CheckForTouchDownHit(touchPos)) {
                touched = true;
            }
        }

        if(Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON)) {

            if(touched) {

                Vector3 touchUpPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

                moveDirection = GetDirection(touchUpPosition, initialTouchDownPoint);

                if(moveDirection != Vector2.zero) {

                    if(moveState == MoveState.IDLE) {
                        moveState = MoveState.TRYING_TO_MOVE;
                    }
                }

                touched = false;
            }
        }
    }

    private Vector2 GetDirection(Vector2 _firstVec, Vector2 _secondVec) {

        Vector2 directionVec = new Vector2();

        float distanceX = _firstVec.x - _secondVec.x;
        float distanceY = _firstVec.y - _secondVec.y;

        float xDistanceAbsVal = Mathf.Abs(distanceX);
        float yDistanceAbsVal = Mathf.Abs(distanceY);

        // Will be checking for a zero direction vector to know no movement will be happening
        if(distanceX == 0 && distanceY == 0) {
            return Vector2.zero;
        }

        if(xDistanceAbsVal > yDistanceAbsVal) {
            if(distanceX > 0) {
                directionVec.x = 1;
            } else {
                directionVec.x = -1;
            }
        } else {
            if(distanceY > 0) {
                directionVec.y = 1;
            } else {
                directionVec.y = -1;
            }
        }

        return directionVec;
    }

    private bool CheckForTouchDownHit(Vector2 _pos) {

        Collider2D hit = Physics2D.OverlapPoint(_pos);

        if(hit == null) {
            return false;
        }

        if(hit.gameObject == this.gameObject) {
            return true;
        }

        return false;
    }

    private void Move() {

        block.transform.position = Vector2.MoveTowards(block.transform.position, nextProp.transform.position, blockMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(block.transform.position, nextProp.transform.position) <= closeEnough) {
            transform.position = nextProp.transform.position;
            moveState = MoveState.IN_PLACE;
        }
    }

    private void CheckMoveDirection(int nextX, int nextY) {
        if (moveDirection == Vector2.zero) {
            Debug.LogError("MoveDirection is zero.");
            return; 
        }

        GameObject testProp;

        testProp = currentLevelScript.propLayer[ nextY, nextX ];

        Prop testPropScript = testProp.GetComponent<Prop>();

        if (testPropScript != null) {

            if(testPropScript.isBlank) {

                moveHappend = true;
                nextProp = testProp;
                CheckMoveDirection(nextX + (int)moveDirection.x, nextY + (int)moveDirection.y);

            } else if(testPropScript.isHole) {

                moveHappend = true;
                nextProp = testProp;
                moveState = MoveState.MOVING;

            } else if(testPropScript.isTrap) {

                moveHappend = true;
                nextProp = testProp;
                moveState = MoveState.MOVING;

            } else {

                if(moveHappend) {

                    moveState = MoveState.MOVING;

                } else {

                    moveState = MoveState.CANT_MOVE;

                }
            }
        }
    }

    private void UpdateBoardOfMovedBlock(ref Prop _nextPropScript) {

        if(currentLevelScript.propLayer[ block.y, block.x ].GetComponent<Prop>().isBlank || currentLevelScript.propLayer[block.y, block.x] == block.gameObject) {
            CreateBlank(block.x, block.y);
        } 

        block.x = _nextPropScript.x;
        block.y = _nextPropScript.y;
        block.lastPosition = transform.position;
        block.name = "_Block" + block.y + ":" + block.x;
    }

    private void CreateBlank(int _x, int _y) {
        GameObject blank = Instantiate(Resources.Load(LevelObjectLookup.BLANK_BLOCK_PATH)) as GameObject;
        Prop blankPropScript = blank.GetComponent<Prop>();
        blankPropScript.isBlank = true;
        blankPropScript.x = _x;
        blankPropScript.y = _y;
        blank.name = "_Blank" + _y + ":" + _x;
        blank.transform.position = block.lastPosition;
        blank.transform.parent = transform.parent;

        currentLevelScript.propLayer[ blankPropScript.y, blankPropScript.x ] = blank;
    }
}
