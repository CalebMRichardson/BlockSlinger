using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public GameObject pausePanel;

    public static bool gamePaused; 

    private void Start() {
        pausePanel.SetActive(false);
        gamePaused = false;
    }

    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(gamePaused == false) {
                PauseGame();
            } else {
                ResumeGame();
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
