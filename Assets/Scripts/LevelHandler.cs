using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    enum GameState
    {
        play,
        pause,
        menu
    }

    GameState gameState;

    bool toggle = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            toggle = !toggle;

            if (toggle) gameState = GameState.play;
            else gameState = GameState.pause;
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            gameState = GameState.menu;
        }
        handleGameState();
    }

    private void handleGameState()
    {
        switch (gameState)
        {
            case GameState.play:
                PlayMode();
                break;
            case GameState.menu:
                MenuMode();
                break;
            case GameState.pause:
                PauseMode();
                break;
        }
    }

    private void PlayMode()
    {
        Time.timeScale = 1;
        PlayerController.SetAll(true, true, true, true, true, true, true);
    }

    private void PauseMode()
    {
        Time.timeScale = 0;
        PlayerController.SetAll(false, false, false, false, false, false, false);
    }

    private void MenuMode()
    {
        Time.timeScale = 0;
        PlayerController.SetAll(false, false, false, false, true, false, false);
    }

}
