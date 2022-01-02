using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    GameManager gameManager;
    AdManager adManager;

    [SerializeField] private Text coinsCollectedText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private Text endCoinsCollectedText;
    [SerializeField] private Text coinsAmountText;
    [SerializeField] private GameObject extraLifeButton;
    [SerializeField] private GameObject endPanel;

    int col;
    bool moved;

    private Vector2 startTouchPosition, currentTouchPosition;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        adManager = GameObject.Find("GoogleAdmobManager").GetComponent<AdManager>();

        col = 2;
        moved = false;
    }

    void Update()
    {
        if (gameManager.gameRunning)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                startTouchPosition = Input.GetTouch(0).position;

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && !moved)
            {
                currentTouchPosition = Input.GetTouch(0).position;

                if ((currentTouchPosition.x < startTouchPosition.x) && col > 1)
                {
                    transform.position = new Vector2(transform.position.x - 2.2f, transform.position.y);
                    col--;
                }

                if ((currentTouchPosition.x > startTouchPosition.x) && col < 3)
                {
                    transform.position = new Vector2(transform.position.x + 2.2f, transform.position.y);
                    col++;
                }

                moved = true;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                moved = false;
        }
    }

    public void BuyExtraLife()
    {
        if (PlayerPrefs.GetInt("ExtraLife") == 0)
        {
            if (PlayerPrefs.GetInt("Coins") >= 150)
            {
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - 150);
                PlayerPrefs.SetInt("ExtraLife", PlayerPrefs.GetInt("ExtraLife") + 1);

                if (PlayerPrefs.GetInt("ExtraLife") > 0)
                {
                    extraLifeButton.SetActive(false);
                }
            }
        }
        else
        {
            PlayerPrefs.SetInt("ExtraLife", PlayerPrefs.GetInt("ExtraLife") - 1);

            PlayerPrefs.SetFloat("StartSpeed", gameManager.obstacleSpeed);
            PlayerPrefs.SetInt("StartCoins", gameManager.coinsCollected);
            PlayerPrefs.SetInt("StartDistance", gameManager.distance);
            PlayerPrefs.SetInt("CoinMultiplier", gameManager.coinMultiplier);

            SceneManager.LoadScene("Game");
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            gameManager.coinsCollected += 1 * gameManager.coinMultiplier;
            coinsCollectedText.text = gameManager.coinsCollected.ToString();

            gameManager.obstacleSpeed += 0.001f;

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            gameManager.gameRunning = false;
            endPanel.SetActive(true);

            adManager.RequestBanner();

            if (gameManager.distance > PlayerPrefs.GetInt("BestScore"))
            {
                PlayerPrefs.SetInt("BestScore", gameManager.distance);
            }

            coinsAmountText.text = PlayerPrefs.GetInt("Coins").ToString();

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + gameManager.coinsCollected);

            scoreText.text = gameManager.distance + "km";
            bestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString() + "km";
            endCoinsCollectedText.text = "Coins collected: " + gameManager.coinsCollected.ToString();

            if (PlayerPrefs.GetInt("ExtraLife") == 0)
            {
                extraLifeButton.SetActive(true);
            }

            PlayerPrefs.SetFloat("StartSpeed", 1.2f);
            PlayerPrefs.SetInt("StartDistance", 0);
            PlayerPrefs.SetInt("CoinMultiplier", 1);
        }
    }
}
