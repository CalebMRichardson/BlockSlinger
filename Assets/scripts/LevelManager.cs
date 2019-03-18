using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static GameObject currentLevel;
    private static GameObject lastLevel;
    private static GameObject nextLevel;
    private LevelBuilder levelBuilder;

    //TODO read from file
    private int currentLevelIndex;

    private void Start() {

        currentLevelIndex = 1; 

        levelBuilder = GameObject.Find("LevelBuilder").GetComponent<LevelBuilder>();
        currentLevel = levelBuilder.Build(currentLevelIndex);
        currentLevel.name = "CurrentLevel";
        currentLevel.transform.parent = this.gameObject.transform;
    }

    public static GameObject GetCurrentLevel() {
        return currentLevel;
    }
}
