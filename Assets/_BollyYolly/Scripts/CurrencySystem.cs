using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{

    public static CurrencySystem currencyInstance;

    public int coins; 
    // Start is called before the first frame update
    void Start()
    {
        currencyInstance = this;
        coins = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
