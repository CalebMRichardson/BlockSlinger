using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Block block;
    private bool touched;
    private Vector3 initialTouchDownPoint;
    private Vector2 destination;
    [SerializeField]
    private Vector2 moveDirection;
    public enum MoveState { IDLE, TRYING_TO_MOVE, MOVING, CANT_MOVE, IN_PLACE, IN_GOAL, FALLING }
    public MoveState moveState;

    private GameObject currentLevel;
    private Level currentLevelScript;

    private float blockMoveSpeed;
    private float closeEnough; 

    private const int LEFT_MOUSE_BUTTON = 0;


    private void Start() {

        block = GetComponent<Block>();
        initialTouchDownPoint = new Vector3();
        destination = new Vector2();
        touched = false;
        currentLevel = transform.parent.gameObject;
        currentLevelScript = currentLevel.GetComponent<Level>();
        moveState = MoveState.IDLE;
        blockMoveSpeed = .4f;
        closeEnough = .04f;
    }

    private void Update() {

        HandleInput();

        switch(moveState) {

            case MoveState.IDLE:
                moveDirection = Vector2.zero;
                block.lastPosition = transform.position;
                break;
            case MoveState.TRYING_TO_MOVE:
                CheckMoveDirection(block.x + (int)moveDirection.x, block.y + (int)moveDirection.y);
                break;
            case MoveState.MOVING:
                Move();
                break;
            case MoveState.IN_PLACE:
               
                break;
            case MoveState.IN_GOAL:
                
                break;
            case MoveState.CANT_MOVE:
                
                break;

            case MoveState.FALLING:
                MoveToHole();
                break;
        }
    }

    private void HandleInput() {

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

    GameObject nextSpot; 

    private void CheckMoveDirection(int nextX, int nextY) {

        if (moveDirection == Vector2.zero) {
            Debug.LogError("MoveDirection is zero.");
            return; 
        }

        GameObject testSpot;

        testSpot = currentLevelScript.propLayer[ nextY, nextX];

        Prop testSpotPropScript = testSpot.GetComponent<Prop>();

        if (testSpotPropScript != null) {
            if (testSpotPropScript.isBlank) {
                nextSpot = testSpot;
                CheckMoveDirection(nextX + (int)moveDirection.x, nextY + (int)moveDirection.y);

            } else if (testSpotPropScript.isHole) {
                nextSpot = testSpot;
                moveState = MoveState.FALLING;

            } else {

                if (nextSpot != null) {

                    if (nextSpot != null) {
                        moveState = MoveState.MOVING;
                    }
                }
            }
        }
    }

    private void Move() {

        transform.position = Vector2.MoveTowards(transform.position, nextSpot.transform.position, blockMoveSpeed);

        if (Vector2.Distance(transform.position, nextSpot.transform.position) <= closeEnough) {
            transform.position = nextSpot.transform.position;
            UpdatePropLayerMove();
            moveState = MoveState.IDLE;
        }
    }

    private void MoveToHole() {
        transform.position = Vector2.MoveTowards(transform.position, nextSpot.transform.position, blockMoveSpeed);

        if(Vector2.Distance(transform.position, nextSpot.transform.position) <= closeEnough) {
            transform.position = nextSpot.transform.position;
            UpdateBoardOfFall();
            block.PlayAnimationAndDestroy("block_fall");
        }
    }

    private void UpdateBoardOfFall() {

        // Update the board about the blocks last position
        GameObject lastBlock = Instantiate(Resources.Load(LevelObjectLookup.BLANK_BLOCK_PATH)) as GameObject;
        Prop lastBlockPropScript = lastBlock.GetComponent<Prop>();
        lastBlockPropScript.isBlank = true;
        lastBlockPropScript.x = block.x;
        lastBlockPropScript.y = block.y;
        lastBlock.name = "_Blank" + lastBlockPropScript.y + ":" + lastBlockPropScript.x;
        lastBlock.transform.position = block.lastPosition;
        lastBlock.transform.parent = transform.parent;

        currentLevelScript.propLayer[ lastBlockPropScript.y, lastBlockPropScript.x ] = lastBlock;

    }

    private void UpdatePropLayerMove() {

        // Update the board about the blocks last position
        GameObject lastBlock = Instantiate(Resources.Load(LevelObjectLookup.BLANK_BLOCK_PATH)) as GameObject;
        Prop lastBlockPropScript = lastBlock.GetComponent<Prop>();
        lastBlockPropScript.isBlank = true;
        lastBlockPropScript.x = block.x;
        lastBlockPropScript.y = block.y; 
        lastBlock.name = "_Blank" + lastBlockPropScript.y + ":" + lastBlockPropScript.x;
        lastBlock.transform.position = block.lastPosition;
        lastBlock.transform.parent = transform.parent;

        currentLevelScript.propLayer[ lastBlockPropScript.y, lastBlockPropScript.x ] = lastBlock;

        // Update the board about the blocks current position
        Prop nextSpotPropScript = nextSpot.GetComponent<Prop>();
        block.x = nextSpotPropScript.x;
        block.y = nextSpotPropScript.y;
        Destroy(currentLevelScript.propLayer[ block.y, block.x ]);
        currentLevelScript.propLayer[ block.y, block.x ] = block.gameObject;
        block.name = "_Block" + block.y + ":" + block.x; 

    }

}
