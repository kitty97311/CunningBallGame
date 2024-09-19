using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeCoinsButton : MonoBehaviour
{
    [SerializeField] Image blockerImage;
    GameOverDataPublisher gameOverDataPublisher;
    private void Awake()
    {
        gameOverDataPublisher = FindObjectOfType<GameOverDataPublisher>();
    }
    private void Start()
    {
        //if (!gameOverDataPublisher.isRank1_2)
        //{
        //    gameObject.GetComponent<Button>().interactable = false;
        //    blockerImage.gameObject.SetActive(true);
        //}
    }
    public void GetQuaterFreeCoins()
    {
        GoogleAdsMediation gam = FindObjectOfType<GoogleAdsMediation>();
        gam.ShowRewardedVideo25Coins();
        gameObject.GetComponent<Button>().interactable = false;
        blockerImage.gameObject.SetActive(true);
    }
}
