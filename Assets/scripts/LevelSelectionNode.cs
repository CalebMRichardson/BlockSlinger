using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionNode : MonoBehaviour
{
    [SerializeField]
    private int levelIndex = -1;
    [SerializeField]
    LevelSelectionManager levelSelectionManager;
    [SerializeField]
    private bool locked;

    private const int LEFT_MOUSE_BUTTON = 0;

    private void Start() {

        if (levelSelectionManager == null) {
            Debug.LogError("LevelSelectionManager.cs is null.");
        }

        UpdateLockedStatus();
    }

    private void Update() {
        HandleInput();
    }

    public int GetLevelIndex() {
        return levelIndex;
    }

    private void HandleInput() {

        if(levelIndex == -1) {
            Debug.LogError("LevelSelction Index is set to -1.");
            return;
        }

        GetMobileInput();
        GetDesktopInput();
    }

    private void GetMobileInput() {
        if(Input.touchCount > 0) {

            Touch touch = Input.touches[0];

            switch(touch.phase) {

                case TouchPhase.Ended:

                    Vector3 touchUpPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));

                    if(CheckForTouchDownHit(new Vector2(touchUpPosition.x, touchUpPosition.y))) {

                        if(!locked) {
                            levelSelectionManager.StartLevel(levelIndex);
                        }

                    }

                    break;
            }
        }
    }

    private void GetDesktopInput() {

        if(Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON)) {

            Vector3 touchUpPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            if(CheckForTouchDownHit(new Vector2(touchUpPosition.x, touchUpPosition.y))) {

                if(!locked) {
                    levelSelectionManager.StartLevel(levelIndex);
                }

            }
        }
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

    public void UpdateLockedStatus() {

        int maxLevel = PlayerPrefs.GetInt(LevelManager.MAX_LEVEL_INDEX);

        if(levelIndex <= maxLevel) {
            Unlock();
        } else {
            Lock();
        }
    }

    public void Unlock() {
        locked = false;
    }

    public void Lock() {
        locked = true;
    }
}
