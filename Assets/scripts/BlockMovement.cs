using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{

    private Vector3 initialTouchDownPoint;
    private Block block;
    private bool touched;
    private Vector2 destination;
    public Vector2 moveDirection;
    public enum MoveState { IDLE, TRYING_TO_MOVE, MOVING, CANT_MOVE, IN_PLACE, IN_GOAL}
    public MoveState moveState;
    private bool moveHappend; 
    [SerializeField]
    private GameObject currentLevel;
    private Level currentLevelScript;

    private const int LEFT_MOUSE_BUTTON = 0; 

    // Start is called before the first frame update
    void Start()
    {
        block = GetComponent<Block>();
        initialTouchDownPoint = new Vector3();
        destination = new Vector2();
        touched = false;
        currentLevel = transform.parent.gameObject;
        currentLevelScript = currentLevel.GetComponent<Level>();
        moveState = MoveState.IDLE;
        moveHappend = false;
    }


    // Update is called once per frame
    private void Update() {

        HandleInput();

        switch(moveState) {

            case MoveState.IDLE:
                moveDirection = Vector2.zero;
                moveHappend = false;
                break;
            case MoveState.TRYING_TO_MOVE:
                CalculateMove();
                break;
            case MoveState.MOVING:
                Move();
                break;
            case MoveState.IN_PLACE:
                block.PlayRandomHitSFX();
                if(block.CheckIfInGoal() == true) {
                    moveState = MoveState.IN_GOAL;
                } else {
                    moveState = MoveState.IDLE;
                }
                break;
            case MoveState.IN_GOAL:
                if(block.GetBlockInGoal() == false) {
                    block.PlayBlockInGoalSFX();
                    block.SetBlockInGoal(true);
                }
                break;
            case MoveState.CANT_MOVE:
                moveDirection = Vector2.zero;
                moveState = MoveState.IDLE;
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

        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON)) {

            initialTouchDownPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            Vector2 touchPos = new Vector2(initialTouchDownPoint.x, initialTouchDownPoint.y);

            if (CheckForTouchDownHit(touchPos)) {
                touched = true;
            }
        }

        if (Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON)) {

            if (touched) {

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
        if (distanceX == 0 && distanceY == 0) {
            return Vector2.zero;
        }

        if (xDistanceAbsVal > yDistanceAbsVal) {
            if (distanceX > 0) {
                directionVec.x = 1;
            } else {
                directionVec.x = -1;
            }
        } else {
            if (distanceY > 0) {
                directionVec.y = 1;
            } else {
                directionVec.y = -1;
            }
        }

        return directionVec;
    }

    private bool CheckForTouchDownHit(Vector2 _pos) {


        Collider2D hit = Physics2D.OverlapPoint(_pos);

        if (hit == null) {
            return false;
        }

        if (hit.gameObject == this.gameObject) {
            return true;
        }

        return false;
    }

    private bool CanMove(GameObject nextObject) {


        if(nextObject != null) {
            Prop nextObjectScript = nextObject.GetComponent<Prop>();

            if(nextObjectScript != null) {

                if(nextObjectScript.isBlank) {
                    return true;
                }
            }
        }

        return false;
    }

    private void CalculateMove() {

        GameObject nextLocationObj = currentLevelScript.propLayer[block.y + (int)moveDirection.y, block.x + (int)moveDirection.x];

        if(CanMove(nextLocationObj)) {

            if (moveHappend == false) {
                block.PlayShootSFX();
            }

            moveHappend = true;

            int currentX = block.x;
            int currentY = block.y;
            Vector2 currentPos = transform.position;

            destination = nextLocationObj.transform.position;

            block.x += (int)moveDirection.x;
            block.y += (int)moveDirection.y;

            block.name = "_Block" + block.y + ":" + block.x;

            currentLevelScript.propLayer[ block.y, block.x ] = block.gameObject;
            GameObject blank = nextLocationObj;

            currentLevelScript.propLayer[ currentY, currentX ] = blank;
            blank.name = "_Blank" + currentY + ":" + currentX;
            blank.transform.position = currentPos;

            Prop blankProp = blank.GetComponent<Prop>();
            blankProp.SetIsBlank(true);

            moveState = MoveState.MOVING;

        } else {

            if(moveHappend) {
                moveState = MoveState.IN_PLACE;
                
            } else {
                moveState = MoveState.CANT_MOVE;
            }
        }
    }

    private void Move() {

        //TODO get rid of magic numbers see if we can speed up the android movement

        transform.position = Vector2.MoveTowards(transform.position, destination, .75f);

        if (Vector2.Distance(transform.position, destination) <= .75f) {
            transform.position = destination;
            moveState = MoveState.TRYING_TO_MOVE;
        } 
    }
}
