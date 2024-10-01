using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDependencyRemover : MonoBehaviour
{
    [SerializeField] GameObject[] winLosePanel;
    [SerializeField] GameObject settingsPanel;

    [SerializeField] GameObject scoreText;
    [SerializeField] Text winnerScore;
    [SerializeField] Text playerName;

    public Text p1Score;
    public Text p2Score;
    public Text p3Score;
    public Text p4Score;

    public Text countDownTimerText;
    public Text timerTextMinutes;
    public Text timerTextSeconds;

    [Header("Winner List")]
    public Text firstPlace;
    public Text secondPlace; 
    public Text thirdPlace;
    public Text fourthPlace;

    public GameObject winnerListBanner;
    public Text coin_Text;

    [SerializeField] GameObject sesitivityPanel;

    private void Start()
    {
        if (PlayerInstance.Instance.Player.name == null) return;
        playerName.text = PlayerInstance.Instance.Player.name.ToString();
    }

    public void OpenSensitivityPanel()
    {
        Time.timeScale = 0;
        sesitivityPanel.SetActive(true);
    }
    public void OpenSettingsPanel()
    {
        Time.timeScale = 0;
        settingsPanel.SetActive(true);
    }
    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public Text CountdownTimerText
    {
        get { return countDownTimerText; }
        set { countDownTimerText = value; }
    }
    public Text TimerTextSeconds
    {
        get { return timerTextSeconds; }
        set { timerTextSeconds = value; }
    }
    public Text TimerTextMinutes
    {
        get { return timerTextMinutes; }
        set { timerTextMinutes = value; }
    }

    public Text P1Score
    {
        get { return p1Score; }
        set { p1Score = value; }
    }
    public Text P2Score
    {
        get { return p2Score; }
        set { p2Score = value; }
    }
    public Text P3Score
    {
        get { return p3Score; }
        set { p3Score = value; }
    }
    public Text P4Score
    {
        get { return p4Score; }
        set { p4Score = value; }
    }

    Text CoinText
    {
        get { return coin_Text; }
        set { coin_Text = value; }
    }
    public GameObject ScoreText
    {
        get { return scoreText; }
        set { scoreText = value; }
    }
    public GameObject WinnerListBanner
    {
        get { return winnerListBanner; }
        set { winnerListBanner = value; }
    }
}
