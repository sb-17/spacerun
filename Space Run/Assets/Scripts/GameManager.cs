using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject doubleCoinsButton;
    [SerializeField] private GameObject doubleCoinsPriceText;
    [SerializeField] private Text distanceText;
    [SerializeField] private Text startText;
    [SerializeField] private Sprite asteroid1;
    [SerializeField] private Sprite asteroid2;

    public float bgSpeed;
    public float obstacleSpeed;

    public int coinsCollected;
    public int distance;
    public int coinMultiplier;
    private int lastPath;
    private int nextPath;

    public bool gameRunning;

    private List<int> currentLayout = new List<int>();

    private void Start()
    {
        if (PlayerPrefs.GetInt("DoubleCoins") == 0 && PlayerPrefs.GetInt("Coins") >= 250)
            doubleCoinsButton.SetActive(true);

        if (PlayerPrefs.GetInt("DoubleCoins") > 0)
            doubleCoinsButton.SetActive(true);

        if (PlayerPrefs.GetInt("DoubleCoins") == 0)
            doubleCoinsPriceText.SetActive(true);

        gameRunning = false;

        if (PlayerPrefs.GetInt("CoinMultiplier") == 0)
            PlayerPrefs.SetInt("CoinMultiplier", 1);

        bgSpeed = 0.3f;
        if (PlayerPrefs.GetFloat("StartSpeed") == 0f)
            PlayerPrefs.SetFloat("StartSpeed", 1.2f);

        obstacleSpeed = PlayerPrefs.GetFloat("StartSpeed");

        coinsCollected = 0;
        distance = PlayerPrefs.GetInt("StartDistance");

        coinMultiplier = PlayerPrefs.GetInt("CoinMultiplier");

        StartCoroutine(Preparation());
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void BuyDoubleCoins()
    {
        if (PlayerPrefs.GetInt("DoubleCoins") == 0)
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - 250);
        else if (PlayerPrefs.GetInt("DoubleCoins") > 0)
            PlayerPrefs.SetInt("DoubleCoins", PlayerPrefs.GetInt("DoubleCoins") - 1);

        coinMultiplier = 2;

        doubleCoinsButton.SetActive(false);
    }

    IEnumerator Preparation()
    {
        startText.gameObject.SetActive(true);
        startText.text = "3";
        yield return new WaitForSeconds(1f);
        startText.text = "2";
        yield return new WaitForSeconds(1f);
        startText.text = "1";
        yield return new WaitForSeconds(1f);
        startText.gameObject.SetActive(false);

        gameRunning = true;

        StartCoroutine(SpawnObstacles());

        yield return new WaitForSeconds(2f);
        doubleCoinsButton.SetActive(false);
    }

    IEnumerator SpawnObstacles()
    {
        while(gameRunning)
        {
            yield return new WaitForSeconds(1f / obstacleSpeed);

            distance += 1;
            distanceText.text = distance.ToString() + "km";

            if (currentLayout.Count == 0)
            {
                nextPath = Random.Range(0, 3);
                if (nextPath == 0)
                {
                    currentLayout.Add(0);
                    currentLayout.Add(Random.Range(0, 4));
                    currentLayout.Add(Random.Range(0, 4));
                }
                else if (nextPath == 1)
                {
                    currentLayout.Add(Random.Range(0, 4));
                    currentLayout.Add(0);
                    currentLayout.Add(Random.Range(0, 4));
                }
                else if (nextPath == 2)
                {
                    currentLayout.Add(Random.Range(0, 4));
                    currentLayout.Add(Random.Range(0, 4));
                    currentLayout.Add(0);
                }
            }
            else
            {
                if (lastPath == 0)
                {
                    nextPath = Random.Range(0, 2);
                    if (nextPath == 0)
                    {
                        currentLayout[0] = 0;
                        currentLayout[1] = Random.Range(0, 4);
                        currentLayout[2] = Random.Range(0, 4);
                    }
                    else if (nextPath == 1)
                    {
                        currentLayout[0] = 0;
                        currentLayout[1] = 0;
                        currentLayout[2] = Random.Range(0, 4);
                    }
                }
                else if (lastPath == 1)
                {
                    nextPath = Random.Range(0, 3);
                    if (nextPath == 0)
                    {
                        currentLayout[0] = 0;
                        currentLayout[1] = 0;
                        currentLayout[2] = Random.Range(0, 4);
                    }
                    else if (nextPath == 1)
                    {
                        currentLayout[0] = Random.Range(0, 4);
                        currentLayout[1] = 0;
                        currentLayout[2] = Random.Range(0, 4);
                    }
                    else if (nextPath == 2)
                    {
                        currentLayout[0] = Random.Range(0, 4);
                        currentLayout[1] = 0;
                        currentLayout[2] = 0;
                    }
                }
                else if (lastPath == 2)
                {
                    nextPath = Random.Range(1, 3);
                    if (nextPath == 1)
                    {
                        currentLayout[0] = Random.Range(0, 4);
                        currentLayout[1] = 0;
                        currentLayout[2] = 0;
                    }
                    else if (nextPath == 2)
                    {
                        currentLayout[0] = Random.Range(0, 4);
                        currentLayout[1] = Random.Range(0, 4);
                        currentLayout[2] = 0;
                    }
                }
            }

            for (int i = 0; i < currentLayout.Count; i++)
            {
                float xPos = 0f;
                if (i == 0)
                    xPos = -2.2f;
                if (i == 1)
                    xPos = 0f;
                if (i == 2)
                    xPos = 2.2f;

                if (currentLayout[i] == 1 || currentLayout[i] == 2 || currentLayout[i] == 3)
                {
                    GameObject obstacle = Instantiate(obstaclePrefab);
                    obstacle.transform.position = new Vector2(xPos, 4f);
                    int random = Random.Range(1, 3);
                    if (random == 1)
                    {
                        obstacle.GetComponent<SpriteRenderer>().sprite = asteroid1;
                    }
                    else if (random == 2)
                    {
                        obstacle.GetComponent<SpriteRenderer>().sprite = asteroid2;
                    }
                }
                else if (currentLayout[i] == 0)
                {
                    int random = Random.Range(0, 3);
                    if (random != 1)
                    {
                        GameObject coin = Instantiate(coinPrefab);
                        coin.transform.position = new Vector2(xPos, 4f);
                    }
                }
            }

            lastPath = nextPath;

            obstacleSpeed += 0.008f;
        }
    }
}
