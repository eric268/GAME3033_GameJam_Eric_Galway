using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public PlayerController playerController;

    public GameObject restartButton;
    public GameObject mainMenuButton;
    public GameObject quitButton;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bulletsText;
    public TextMeshProUGUI gameOverText;
    public RawImage leftBoarderImage;
    public RawImage rightBoarderImage;
    public static bool gameActive;
    public float boarderMovementSpeed;
    Vector2 leftStartingPosition;
    Vector2 rightStartingPosition;
    float gameOverPosition = 200.0f;
    int timerCounter;

    // Start is called before the first frame update
    void Start()
    {
        gameActive = false;
        leftStartingPosition = leftBoarderImage.rectTransform.anchoredPosition;
        rightStartingPosition = rightBoarderImage.rectTransform.anchoredPosition;
        StartGame();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameActive)
        {
            MoveBoarders();
            bulletsText.text = playerController.gunController.bulletsInClip.ToString() + " / " + playerController.gunController.maxClipSize;
        }
    }

    void StartGame()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(ShowCountdown());
    }

    IEnumerator ShowCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownText.text = "";
        gameActive = true;
        InvokeRepeating("IncrementTimer", 0.0f, 1.0f);
    }

    void MoveBoarders()
    {
        if (leftBoarderImage.rectTransform.anchoredPosition.x <= gameOverPosition)
        {
            leftBoarderImage.rectTransform.anchoredPosition = leftStartingPosition;
            rightBoarderImage.rectTransform.anchoredPosition = rightStartingPosition;
            gameActive = false;
            Time.timeScale = 0.0f;
            GameOver();
        }

        leftBoarderImage.rectTransform.anchoredPosition = new Vector2(leftBoarderImage.rectTransform.anchoredPosition.x - boarderMovementSpeed, leftBoarderImage.rectTransform.anchoredPosition.y);
        rightBoarderImage.rectTransform.anchoredPosition = new Vector2(rightBoarderImage.rectTransform.anchoredPosition.x + boarderMovementSpeed, rightBoarderImage.rectTransform.anchoredPosition.y);
    }

    public void TargeHit(float targetHitMoveAmount)
    {
        if (leftBoarderImage.rectTransform.anchoredPosition.x + targetHitMoveAmount > leftStartingPosition.x)
        {
            leftBoarderImage.rectTransform.anchoredPosition = leftStartingPosition;
            rightBoarderImage.rectTransform.anchoredPosition = rightStartingPosition;
        }
        else
        {
            leftBoarderImage.rectTransform.anchoredPosition = new Vector2(leftBoarderImage.rectTransform.anchoredPosition.x + targetHitMoveAmount, leftBoarderImage.rectTransform.anchoredPosition.y);
            rightBoarderImage.rectTransform.anchoredPosition = new Vector2(rightBoarderImage.rectTransform.anchoredPosition.x - targetHitMoveAmount, rightBoarderImage.rectTransform.anchoredPosition.y);
        }
    }

    public void IncrementTimer()
    {
        timerText.text = timerCounter.ToString();
        timerCounter++;
    }

    void GameOver()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        timerText.text = "";
        bulletsText.text = "";
        gameOverText.text = "Game Over!\n Score:" + timerCounter + " seconds!";
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
        quitButton.SetActive(true);
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenuScene");
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
