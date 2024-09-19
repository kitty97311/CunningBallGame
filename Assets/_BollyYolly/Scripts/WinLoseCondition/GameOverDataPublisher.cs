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
        currentLevelNumber = globalSceneManager.GetCurrentLevelNumber();
        currentLevelNumber = globalSceneManager.GetCurrentLevelNumber();
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
        int currentLevel = PlayerInstance.playerInstance.playerData.currentLevelNumber;
        long currentLevelFee = levels[PlayerInstance.playerInstance.playerData.currentLevelNumber].entryFees;
        if (rankedPlayersArray[0] == 1)
        {
            PlayerInstance.playerInstance.playerData.eligibleForLevel = currentLevel;
            //set highest level won
            PlayerInstance.playerInstance.AddCoins(currentLevelFee * 4);
            //adds coin awards for Rank1
        }
        //rankedPlayersArray[0] = // add coins 4 times the price of entry
        //rankedPlayersArray[1] = // add coins equal to the entry fee

    }


    /* ---- added by Harshdeep ---- */
    internal void PublishFinalScoreData(Dictionary<string, int> playersWithScrores)
    //publish final data accordingly
    {
        PlayerInstance.playerInstance.playerData.totalMatchesPlayed++;
        rank1PrizeAmount.text = " " + Utility.NumberToWordConverted((levels[PlayerInstance.playerInstance.playerData.currentLevelNumber].entryFees * 4)).ToString();
        rank2PrizeAmount.text = " " + Utility.NumberToWordConverted((levels[PlayerInstance.playerInstance.playerData.currentLevelNumber].entryFees)).ToString();
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
                        PlayerInstance.playerInstance.playerData.totalMatchesWon++;
                        PlayerInstance.playerInstance.playerData.winningStreak++;
                        PlayerInstance.playerInstance.SavePlayer();

                        isWinnerDecided = true;
                    }
                    RewardUser(i); // Need to assign the reward as per position
                    UnlockLevel();
                }
                else if (!isWinnerDecided)
                {
                    PlayerInstance.playerInstance.playerData.winningStreak = 0;
                    //gameOverScreen.transform.GetChild(0).GetComponent<Text>().text = "YOU LOST";
                }
                if (author.Key == "Player1")
                {
                    SetPlayer1WinningAmount(i);
                }
                i++;
            }
            isWinnerDecided = false;
            PlayerInstance.playerInstance.SavePlayer();
            settingMenuButton.SetActive(false);
        }
    }
    void SetCurrentPlayerNameOnScore(int playerRank)
    {
        switch (playerRank)
        {
            case 0:
                if (PlayerInstance.playerInstance.playerData.playerName == null) break;
                rank1PlayerName.text = PlayerInstance.playerInstance.playerData.playerName.ToString();
                break;
            case 1:
                if (PlayerInstance.playerInstance.playerData.playerName == null) break;
                rank2PlayerName.text = PlayerInstance.playerInstance.playerData.playerName.ToString();
                break;
            case 2:
                if (PlayerInstance.playerInstance.playerData.playerName == null) break;
                rank3PlayerName.text = PlayerInstance.playerInstance.playerData.playerName.ToString();
                break;
            case 3:
                if (PlayerInstance.playerInstance.playerData.playerName == null) break;
                rank4PlayerName.text = PlayerInstance.playerInstance.playerData.playerName.ToString();
                break;
        }
    }

    //TODO: Remove below 2 methods 'RewardUser()' & 'UnlockLevel()' form here and move it to WinLoseCalculation
    private void RewardUser(int playerPosition)
    {
        Debug.Log(PlayerInstance.playerInstance.playerData.currentLevelNumber);
        PlayerInstance.playerInstance.AddCoins(playerPosition == 0
                        ? levels[PlayerInstance.playerInstance.playerData.currentLevelNumber].entryFees * 4 // As 25% reward
                        : levels[PlayerInstance.playerInstance.playerData.currentLevelNumber].entryFees
                        );


        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
    }
    void SetPlayer1WinningAmount(int player1Rank)
    {
        if(player1Rank == 0)
        {
            player1WinningAmount = levels[PlayerInstance.playerInstance.playerData.currentLevelNumber].entryFees * 4;
        }
        else if (player1Rank == 1)
        {
            player1WinningAmount = levels[PlayerInstance.playerInstance.playerData.currentLevelNumber].entryFees;
        }
        else if (player1Rank == 2 || player1Rank == 3)
        {
            player1WinningAmount = 25;
            //player winning amount if ranks 3 and rank 4
        }
    }
    private void UnlockLevel()
    {
        if (PlayerInstance.playerInstance.playerData.currentLevelNumber >= PlayerInstance.playerInstance.playerData.eligibleForLevel)
            PlayerInstance.playerInstance.playerData.eligibleForLevel++;
    }
    public long GetCurrentLevelFees()
    {
        return levels[PlayerInstance.playerInstance.playerData.currentLevelNumber].entryFees;
    }
    public long GetCurrentPrizeAmount()
    {
        return player1WinningAmount;
    }
    /* ---- Till here ----*/
}
