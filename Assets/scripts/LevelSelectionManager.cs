using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera levelSelectCam;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject levelManager;
    private int maxLevel = -1;
    private const int LEFT_MOUSE_BUTTON = 0;
    public List<LevelSelectionNode> levelSelectionNodes; 

    private void Awake() {

        if (PlayerPrefs.HasKey(LevelManager.MAX_LEVEL_INDEX)) {

            maxLevel = PlayerPrefs.GetInt(LevelManager.MAX_LEVEL_INDEX);

        } else if (!PlayerPrefs.HasKey(LevelManager.MAX_LEVEL_INDEX)) {

            PlayerPrefs.SetInt(LevelManager.MAX_LEVEL_INDEX, 1);
            maxLevel = PlayerPrefs.GetInt(LevelManager.MAX_LEVEL_INDEX);
            PlayerPrefs.Save();

        }
    }

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

        foreach (LevelSelectionNode lvlNode in levelSelectionNodes) {
            lvlNode.UpdateLockedStatus();
        }

        levelManager.SetActive(false);

        GameStateManager.instance.ResumeGame();
        GameStateManager.instance.gameState = GameStateManager.GameState.LEVEL_SELECTION;
    }
}
