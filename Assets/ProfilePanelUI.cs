using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfilePanelUI : MonoBehaviour
{
    [SerializeField] GameObject usernameChangePanel;
    //username panel reference (not in use)
    [SerializeField] Text usernameText;
    //text ref on input field
    [SerializeField] Text usernameInputFieldText;
    //text ref on input field
    [SerializeField] Text tournamentWinsText;
    //text ref for tournament wins
    [SerializeField] Text placeholderText;
    //text ref for placeholder text of input field(not in use)
    [SerializeField] InputField usernameInputField;
    //ref to input field component
    [SerializeField] Text totalWinsText;
    //text ref for total number of matches won (rank 1 and rank 2)
    [SerializeField] Text totalDeathsText;
    //text ref for total deaths(by special balls)
    [SerializeField] Text totalMatchesPLayedText;
    //text ref for total matches played(completed until Time's Up screen)
    [SerializeField] Text totalCoinsText;
    //text ref for total number of coins on account
    [SerializeField] Text totalGemsText;
    //text ref for total number of gems on account
    [SerializeField] Text winPercentText;
    //text ref for win percent (total matches won(rank1 and rank 2)/ total matches played)
    [SerializeField] Text winningStreakText;
    //winning streak ref/ total back to back wins (not in use)
    [SerializeField] TextMeshProUGUI playerNameHomescreen;
    //text ref to player name on the homescreen(top left)
    InputField inp;
    //ref to input field
    string username;
    //username ref local
    /// <summary>
    /// Gets input field component and saved playername
    /// </summary>
    private void OnEnable()
    {
        //Debug.Log(PlayerInstance.playerInstance.playerData + " Player data Name on Enable");
        //Debug.Log(PlayerInstance.playerInstance.playerData.playerName + " Name on Enable");
        inp = GetComponentInChildren<InputField>();
        //input field ref
        usernameChangePanel.SetActive(false);
        //sets username panel to not active(not in use)
        username = PlayerInstance.playerInstance.playerData.playerName;
        //gets username saved in the file
        inp.onValueChanged.AddListener(delegate { SaveUsername(); });
        //subscribe onValueChanged event and saves username to playerInstance on every change
        CheckAndSetUsername();
        SetValues();
    }
    /// <summary>
    /// Calls SaveUsername() and displays current
    /// final username on the homescreen
    /// </summary>
    private void OnDisable()
    {
        SaveUsername();
        //Debug.Log(PlayerInstance.playerInstance.playerData.playerName + " Name on Disable");
        playerNameHomescreen.text = PlayerInstance.playerInstance.playerData.playerName.ToString();
        //displays current final username on the homescreen
    }
    /// <summary>
    /// Gets text on use input field and saves it to player instance
    /// </summary>
    public void SaveUsername()
    {
        //Debug.Log(usernameInputFieldText.text + " Name on field");
        PlayerInstance.playerInstance.playerData.playerName = usernameInputField.text;
        //Gets text on use input field and saves it to player instance
        PlayerInstance.playerInstance.SavePlayer();
        //saves player data
    }
    /// <summary>
    /// sets current username to input field text
    /// </summary>
    private void CheckAndSetUsername()
    {
        //Debug.Log(PlayerInstance.playerInstance.playerData.playerName + " Name on Set");

        usernameInputField.text = username;
        //sets username to input field text


        // Debug.Log(usernameInputFieldText.text + "Name on Set 1");
        //Debug.Log(PlayerInstance.playerInstance.playerData.playerName + " Name on Set 2");
    }
    /// <summary>
    /// sets values in UI (total matches, wins, deaths, coins, gems)
    /// and calls method to calculate win percentage
    /// </summary>
    void SetValues()
    {
        totalMatchesPLayedText.text = PlayerInstance.playerInstance.playerData.totalMatchesPlayed.ToString();
        totalWinsText.text = PlayerInstance.playerInstance.playerData.totalMatchesWon.ToString();
        totalDeathsText.text = PlayerInstance.playerInstance.playerData.totalDeaths.ToString();
        totalCoinsText.text = Utility.NumberToWordConverted(PlayerInstance.playerInstance.playerData.Coins).ToString();
        totalGemsText.text = PlayerInstance.playerInstance.playerData.Gems.ToString();
        SetWinPercentage();
    }
    /// <summary>
    /// calculates win percentage and sets to the UI
    /// if total matches played are zero win percentage
    /// is set to as '-'
    /// </summary>
    void SetWinPercentage()
    {
        if (PlayerInstance.playerInstance.playerData.totalMatchesPlayed == 0)
        //no matches played/completed
        {
            winPercentText.text = "-";
        }
        else
        {
            winPercentText.text = (((float)PlayerInstance.playerInstance.playerData.totalMatchesWon / (float)PlayerInstance.playerInstance.playerData.totalMatchesPlayed) * 100).ToString().Substring(0,3) + "%";
        }
    }

    ////NOT IS USE///
    public void OpenUsernamePanel()
    {
        usernameChangePanel.SetActive(true);
    }
    public void EditUsername()
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        InputField inp = GetComponentInChildren<InputField>();
        inp.interactable = true;
    }
}
