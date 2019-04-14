using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance = null;

    private GameObject currentLevel;
    private GameObject lastLevel;
    private GameObject nextLevel;
    private LevelBuilder levelBuilder;
    private Level currentLevelScript;
    private Vector2 currentLevelPos;
    [SerializeField]
    private CameraMovement cameraMovement;
    public LevelSelectionManager levelSelectionManager; 
    public static string MAX_LEVEL_INDEX = "MAX_LEVEL_INDEX";

    // Size of a 512x512 in unity units
    private float hallSizeOffset = 5.12f; 

    //TODO read from file
    private int currentLevelIndex;


    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        cameraMovement = Camera.main.GetComponent<CameraMovement>();
    }

    public void LoadCurrentLevel() {
        levelBuilder = GameObject.Find("LevelBuilder").GetComponent<LevelBuilder>();
        currentLevel = levelBuilder.Build(currentLevelIndex);
        currentLevel.transform.parent = gameObject.transform;
        currentLevelScript = currentLevel.GetComponent<Level>();
        currentLevelScript.SetIsCurrentLevel(true);
        cameraMovement.SetCameraPos(currentLevelScript.GetCameraFocus());
        nextLevel = LoadNextLevel();
    }

    public void SetCurrentLevelIndex(int _currentLevelIndex) {

        currentLevelIndex = _currentLevelIndex; 

    }

    private GameObject LoadNextLevel() {

        currentLevelIndex++;

        GameObject nextLvl = levelBuilder.Build(currentLevelIndex);
        nextLvl.transform.parent = gameObject.transform;
        nextLvl.transform.position = CalculateNextLevelPosition(ref nextLvl);

        return nextLvl;
    }

    private Vector2 CalculateNextLevelPosition(ref GameObject _nextLvl) {

        Vector2 nextLevelPos = new Vector2();

        if(currentLevel == null) {
            Debug.LogError(currentLevel.name + " is null when Calculating NextLevelPosition");
        }

        if (_nextLvl == null) {
            Debug.LogError(nextLevel.name + " is null when Calculating NextLevelPosition");
        }

        Level nextLvlScript = _nextLvl.GetComponent<Level>();

        float offsetX = 0;
        float offsetY = 0;

        if (nextLvlScript.IsNorthEnterance()) {

            offsetX = _nextLvl.transform.position.x - nextLvlScript.GetLevelEnterancePos(true).x;
            offsetY = _nextLvl.transform.position.y - nextLvlScript.GetLevelEnterancePos(true).y;
            nextLevelPos = currentLevelScript.GetLevelExitPos(true);
            offsetY -= hallSizeOffset;

        } else if (nextLvlScript.IsSouthEnterance()) {
            
            offsetX = _nextLvl.transform.position.x - nextLvlScript.GetLevelEnterancePos(true).x;
            offsetY = _nextLvl.transform.position.y - nextLvlScript.GetLevelEnterancePos(true).y;
            nextLevelPos = currentLevelScript.GetLevelExitPos(true);
            offsetY += hallSizeOffset;

        } else if (nextLvlScript.IsWestEnterance()) {
            
            offsetX = _nextLvl.transform.position.x - nextLvlScript.GetLevelEnterancePos(false).x;
            offsetY = _nextLvl.transform.position.y - nextLvlScript.GetLevelEnterancePos(false).y;
            nextLevelPos = currentLevelScript.GetLevelExitPos(false);
            offsetX += hallSizeOffset;

        } else if (nextLvlScript.IsEastEnterance()) {
            
            offsetX = _nextLvl.transform.position.x - nextLvlScript.GetLevelEnterancePos(false).x;
            offsetY = _nextLvl.transform.position.y - nextLvlScript.GetLevelEnterancePos(false).y;
            nextLevelPos = currentLevelScript.GetLevelExitPos(false);
            offsetX -= hallSizeOffset;
        }

        nextLevelPos.x += offsetX;
        nextLevelPos.y += offsetY; 

        return nextLevelPos;
    }

    private void IncrementCurrentLevel() {

        if (lastLevel != null) {
            Destroy(lastLevel);
        }

        if (currentLevelScript.IsLastLevel()) {
            GameStateManager.instance.gameState = GameStateManager.GameState.LEVEL_SELECTION;

            SaveLevelProgress(currentLevelScript.GetLevelIndex());

            levelSelectionManager.LevelSelctionEnable();

            print(PlayerPrefs.GetInt(MAX_LEVEL_INDEX));

            return;
        }

        currentLevelScript.PlayGateAnimations();
        
        lastLevel = currentLevel;

        Level lastLevelScript = lastLevel.GetComponent<Level>();
        lastLevelScript.SetIsCurrentLevel(false);

        currentLevel = nextLevel;
        currentLevelScript = currentLevel.GetComponent<Level>();
        currentLevelScript.SetIsCurrentLevel(true);
        FocusCameraOnCurrentLevel();

        nextLevel = LoadNextLevel();
    }

    public void FocusCameraOnCurrentLevel() {

        Level level = currentLevel.GetComponent<Level>();

        cameraMovement.MoveToNextLevel(level.GetCameraFocus());
    }

    private void Update() {
        
        if (currentLevelScript.IsLevelComplete()) {
            IncrementCurrentLevel();
            SaveLevelProgress(currentLevelScript.GetLevelIndex());
        }
    }

    private void SaveLevelProgress(int _levelIndex) {

        PlayerPrefs.SetInt(MAX_LEVEL_INDEX, _levelIndex);
        PlayerPrefs.Save();
    }

    public void ReloadCurrentLevel() {

        Vector2 pos = currentLevel.transform.position;

        Debug.Log("Reloading: " + currentLevel.name + " index: " + currentLevelScript.GetLevelIndex());

        int currentLevelIndex = currentLevelScript.GetLevelIndex();

        Destroy(currentLevel);

        currentLevel = levelBuilder.Build(currentLevelIndex);
        currentLevel.transform.parent = transform;
        currentLevelScript = currentLevel.GetComponent<Level>();
        currentLevelScript.SetIsCurrentLevel(true);
        currentLevel.transform.position = pos;

    }
}
