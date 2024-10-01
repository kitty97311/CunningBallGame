using System;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using UnityEngine;

public class IAPManager : MonoBehaviour
{
    private string coin500 = "com.farisjastania.cunningball.packofcoins";
    private string coin1000 = "com.farisjastania.cunningball.bunchofcoins";
    private string coin100k = "com.farisjastania.cunningball.bagofcoins";
    private string coin10M = "com.farisjastania.cunningball.boxofcoins";
    private string coin1B = "com.farisjastania.cunningball.crateofcoins";
    private string coin100B = "com.farisjastania.cunningball.mountainofcoins";


    private string Gem10 = "com.farisjastania.cunningball.pocketfulgems";
    private string Gem40 = "com.farisjastania.cunningball.pileofgems";
    private string Gem100 = "com.farisjastania.cunningball.bagofgems";
    private string Gem200 = "com.farisjastania.cunningball.boxofgems";
    private string Gem300 = "com.farisjastania.cunningball.chestofgems";

    public void OnPurchaseComplete(Product product)
    {
        // Coins Product
        if (product.definition.id == coin500)
        {
            AddCoins(500);
            Debug.Log("Sucess");
        }
        if (product.definition.id == coin1000)
        {
            AddCoins(1000);
            Debug.Log("Sucess");
        }
        if (product.definition.id == coin100k)
        {
            AddCoins(100000);
            Debug.Log("Sucess");
        }
        if (product.definition.id == coin10M)
        {
            AddCoins(10000000);
            Debug.Log("Sucess");
        }
        if (product.definition.id == coin1B)
        {
            //10110101
            AddCoins(1000000000);
            Debug.Log("Sucess");
        }
        if (product.definition.id == coin100B)
        {
            AddCoins(100000000000);
            Debug.Log("Sucess");
        }

        //Gems Product
        if (product.definition.id == Gem10)
        {
            AddGems(10);
            Debug.Log("Sucess");
        }
        if (product.definition.id == Gem40)
        {
            AddGems(40);
            Debug.Log("Sucess");
        }
        if (product.definition.id == Gem100)
        {
            AddGems(100);
            Debug.Log("Sucess");
        }
        if (product.definition.id == Gem200)
        {
            AddGems(200);
            Debug.Log("Sucess");
        }
        if (product.definition.id == Gem300)
        {
            AddGems(300);
            Debug.Log("Sucess");
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(product.definition.id + "Because Failure" + failureReason); 
    }

    void AddCoins(long coin)
    {
        PlayerInstance.Instance.AddCoins(coin);
    }

    void AddGems(int Gem)
    {
        PlayerInstance.Instance.AddGems(Gem);
    }
}
