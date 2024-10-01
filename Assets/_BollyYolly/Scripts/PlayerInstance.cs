using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerInstance : MonoBehaviour
{
    public int CurrentLevelNumber = 0;
    public string[] enemyNames = { "P1", "P2", "P3" };
    public static PlayerInstance Instance { get; private set; }

    #region Events

    public Action<long> GameCoinsUpdated;
    public Action<int> GameGemsUpdated;

    #endregion
    private void Awake()
    {
        // Ensure this instance is the only one  
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when loading new scenes  
            Player = new PlayerModel
            {
                name = "kitty",
                email = "a@b.c",
                coin = 1997311,
                gem = 311,
                match = 103,
                win = 43,
                death = 35
            };
            LoadSetting();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances  
        }
    }

    internal PlayerModel Player;
    public class PlayerModel
    {
        public string name = "";
        public string email = "";
        public long coin = 0;
        public int gem = 0;
        public int match = 0;
        public int win = 0;
        public int death = 0;
    }
    internal SettingModel Setting;

    [System.Serializable]
    public class SettingModel
    {
        public bool music = true;
        public bool sfx = true;
        public int sensitivity = 9; // Min: 1, Max:9
    }


    // If you charge coin or gem, param should be positive number else negative number. e.g: coin = -100 or gem = 10
    public IEnumerator UpdatePlayerInfo()
    {

        string jsonString = JsonUtility.ToJson(Player);

        // Create a UnityWebRequest for a POST request
        using UnityWebRequest www = new UnityWebRequest("http://192.168.12.139:8000/cunning-updateplayerinfo", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return www.SendWebRequest();

        JObject resJson = JObject.Parse(www.downloadHandler.text);
        // Check for errors
        if (www.responseCode == 200)
        {
            JObject playerJson = JObject.Parse(resJson["player"].ToString());
            Player = new PlayerModel
            {
                email = playerJson["email"].ToString(),
                coin = (long)playerJson["coin"],
                gem = (int)playerJson["gem"],
                match = (int)playerJson["match"],
                win = (int)playerJson["win"],
                death = (int)playerJson["death"]
            };
            Debug.Log("Player info updated");
        }
        else
        {
            Toast.Instance.ShowToast("Connection error!");
        }
    }

    // If you charge coin or gem, param should be positive number else negative number. e.g: coin = -100 or gem = 10
    private IEnumerator GetPlayerInfo(string email)
    {
        PlayerModel player = new PlayerModel { email = email };
        string jsonString = JsonUtility.ToJson(player);

        // Create a UnityWebRequest for a POST request
        using UnityWebRequest www = new UnityWebRequest("http://192.168.12.139:8000/cunning-getplayerinfo", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return www.SendWebRequest();

        JObject resJson = JObject.Parse(www.downloadHandler.text);
        // Check for errors
        if (www.responseCode == 200)
        {
            JObject playerJson = JObject.Parse(resJson["player"].ToString());
            Player = new PlayerModel
            {
                email = playerJson["email"].ToString(),
                coin = (long)playerJson["coin"],
                gem = (int)playerJson["gem"],
                match = (int)playerJson["match"],
                win = (int)playerJson["win"],
                death = (int)playerJson["death"]
            };
        }
        else
        {
            Toast.Instance.ShowToast("Connection error!");
        }
    }

    #region Save Cache Data
    public void SaveSetting()
    {
        string path = Application.persistentDataPath + "/settings.json";

        // Serialize the setting object to a JSON string
        string jsonData = JsonUtility.ToJson(Setting);

        // Write the JSON string to a file
        File.WriteAllText(path, jsonData);

        Debug.Log("Settings saved: " + jsonData);
    }

    public void LoadSetting()
    {
        string path = Application.persistentDataPath + "/settings.json";

        if (File.Exists(path))
        {
            // Read the JSON data from the file
            string jsonData = File.ReadAllText(path);

            // Deserialize the JSON data back into the Setting object
            Setting = JsonUtility.FromJson<SettingModel>(jsonData);

            Debug.Log("Settings loaded: " + jsonData);
        }
        else
        {
            Debug.LogWarning("No settings file found.");
        }
    }
    #endregion

    #region Currency Management

    public void AddCoins(long amount)
    {
        Player.coin += amount;
        StartCoroutine(UpdatePlayerInfo());
    }

    public void DeductCoins(long amount)
    {
        Player.coin -= amount;
        StartCoroutine(UpdatePlayerInfo());
    }

    internal void AddGems(int amount)
    {
        Player.gem += amount;
        StartCoroutine(UpdatePlayerInfo());
    }
    internal void DeductGems(int amount)
    {
        Player.gem -= amount;
        StartCoroutine(UpdatePlayerInfo());
    }
    internal void IncreaseMatch()
    {
        Player.match ++;
        StartCoroutine(UpdatePlayerInfo());
    }
    internal void IncreaseWin()
    {
        Player.win ++;
        StartCoroutine(UpdatePlayerInfo());
    }
    internal void IncreaseDeath()
    {
        Player.death ++;
        StartCoroutine(UpdatePlayerInfo());
    }

    #endregion


}