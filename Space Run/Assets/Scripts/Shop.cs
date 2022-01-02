using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Text doubleCoinsTitle;
    [SerializeField] private Text extraLifeTitle;
    [SerializeField] private GameObject removeAdsButton;

    private int doubleCoinsPrice;
    private int extraLifePrice;

    private void Start()
    {
        doubleCoinsPrice = 200;
        extraLifePrice = 100;

        if (PlayerPrefs.GetInt("a") == 1)
        {
            removeAdsButton.SetActive(false);
        }
    }

    private void Update()
    {
        doubleCoinsTitle.text = "Double Coins x" + PlayerPrefs.GetInt("DoubleCoins").ToString();
        extraLifeTitle.text = "Extra Life x" + PlayerPrefs.GetInt("ExtraLife").ToString();
    }

    public void BuyDoubleCoins()
    {
        if (PlayerPrefs.GetInt("Coins") >= doubleCoinsPrice)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - doubleCoinsPrice);
            PlayerPrefs.SetInt("DoubleCoins", PlayerPrefs.GetInt("DoubleCoins") + 1);
        }
    }

    public void BuyExtraLife()
    {
        if (PlayerPrefs.GetInt("Coins") >= extraLifePrice)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - extraLifePrice);
            PlayerPrefs.SetInt("ExtraLife", PlayerPrefs.GetInt("ExtraLife") + 1);
        }
    }

    public void RemoveAds()
    {
        PlayerPrefs.SetInt("a", 1);
    }
}
