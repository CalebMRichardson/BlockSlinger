using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static GameObject currentLevel;
    private static GameObject lastLevel;
    private static GameObject nextLevel;
    private LevelBuilder levelBuilder;
    private Level currentLevelScript;
    private Vector2 currentLevelPos; 

    //TODO read from file
    private int currentLevelIndex;

    private void Start() {
        
        //TODO read this from a file
        currentLevelIndex = 1; 

        levelBuilder = GameObject.Find("LevelBuilder").GetComponent<LevelBuilder>();
        currentLevel = levelBuilder.Build(currentLevelIndex);
        currentLevel.transform.parent = this.gameObject.transform;
        currentLevelScript = currentLevel.GetComponent<Level>();
        currentLevelScript.SetIsCurrentLevel(true);
        FocusCameraOnCurrentLevel();
        nextLevel = LoadNextLevel();
    }

    private GameObject LoadNextLevel() {

        currentLevelIndex++;

        GameObject nextLvl = levelBuilder.Build(currentLevelIndex);
        nextLvl.transform.parent = this.gameObject.transform;
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

        float offsetX = _nextLvl.transform.position.x - nextLvlScript.GetLevelEnterancePos().x;
        float offsetY = _nextLvl.transform.position.y - nextLvlScript.GetLevelEnterancePos().y; 

        if (nextLvlScript.IsNorthEnterance()) {
            offsetY -= 5;
        } else if (nextLvlScript.IsSouthEnterance()) {
            offsetY += 5;
        } else if (nextLvlScript.IsWestEnterance()) {
            offsetX += 10;
        } else if (nextLvlScript.IsEastEnterance()) {
            offsetX -= 10;
        }

        nextLevelPos = currentLevelScript.GetLevelExitPos();

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

        level.SetCameraFocus();
    }

    private void Update() {
        
        if (currentLevelScript.IsLevelComplete()) {
            Debug.Log(currentLevel.name + ": is complete.");
            IncrementCurrentLevel();
        }
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
