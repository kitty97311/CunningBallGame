using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class WinLoseController : MonoBehaviour
{
    LevelGameManager levelGameManager;
    GameOverDataPublisher gameOverDataPublisher;
    GlobalSceneManager globalSceneManager;

    //[SerializeField] GameObject gameOverScreen;

    int[] arrayOfPlayersSortedRankWise;
    //to store players score for sorting
    int[] arrayOfPlayerReference;
    //to store player name as ref
    double[] arrayOfLastGoalTime = new double[4];
    //double type coll. to store last goeal time
    //against a particular player

    private void Awake()
    {
        levelGameManager = FindObjectOfType<LevelGameManager>();
        gameOverDataPublisher = GetComponent<GameOverDataPublisher>();
        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
    }
    void Start()
    {
        arrayOfPlayerReference = new int[] { 1, 2, 3, 4 };
        //1,2,3,4 are mock references for player1, player2, player3, and player4 respectively 
    }
    public void GetAllScores()
    //gets scores and goal time arrays
    //and calls sorting of the score
    {
        /* 
         * Commneted by Harshdeep on date 12 July bcz of Crash issue
         
        Debug.Log("Game Over 8 ");
        arrayOfPlayersSortedRankWise = levelGameManager.GetAllScoresForRanking();
        //getting all final scores
        Debug.Log("Game Over 11");

        SortScoreInIncreasingOrder(arrayOfPlayersSortedRankWise);

        Till here*/
        gameOverDataPublisher.PublishFinalScoreData(playerWithScores);
    }

    
    //gets called in level game manager script
    //adds latest goal time against a particular player
    public void AddTimeToArrayOnPlayerIndex(int playerNumber, double timeOfGoal)
    {
        switch (playerNumber)
        {
            case 1:
                Debug.Log(" Detecting crash 10++1");
                arrayOfLastGoalTime[0] = timeOfGoal;
                break;
            case 2:
                Debug.Log(" Detecting crash 10++");
                arrayOfLastGoalTime[1] = timeOfGoal;
                break;
            case 3:
                Debug.Log(" Detecting crash 11++");
                arrayOfLastGoalTime[2] = timeOfGoal;
                break;
            case 4:
                Debug.Log(" Detecting crash 12++");
                arrayOfLastGoalTime[3] = timeOfGoal;
                break;
        }
    }
    void SortScoreInIncreasingOrder(int[] array)
    //sort scores in increasing order
    //and respectively sorts player name
    //reference array
    //and calls to publish score after sorting
    {
        int[] initialScoreArray = array;
        var itemMoved = false;
        do
        {
            itemMoved = false;
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] > array[i + 1])
                {
                    var lowerValue = array[i + 1];
                    array[i + 1] = array[i];
                    array[i] = lowerValue;

                    var lowerScorePlayer = arrayOfPlayerReference[i + 1];
                    arrayOfPlayerReference[i + 1] = arrayOfPlayerReference[i];
                    arrayOfPlayerReference[i] = lowerScorePlayer;

                    itemMoved = true;
                }
                else if (array[i] == array[i + 1])
                {
                    if (arrayOfLastGoalTime[i] < arrayOfLastGoalTime[i + 1])
                    {
                        var lowerValue = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = lowerValue;

                        var lowerScorePlayer = arrayOfPlayerReference[i + 1];
                        arrayOfPlayerReference[i + 1] = arrayOfPlayerReference[i];
                        arrayOfPlayerReference[i] = lowerScorePlayer;

                        itemMoved = true;
                    }
                }
            }
        } while (itemMoved);

        CallToPublishScore(initialScoreArray, array);
    }
    void CallToPublishScore(int[] initialScoreArray, int[] array)
    //call publish in gameOverDataPublisher script
    {
        gameOverDataPublisher.GetFinalScoreData();
    }

    public int[] GetSortedScoreArray
    {
        get { return arrayOfPlayersSortedRankWise; }
    }
    public int[] GetSortedPlayerRefArray
    {
        get { return arrayOfPlayerReference; }
    }


    /*---Temporary code------
     * Added by Harshdeep on date 12 July 2022 for resolving the crash issue and for calculation of the ranking*/


    [HideInInspector] public Dictionary<string, int> playerWithScores;

    internal void LoadPlayers()
    {
        playerWithScores = new Dictionary<string, int>();

        //adding inveresed learderboard bcz of sorting at the last
        //Dict
        playerWithScores.Add("Player1", 0);
        playerWithScores.Add("Player2", 0);
        playerWithScores.Add("Player3", 0);
        playerWithScores.Add("Player4", 0);
    }

    /*
     * First ball went to player 2
     * 1. Need to make the list Player2, player1, player 3, player4
     * 2. Then ball went to player 1
     * 3. Need to update the list Player2, player1, player3, player4
     * 4. Then ball went to player 4
     * 5. Need to update the list player2, player1, player4, player3
     * 6. Then ball went to player 1
     * 7. Need to update the list Player1, player2, player4, player3
     * 8. Need to invert the list to get the sorted leaderboard 
     */

    internal void UpdateLeaderboad(string scoredByPlayer)
    {
        playerWithScores[scoredByPlayer] += 1;
    }

    public void CalculateResults()
    {
        //if (currentLevelNumber == 14)
        //{
        //    foreach (KeyValuePair<string, int> author in playerWithScores.OrderByDescending(key => key.Value))
        //    {
        //        Debug.LogError("Key " + author.Key + " Value: " + author.Value);
        //    }
        //}
        //else
        {
            foreach (KeyValuePair<string, int> author in playerWithScores.OrderBy(key => key.Value))
            {
                Debug.LogError("Key " + author.Key + " Value: " + author.Value);
            }
        }
    }

    public void DeleteAll()
    {
        playerWithScores.Clear();
    }
   /* ----------- Till here -------- */
}