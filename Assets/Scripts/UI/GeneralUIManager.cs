using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GeneralUIManager : MonoBehaviour
{
    public GameObject playGameButton;
    public GameObject mainMenuButton;
    public GameObject instructionsButton;
    public GameObject restartButton;
    public GameObject resumeButton;
    public GameObject creditsButton;
    public GameObject quitButton;

    public void OnStartGameButtonPressed()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OnInstructionsButtonPressed()
    {
        SceneManager.LoadScene("InstructionsScene");
    }

    public void OnCreditsButtonPressed()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void OnResumeGameButton()
    {
        Time.timeScale = 1.0f;
        PlayerController.gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
    }

    public void OnQuitButtonPressed()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
