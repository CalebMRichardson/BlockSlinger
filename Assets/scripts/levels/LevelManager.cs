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
    public static string MAX_LEVEL_INDEX = "MAX_LEVEL_INDEX";

    public GameObject northSouthHall;
    public GameObject eastWestHall;
    
    [SerializeField]
    private float northSouthHallHeight;
    [SerializeField]
    private float eastWestHallWidth; 

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

        SpriteRenderer sr = northSouthHall.transform.GetChild(0).GetComponent<SpriteRenderer>();

        northSouthHallHeight = sr.size.y;

        sr = eastWestHall.transform.GetChild(0).GetComponent<SpriteRenderer>();

        eastWestHallWidth = sr.size.x; 

    }

    public void LoadCurrentLevel() {
        levelBuilder = GameObject.Find("LevelBuilder").GetComponent<LevelBuilder>();
        currentLevel = levelBuilder.Build(currentLevelIndex);
        currentLevel.transform.parent = gameObject.transform;
        currentLevelScript = currentLevel.GetComponent<Level>();
        currentLevelScript.SetIsCurrentLevel(true);
        //FocusCameraOnCurrentLevel();
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

        //float offsetX = _nextLvl.transform.position.x - nextLvlScript.GetLevelEnterancePos().x;
        //float offsetY = _nextLvl.transform.position.y - nextLvlScript.GetLevelEnterancePos().y; 

        float offsetX = 0;
        float offsetY = 0;

        if (nextLvlScript.IsNorthEnterance()) {

            offsetX = _nextLvl.transform.position.x - nextLvlScript.GetLevelEnterancePos(true).x;
            offsetY = _nextLvl.transform.position.y - nextLvlScript.GetLevelEnterancePos(true).y;
            nextLevelPos = currentLevelScript.GetLevelExitPos(true);
            offsetY -= 5;
            //TODO get reference to Hallways and move it by -sr.size.y

        } else if (nextLvlScript.IsSouthEnterance()) {
            
            offsetX = _nextLvl.transform.position.x - nextLvlScript.GetLevelEnterancePos(true).x;
            offsetY = _nextLvl.transform.position.y - nextLvlScript.GetLevelEnterancePos(true).y;
            nextLevelPos = currentLevelScript.GetLevelExitPos(true);
            offsetY += 5;
            //TODO get reference to Hallways and move it by +sr.size.y

        } else if (nextLvlScript.IsWestEnterance()) {
            
            offsetX = _nextLvl.transform.position.x - nextLvlScript.GetLevelEnterancePos(false).x;
            offsetY = _nextLvl.transform.position.y - nextLvlScript.GetLevelEnterancePos(false).y;
            nextLevelPos = currentLevelScript.GetLevelExitPos(false);
            offsetX += 5;

        } else if (nextLvlScript.IsEastEnterance()) {
            
            offsetX = _nextLvl.transform.position.x - nextLvlScript.GetLevelEnterancePos(false).x;
            offsetY = _nextLvl.transform.position.y - nextLvlScript.GetLevelEnterancePos(false).y;
            nextLevelPos = currentLevelScript.GetLevelExitPos(false);
            offsetX -= 5;
        }

        //nextLevelPos = currentLevelScript.GetLevelExitPos();

        nextLevelPos.x += offsetX;
        nextLevelPos.y += offsetY; 

        return nextLevelPos;
    }

    private void IncrementCurrentLevel() {

        if (lastLevel != null) {
            Destroy(lastLevel);
        }

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
