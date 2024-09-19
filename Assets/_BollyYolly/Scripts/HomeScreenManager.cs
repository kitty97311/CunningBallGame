using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class HomeScreenManager : MonoBehaviour
{
    public GameObject storePanel;

    public Text coins;

    [SerializeField] TextMeshProUGUI usernameText;
    //username ref on homescreen (top left)
    string username;

    void Start()
    {
        if (PlayerInstance.playerInstance.playerData.playerName == null)
        //if no name has been set by the user
        {
            username = Constants.DEFAULT_PLAYER_NAMES[Random.Range(0, 4)];
            //gets a random name out of 4 names (constants script)
            usernameText.text = username;
            //sets text on homescreen
            PlayerInstance.playerInstance.playerData.playerName = username;
            //sets selected random name to player instance 
            PlayerInstance.playerInstance.SavePlayer();
        }
    }
}

//private void Start()
//{
//    PlayerInstance.playerInstance.LoadPlayer();
//    coins.text = "Coins: " + Utility.NumberToWordConverted(PlayerInstance.playerInstance.playerData.Coins);
//}
//public void LevelScreen()
//{
//    SceneManager.LoadScene("Levels Screen");
//    PlayerInstance.playerInstance.SavePlayer();
//}

//public void StoreLoader()
//{
//    storePanel.SetActive(true);
//}

//public void GoToHome()
//{
//    storePanel.SetActive(false);
//}
//private void Update()
//{
//    //coins.text = "Coins:" + PlayerInstance.pb.Conversion(PlayerInstance.pb.data.coins);
//}

//public void BuyCoins()
//{
//    PlayerInstance.playerInstance.playerData.AddCoins(1000);
//    coins.text = "Coins:" + Utility.NumberToWordConverted(PlayerInstance.playerInstance.playerData.Coins);
//    PlayerInstance.playerInstance.SavePlayer();
//}
