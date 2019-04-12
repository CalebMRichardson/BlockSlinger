﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    public static GameStateManager instance = null;

    public GameObject pausePanel;
    public GameObject levelSelection;
    public GameObject levelManager; 
    public static bool gamePaused;

    public enum GameState { PLAY_STATE, LEVEL_SELECTION };
    public GameState gameState;


    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        pausePanel.SetActive(false);
        gamePaused = false;
        gameState = GameState.LEVEL_SELECTION;
    }

    private void Update() {

        if(gameState == GameState.PLAY_STATE) {

            if(Input.GetKeyDown(KeyCode.Escape)) {
                if(gamePaused == false) {
                    PauseGame();
                } else {
                    ResumeGame();
                }
            }
        }
    }

    public void PauseGame() {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        gamePaused = true;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        gamePaused = false;
    }
}
