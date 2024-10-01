using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class LoginScreenManager : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject registerPanel;
    public InputField loginEmailInput;
    public InputField registerEmailInput;
    public InputField loginPasswordInput;
    public InputField registerPasswordInput;
    public InputField registerNameInput;
    public Text loginEmailErrorText;
    public Text registerEmailErrorText;
    public Text loginPasswordErrorText;
    public Text registerPasswordErrorText;
    public Text registerNameErrorText;
    public GlobalSceneManager sceneManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private class UserModel
    {
        public string email; public string password;
        public string name; public string photo;
        public string location; public string country;
    }

    public void DoLogin()
    {
        StartCoroutine(LoginCoroutine());
    }

    public void DoRegister()
    {
        StartCoroutine(RegisterCoroutine());
    }

    private IEnumerator LoginCoroutine()
    {
        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;
        // Create the JSON data
        UserModel user = new UserModel { email = email, password = password };
        string jsonString = JsonUtility.ToJson(user);

        // Create a UnityWebRequest for a POST request
        using UnityWebRequest www = new UnityWebRequest("http://192.168.12.139:8000/cunning-login", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return www.SendWebRequest();

        JObject resJson = JObject.Parse(www.downloadHandler.text);
        // Check for errors
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            string messageJson = resJson["message"].ToString();
            if (www.responseCode == 401)
            {
                Toast.Instance.ShowToast(messageJson, 3f);
                loginEmailErrorText.text = "";
                loginPasswordErrorText.text = "";
            }
            else
            {
                JObject errorJson = JObject.Parse(resJson["errors"].ToString());
                if (errorJson.ContainsKey("email") && errorJson["email"].HasValues)
                {
                    loginEmailErrorText.text = errorJson["email"][0].ToString();
                }
                else
                {
                    loginEmailErrorText.text = ""; // Or handle the absence of the email error
                }

                // Check if the password attribute exists
                if (errorJson.ContainsKey("password") && errorJson["password"].HasValues)
                {
                    loginPasswordErrorText.text = errorJson["password"][0].ToString();
                }
                else
                {
                    loginPasswordErrorText.text = ""; // Or handle the absence of the password error
                }
            }
        }
        else
        {
            // Handle successful response
            loginEmailErrorText.text = "";
            loginPasswordErrorText.text = "";
            JObject playerData = JObject.Parse(resJson["user"].ToString());
            PlayerInstance.Instance.Player = new PlayerInstance.PlayerModel
            {
                name = playerData["name"].ToString(),
                email = playerData["email"].ToString(),
                coin = (long)playerData["coin"],
                gem = (int)playerData["gem"],
            };
            sceneManager.LoadStartScreen();
        }
    }

    private string location;
    private string country;

    private IEnumerator RegisterCoroutine()
    {
        string email = registerEmailInput.text;
        string password = registerPasswordInput.text;
        string name = registerNameInput.text;
        string photo = "default_user.jpg";

        // Get the public IP address and country code
        yield return StartCoroutine(GetLocationAndCountry());

        // Now, create the user model
        UserModel user = new UserModel { email = email, password = password, name = name, photo = photo, location = location, country = country };
        string jsonString = JsonUtility.ToJson(user);

        // Create a UnityWebRequest for a POST request
        using UnityWebRequest www = new UnityWebRequest("http://192.168.12.139:8000/cunning-register", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return www.SendWebRequest();

        JObject resJson = JObject.Parse(www.downloadHandler.text);
        // Check for errors
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            string messageJson = resJson["message"].ToString();
            JObject errorJson = JObject.Parse(resJson["errors"].ToString());
            if (errorJson.ContainsKey("email") && errorJson["email"].HasValues)
            {
                registerEmailErrorText.text = errorJson["email"][0].ToString();
            }
            else
            {
                registerEmailErrorText.text = ""; // Or handle the absence of the email error
            }

            // Check if the password attribute exists
            if (errorJson.ContainsKey("password") && errorJson["password"].HasValues)
            {
                registerPasswordErrorText.text = errorJson["password"][0].ToString();
            }
            else
            {
                registerPasswordErrorText.text = ""; // Or handle the absence of the password error
            }

            if (errorJson.ContainsKey("name") && errorJson["name"].HasValues)
            {
                registerNameErrorText.text = errorJson["name"][0].ToString();
            }
            else if (name.Contains(" "))
            {
                registerNameErrorText.text = "User name don't have to contain spaces.";
            }
            else
            {
                registerNameErrorText.text = ""; // Or handle the absence of the password error
            }
        }
        else
        {
            ShowLoginOrRegister(true);
        }
    }

    public void ShowLoginOrRegister(bool isLogin)
    {
        registerPanel.SetActive(!isLogin);
        loginPanel.SetActive(isLogin);
    }

    private IEnumerator GetLocationAndCountry()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://ipinfo.io/json"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                location = ""; // Return empty values on error
                country = "";
            }
            else
            {
                // Parse the response JSON
                JObject ipInfo = JObject.Parse(www.downloadHandler.text);
                location = ipInfo["ip"].ToString(); // Get the IP address
                country = ipInfo["country"].ToString(); // Get the country code
            }
        }
    }

}
