using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameOnClick : MonoBehaviour
{
    public void PauseGame() {

        if (GameStateManager.instance.gameState == GameStateManager.GameState.PLAY_STATE) {

            if(GameStateManager.gamePaused) {
                GameStateManager.instance.ResumeGame();
            } else {
                GameStateManager.instance.PauseGame();
            }
        }
    }
}
