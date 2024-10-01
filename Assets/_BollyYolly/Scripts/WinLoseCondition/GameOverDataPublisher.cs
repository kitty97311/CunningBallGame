using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverDataPublisher : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;

    [SerializeField] GameObject useGemsButton;

    [SerializeField] Text rank1PlayerName;
    [SerializeField] Text rank1Score;

    [SerializeField] Text rank2PlayerName;
    [SerializeField] Text rank2Score;

    [SerializeField] Text rank3PlayerName;
    [SerializeField] Text rank3Score;

    [SerializeField] Text rank4PlayerName;
    [SerializeField] Text rank4Score;


    [SerializeField] Text[] rankPlayerName;
    [SerializeField] Text[] playerScores;

    [SerializeField] GameObject settingMenuButton;

    WinLoseController winLoseController;

    int[] sortedScoreArray;
    int[] sortedPlayerRefArray;

    [SerializeField] public LevelsScriptable[] levels;

    [SerializeField] Text rank1PrizeAmount;
    [SerializeField] Text rank2PrizeAmount;

    GlobalSceneManager globalSceneManager;

    int currentLevelNumber;
    long player1WinningAmount;

    bool isWinnerDecided;
    public bool isRank1_2;
    void Start()
    {
        winLoseController = GetComponent<WinLoseController>();
        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
        currentLevelNumber = PlayerInstance.Instance.CurrentLevelNumber;
    }

    public void GetFinalScoreData()
    //gets final data to display ond end screen
    //and then calling to publish on screen
    {
        useGemsButton.SetActive(false);

        sortedScoreArray = winLoseController.GetSortedScoreArray;
        sortedPlayerRefArray = winLoseController.GetSortedPlayerRefArray;

        PublishFinalScoreData();
    }

    void PublishFinalScoreData()
    //publish final data accordingly
    {
        rank1PlayerName.text = "Player " + sortedPlayerRefArray[0].ToString();
        rank1Score.text = "Score " + sortedScoreArray[0].ToString();

        rank2PlayerName.text = "Player " + sortedPlayerRefArray[1].ToString();
        rank2Score.text = "Score " + sortedScoreArray[1].ToString();

        rank3PlayerName.text = "Player " + sortedPlayerRefArray[2].ToString();
        rank3Score.text = "Score " + sortedScoreArray[2].ToString();

        rank4PlayerName.text = "Playerffh " + sortedPlayerRefArray[3].ToString();
        rank4Score.text = "Score " + sortedScoreArray[3].ToString();
        AwardCoinsToWinners(sortedPlayerRefArray);
    }

    void AwardCoinsToWinners(int[] rankedPlayersArray)
    //awards coins at the end of the game
    //according to the rank
    {
        long currentLevelFee = levels[PlayerInstance.Instance.CurrentLevelNumber].entryFees;
        if (rankedPlayersArray[0] == 1)
        {
            //kittymark
            /*PlayerInstance.Instance.Settttt.eligibleForLevel = currentLevel;*/
            //set highest level won
            PlayerInstance.Instance.AddCoins(currentLevelFee * 4);
            //adds coin awards for Rank1
        }
        //rankedPlayersArray[0] = // add coins 4 times the price of entry
        //rankedPlayersArray[1] = // add coins equal to the entry fee

    }


    /* ---- added by Harshdeep ---- */
    internal void PublishFinalScoreData(Dictionary<string, int> playersWithScrores)
    //publish final data accordingly
    {
        PlayerInstance.Instance.IncreaseMatch();
        rank1PrizeAmount.text = " " + Utility.NumberToWordConverted((levels[PlayerInstance.Instance.CurrentLevelNumber].entryFees * 4)).ToString();
        rank2PrizeAmount.text = " " + Utility.NumberToWordConverted((levels[PlayerInstance.Instance.CurrentLevelNumber].entryFees)).ToString();
        //TODO: Remove foreach form here and move it to WinLoseCalculation
        {
            int i = 0;
            foreach (KeyValuePair<string, int> author in playersWithScrores.OrderBy(key => key.Value))
            {
                rankPlayerName[i].text = author.Key;
                playerScores[i].text = author.Value.ToString() + " points";

                if (author.Key == "Player1")
                //to highlight the current player(on game over screen) 
                {
                    //playerScores[i].transform.parent.GetChild(0).gameObject.SetActive(true);
                    SetCurrentPlayerNameOnScore(i);
                }
                if (author.Key == "Player1" && (i == 0 || i == 1)) // If player1 is on 1st OR 2nd postion, then give him reward and unlock next level 2
                {
                    //isRank1_2 = true;
                    if (i == 0 && !isWinnerDecided)
                    {
                        //gameOverScreen.transform.GetChild(0).GetComponent<Text>().text = "YOU WON";
                        PlayerInstance.Instance.IncreaseWin();

                        isWinnerDecided = true;
                    }
                    RewardUser(i); // Need to assign the reward as per position
                    UnlockLevel();
                }
                else if (!isWinnerDecided)
                {
                    //kittymark
                    /*PlayerInstance.Instance.Settttt.winningStreak = 0;*/
                    //gameOverScreen.transform.GetChild(0).GetComponent<Text>().text = "YOU LOST";
                }
                if (author.Key == "Player1")
                {
                    SetPlayer1WinningAmount(i);
                }
                i++;
            }
            isWinnerDecided = false;
            settingMenuButton.SetActive(false);
        }
    }
    void SetCurrentPlayerNameOnScore(int playerRank)
    {
        switch (playerRank)
        {
            case 0:
                if (PlayerInstance.Instance.Player.name == null) break;
                rank1PlayerName.text = PlayerInstance.Instance.Player.name.ToString();
                break;
            case 1:
                if (PlayerInstance.Instance.Player.name == null) break;
                rank2PlayerName.text = PlayerInstance.Instance.Player.name.ToString();
                break;
            case 2:
                if (PlayerInstance.Instance.Player.name == null) break;
                rank3PlayerName.text = PlayerInstance.Instance.Player.name.ToString();
                break;
            case 3:
                if (PlayerInstance.Instance.Player.name == null) break;
                rank4PlayerName.text = PlayerInstance.Instance.Player.name.ToString();
                break;
        }
    }

    //TODO: Remove below 2 methods 'RewardUser()' & 'UnlockLevel()' form here and move it to WinLoseCalculation
    private void RewardUser(int playerPosition)
    {
        PlayerInstance.Instance.AddCoins(playerPosition == 0
                        ? levels[PlayerInstance.Instance.CurrentLevelNumber].entryFees * 4 // As 25% reward
                        : levels[PlayerInstance.Instance.CurrentLevelNumber].entryFees
                        );


        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
    }
    void SetPlayer1WinningAmount(int player1Rank)
    {
        if(player1Rank == 0)
        {
            player1WinningAmount = levels[PlayerInstance.Instance.CurrentLevelNumber].entryFees * 4;
        }
        else if (player1Rank == 1)
        {
            player1WinningAmount = levels[PlayerInstance.Instance.CurrentLevelNumber].entryFees;
        }
        else if (player1Rank == 2 || player1Rank == 3)
        {
            player1WinningAmount = 25;
            //player winning amount if ranks 3 and rank 4
        }
    }
    private void UnlockLevel()
    {
        //kittymark
        /*if (PlayerInstance.Instance.CurrentLevelNumber >= PlayerInstance.Instance.Settttt.eligibleForLevel)
            PlayerInstance.Instance.Settttt.eligibleForLevel++;*/
    }
    public long GetCurrentLevelFees()
    {
        return levels[PlayerInstance.Instance.CurrentLevelNumber].entryFees;
    }
    public long GetCurrentPrizeAmount()
    {
        return player1WinningAmount;
    }
    /* ---- Till here ----*/
}
