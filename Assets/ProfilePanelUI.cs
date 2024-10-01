using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfilePanelUI : MonoBehaviour
{
    [SerializeField] Text tournamentWinsText;
    [SerializeField] InputField usernameInputField;
    [SerializeField] Text totalWinsText;
    [SerializeField] Text totalDeathsText;
    [SerializeField] Text totalMatchesPLayedText;
    [SerializeField] Text totalCoinsText;
    [SerializeField] Text totalGemsText;
    [SerializeField] Text winPercentText;
    [SerializeField] Text winningStreakText;
    [SerializeField] TextMeshProUGUI playerNameHomescreen;
    private void OnEnable()
    {
        SetValues();
    }
    /// <summary>
    /// Calls SaveUsername() and displays current
    /// final username on the homescreen
    /// </summary>
    private void OnDisable()
    {
        string username = usernameInputField.text;
        PlayerInstance.Instance.Player.name = username;
        PlayerInstance.Instance.UpdatePlayerInfo();
        playerNameHomescreen.text = username;
    }

    void SetValues()
    {
        usernameInputField.text = PlayerInstance.Instance.Player.name;
        totalMatchesPLayedText.text = PlayerInstance.Instance.Player.match.ToString();
        totalWinsText.text = PlayerInstance.Instance.Player.win.ToString();
        totalDeathsText.text = PlayerInstance.Instance.Player.death.ToString();
        totalCoinsText.text = Utility.NumberToWordConverted(PlayerInstance.Instance.Player.coin).ToString();
        totalGemsText.text = PlayerInstance.Instance.Player.gem.ToString();
        SetWinPercentage();
    }

    void SetWinPercentage()
    {
        if (PlayerInstance.Instance.Player.match == 0)
        {
            winPercentText.text = "-";
        }
        else
        {
            float winPercentage = ((float)PlayerInstance.Instance.Player.win / (float)PlayerInstance.Instance.Player.match) * 100;

            // Check if the percentage is a whole number
            if (winPercentage == Mathf.Floor(winPercentage))
            {
                // Display as an integer percentage
                winPercentText.text = winPercentage.ToString("F0") + "%";
            }
            else
            {
                // Display with one decimal point
                winPercentText.text = winPercentage.ToString("F1") + "%";
            }
        }
    }

}
