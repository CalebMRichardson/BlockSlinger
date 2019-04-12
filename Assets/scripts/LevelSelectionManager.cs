using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera levelSelectCam;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject levelManager; 

    private const int LEFT_MOUSE_BUTTON = 0;

    private void Start() {
        mainCamera.enabled = false;
        levelSelectCam.enabled = true;
    }

    public void StartLevel(int _levelIndex) {

        mainCamera.enabled = true;
        levelSelectCam.enabled = false;

        levelManager.gameObject.SetActive(true);

        LevelManager.instance.SetCurrentLevelIndex(_levelIndex);
        LevelManager.instance.LoadCurrentLevel();

        GameStateManager.instance.gameState = GameStateManager.GameState.PLAY_STATE;

    }

    public void LevelSelctionEnable() {

        mainCamera.enabled = false;
        levelSelectCam.enabled = true;

        foreach (Transform child in levelManager.transform) {
            Destroy(child.gameObject);
        }

        levelManager.SetActive(false);

        GameStateManager.instance.ResumeGame();
        GameStateManager.instance.gameState = GameStateManager.GameState.LEVEL_SELECTION;
    }
}
