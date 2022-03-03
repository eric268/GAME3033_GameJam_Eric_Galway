using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public PlayerController playerController;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bulletsText;
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
        StartCoroutine(ShowCountdown());
    }

    IEnumerator ShowCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
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

    public void UpdateBulletText(int remainingBullets)
    {

    }
}
