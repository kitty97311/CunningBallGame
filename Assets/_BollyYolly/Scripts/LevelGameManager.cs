using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Colour { Red, Yellow, Blue, Green, None};
public class LevelGameManager : MonoBehaviour
{
    CanvasDependencyRemover canvasDependencyRemover;
    GlobalSceneManager globalSceneManager;
    WinLoseController winLoseController;
    GemsUseHandler gemsUseHandler;

    public LevelsScriptable level;
    public GameObject[] typeOfBalls;

    [SerializeField] GameObject[] outBoundaries;
    [SerializeField] GameObject[] playerCars;
    [SerializeField] GameObject[] players;
    [SerializeField] GameObject[] winLosePanel;
    [SerializeField] GameObject scoreText;
    [SerializeField] Text winnerScore;
    ScoreManager[] scoreManagers;

    int player1Score = 0;
    int player2Score = 0;
    int player3Score = 0;
    int player4Score = 0;

    public float maxSpeedBall = 7f;
    int selectCharacter = 0;
    float countDownTimer = 3f;
    int intCountDownTimer = 0;
    float gameTimer;

    public float level4And5Timer = 5f;
    public float level10Timer = 5f;

    public int totalNoOfBalls = 0;

    public Text p1Score;
    public Text p2Score;
    public Text p3Score;
    public Text p4Score;

    public Text countDownTimerText;
    public Text timerTextMinutes;
    public Text timerTextSeconds;

    public GameObject winnerListBanner;
    public Text coin_Text;


    int count = 0;
    int secondsToAdd = 15;//time to add in seconds on gems use
    public bool gettingScores = false;
    public bool ifGemsUsedOnce = false;

    bool iceBallPresent;
    int levelNumber;

    [SerializeField] float timeIntervalIceBall = 10f; 
    [SerializeField] float timeIntervalSpeedBall = 10f;
    [SerializeField] float timeIntervalThunderBall = 10f;
    [SerializeField] float timeIntervalFireBall = 10f;

    GameObject charactersParent;

    BallInstantiator11 ballInstantiator11;

    BallTypes specialBallToInst;
    float timeInterval;
    Dictionary<BallTypes, int> dictOfLevelBallData = new Dictionary<BallTypes, int>();
    private void Awake()
    {
        canvasDependencyRemover = FindObjectOfType<CanvasDependencyRemover>();
        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
        winLoseController = FindObjectOfType<WinLoseController>();
        gemsUseHandler = FindObjectOfType<GemsUseHandler>();

        Time.timeScale = 1;
        gemsUseHandler.OnGemUseButtonClicked += AddToTimer;
    }

    // Start is called before the first frame update
    void Start()
    {
        countDownTimer = level.spawnFrequency;
        gameTimer = level.gameTime;
        totalNoOfBalls = level.maxBallsOnStage - 1;
        scoreManagers = FindObjectsOfType<ScoreManager>();
        player1Score = player2Score = player3Score = player4Score = level.ballToSave;
        ballInstantiator11 = FindObjectOfType<BallInstantiator11>();

        foreach (ScoreManager scoreManager in scoreManagers)
        {
            scoreManager.OnGoalScored += ScoreManager_OnGoalScored;
        }
        //StartCoroutine(InstantiateNewBall());

        if (PlayerPrefs.HasKey("Player1Selection"))
        {
            selectCharacter = PlayerPrefs.GetInt("Player1Selection");
        }

        ReleaseBall();
        InstantiatePlayers();
        winLoseController.LoadPlayers();

        if (levelNumber == 12)
        {
            countDownTimer = 6;
        }
        if (levelNumber == 7 || levelNumber == 3 || levelNumber == 9 || levelNumber == 5) // TODO can be simplified, after discussion
        {
            StartCoroutine(InstantiateSpecialBall());
        }
        if (FindObjectOfType<CharacterForGameOver>() == null) return;
        charactersParent = FindObjectOfType<CharacterForGameOver>().gameObject;
    }
    public void SetLevelSpecificBallINtervalValues(float fireBallTime, float speedBallTime, float thunderBallTime, float iceBallTime)
    {
        timeIntervalIceBall = iceBallTime;
        timeIntervalFireBall = fireBallTime;
        timeIntervalSpeedBall = speedBallTime;
        timeIntervalThunderBall = thunderBallTime;
    }

    public bool gameOver;

    void FixedUpdate()
    {
        HandleSfx();

        if (gettingScores) return;

        GameTimerAndCountdownTimeManager();

        canvasDependencyRemover.P1Score.text = player1Score.ToString();
        canvasDependencyRemover.P2Score.text = player2Score.ToString();
        canvasDependencyRemover.P3Score.text = player3Score.ToString();
        canvasDependencyRemover.P4Score.text = player4Score.ToString();

        WinLoseManager();

        if (PlayerPrefs.GetString("SelectedLevel") == "Level 8" || PlayerPrefs.GetString("SelectedLevel") == "Level 6")
        {
            //additional counter specially for level 6 and 8
            level4And5Timer -= Time.deltaTime;
            if (level4And5Timer < 0f)
            {

                StartCoroutine(InstantiateNewBall());
                level4And5Timer = 5f;
            }
        }
    }
    public void HandleSfx()
    {
        if (PlayerInstance.Instance.Setting.sfx)
        {
            GetComponent<AudioSource>().volume = 1;
        }
        else
        {
            GetComponent<AudioSource>().volume = 0;
        }
    }
    private void WinLoseManager()
    {
        //if (CountActiveChildren() == 2)
        //{
        //    ForceEndGame();
        //}
        if (gameTimer < 0f)
        {
            gettingScores = true;

            canvasDependencyRemover.coin_Text.text = "Coins:" + Utility.NumberToWordConverted(PlayerInstance.Instance.Player.coin);
            canvasDependencyRemover.ScoreText.SetActive(false);

            if (gameOver == false)
            {
                gameOver = true;

                Banners();

                winLoseController.GetAllScores();
                //calls initiation of getting scores in the said script and then publish score at the game-end screen

                GameEndProcesses(); //TODO
            }
        }
    }
    public void ForceEndGame()
    {
        gameTimer = -1;
        Debug.Log("ending Game");
    }
    int CountActiveChildren()
    {
        int totalActiveChildren = 0;

        if (charactersParent == null) return 5;

        foreach (Transform child in charactersParent.transform)
        {
            if(child.gameObject.activeSelf == true)
            {
                totalActiveChildren++;
            }
        }
        if(charactersParent.transform.GetChild(0).gameObject.activeSelf == false)
        {
            totalActiveChildren = 2;
            Debug.Log("GameOver");
        }
        return totalActiveChildren;
    }
    private void GameTimerAndCountdownTimeManager()
    {
        canvasDependencyRemover.CountdownTimerText.enabled = false;
        if (countDownTimer <= 3.0f)
        {
            canvasDependencyRemover.CountdownTimerText.enabled = true;
        }
        if (gameTimer >= 0f)
        {
            canvasDependencyRemover.TimerTextMinutes.text = ((int)gameTimer / 60).ToString("00");
            canvasDependencyRemover.TimerTextSeconds.text = ((int)gameTimer % 60).ToString("00");
        }

        if (countDownTimer > 0)
        {
            countDownTimer -= Time.deltaTime;
            intCountDownTimer = (int)countDownTimer;
            canvasDependencyRemover.CountdownTimerText.text = (intCountDownTimer + 1).ToString();
        }
        else
        {
            gameTimer -= Time.deltaTime;
            canvasDependencyRemover.CountdownTimerText.enabled = false;
        }
    }

    public void AddToTimer()
    //adds a predecided amount of seconds to 
    //to the game timer for cost of one gem
    {
        if (PlayerInstance.Instance.Player.gem <= 0)
        //checks if no sufficient gems balance available
        //exits method if gems zero
        {
            Toast.Instance.ShowToast("Insufficient gem!!!");
            return;
        }

        if (gameTimer <= 1) return;
        //returns if game times is already to 1
        gameTimer += secondsToAdd;
        //add to timer variable
    }
    public int[] GetAllScoresForRanking()
    //return all score for sorting and inturn ranking 
    {
        int[] allScoresArray = new int[4];

        return AddAllScoresToDict(allScoresArray);
    }
    private int[] AddAllScoresToDict(int[] allScoresDict)
    //adds all score to dict (sorted as p1 to p4 for indices 0-3)
    {
        allScoresDict[0] = (player1Score);
        allScoresDict[1] = (player2Score);
        allScoresDict[2] = (player3Score);
        allScoresDict[3] = (player4Score);

        return allScoresDict;
    }
    private void GameEndProcesses()
    //processes at the end of game
    {
        GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");
        //gets all balls in the scene
        //and them sestroy all
        foreach (GameObject item in allBalls)
        {
            Destroy(item);
        }
    }
    void Banners()
    {
        canvasDependencyRemover.WinnerListBanner.SetActive(true);
        canvasDependencyRemover.coin_Text.text = "Coins:" + Utility.NumberToWordConverted(PlayerInstance.Instance.Player.coin);
    }

    private void InstantiatePlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(playerCars[(i + selectCharacter) % 4], players[i].transform);
        }
    }
    private void ScoreManager_OnGoalScored(object sender, ScoreManager.OnGoalScoredEventArgs e)
    {
        //TODO HARSH: UPDATE THIS CODE


        GetComponent<AudioSource>().Play();

        if (levelNumber == 14 || levelNumber == 15)
        {
            ProcessColouredBallsScores(sender, e);
        }
        else
        {
            //TODO REASON FOR CRASH FROM HERE TO

            winLoseController.UpdateLeaderboad(e.playerTag);

            switch (e.playerTag)
            {
                case "Player1":
                    player1Score++;
                    if (e.typeOfBall == BallTypes.Iron) return;
                    ProcessSpecialBallEffects(0, e.typeOfBall);
                    break;
                case "Player2":
                    player2Score++;
                    if (e.typeOfBall == BallTypes.Iron) return;
                    ProcessSpecialBallEffects(1, e.typeOfBall);
                    break;
                case "Player3":
                    player3Score++;
                    if (e.typeOfBall == BallTypes.Iron) return;
                    ProcessSpecialBallEffects(2, e.typeOfBall);
                    break;
                case "Player4":
                    player4Score++;
                    if (e.typeOfBall == BallTypes.Iron) return;
                    ProcessSpecialBallEffects(3, e.typeOfBall);
                    break;
                default:
                    Debug.Log(" Detecting crash 8 " + Time.timeSinceLevelLoadAsDouble);
                    break;
            }
            //TODO REASON FOR CRASH TILL HERE 

        }
        if (totalNoOfBalls >= 0)
        {
            totalNoOfBalls--;
        }
    }
    void ProcessColouredBallsScores(object sender, ScoreManager.OnGoalScoredEventArgs e)
    {
        switch (e.playerTag)
        {
            case "Player1":
                if(e.typeOfBall == BallTypes.Yellow)
                {
                    player1Score++;
                    winLoseController.UpdateLeaderboad("Player1");
                    //ProcessCorrectColourMatchForScore(e);
                }
                else if (!e.ifIsColored)
                {
                    player1Score++;
                    if (e.typeOfBall == BallTypes.Iron) return;
                    ProcessSpecialBallEffects(0, e.typeOfBall);
                }
                break;
            case "Player2":
                if (e.typeOfBall == BallTypes.Red)
                {
                    player2Score++;
                    winLoseController.UpdateLeaderboad("Player2");
                    //ProcessCorrectColourMatchForScore(e);
                }
                else if (!e.ifIsColored)
                {
                    player2Score++;
                    if (e.typeOfBall == BallTypes.Iron) return;
                    ProcessSpecialBallEffects(1, e.typeOfBall);
                }
                break;
            case "Player3":
                if (e.typeOfBall == BallTypes.Blue)
                {
                    player3Score++;
                    winLoseController.UpdateLeaderboad("Player3");
                    //ProcessCorrectColourMatchForScore(e);
                }
                else if (!e.ifIsColored)
                {
                    player3Score++;
                    if (e.typeOfBall == BallTypes.Iron) return;
                    ProcessSpecialBallEffects(2, e.typeOfBall);
                }
                break;
            case "Player4":
                if (e.typeOfBall == BallTypes.Green)
                {
                    player4Score++;
                    winLoseController.UpdateLeaderboad("Player4");
                    //ProcessCorrectColourMatchForScore(e);
                }
                else if (!e.ifIsColored)
                {
                    player4Score++;
                    if (e.typeOfBall == BallTypes.Iron) return;
                    ProcessSpecialBallEffects(3, e.typeOfBall);
                }
                break;
            default:
                Debug.Log(" Detecting crash 8 " + Time.timeSinceLevelLoadAsDouble);
                break;
        }

        if (totalNoOfBalls >= 0)
        {
            totalNoOfBalls--;
        }

    }
    //Redundant
    void ProcessCorrectColourMatchForScore(ScoreManager.OnGoalScoredEventArgs e)
    {
        switch (e.lastHitPlayer)
        {
            case 1:
                player1Score++;
                winLoseController.UpdateLeaderboad(e.playerTag);
                break;
            case 2:
                player2Score++;
                winLoseController.UpdateLeaderboad(e.playerTag);
                break;
            case 3:
                player3Score++;
                winLoseController.UpdateLeaderboad(e.playerTag);
                break;
            case 4:
                player4Score++;
                winLoseController.UpdateLeaderboad(e.playerTag);
                break;
            default:
                Debug.Log(" Detecting crash 8 " + Time.timeSinceLevelLoadAsDouble);
                break;
        }
        return;
    }
    void ProcessSpecialBallEffects(int i, BallTypes type)
    //processes different effects according to ball taken
    {
        if (players[i].activeSelf == false) return;

        if (type == BallTypes.Ice)
        {
            if (players[i].GetComponent<PlayerMechanics>() != null)
            {
                players[i].GetComponent<PlayerMechanics>().FreezeOnIceBallTaken();
            }
            if (players[i].GetComponent<CPUAi>() != null)
            {
                players[i].GetComponent<CPUAi>().FreezeOnIceBallTaken();
            }
        }
        if (type == BallTypes.Thunder)
        {
            if (players[i].GetComponent<PlayerMechanics>() != null)
            {
                players[i].GetComponent<PlayerMechanics>().ProcessThunderKilling();
            }
            if (players[i].GetComponent<CPUAi>() != null)
            {
                players[i].GetComponent<CPUAi>().ProcessThunderKilling();
            }
        }
        if (type == BallTypes.Speed)
        {
            if (players[i].GetComponent<PlayerMechanics>() != null)
            {
                players[i].GetComponent<PlayerMechanics>().ProcessSpeedUp();
            }
            if (players[i].GetComponent<CPUAi>() != null)
            {
                players[i].GetComponent<CPUAi>().ProcessSpeedUp();
            }
        }
        if (type == BallTypes.Fire)
        {
            if (players[i].GetComponent<PlayerMechanics>() != null)
            {
                players[i].GetComponent<PlayerMechanics>().ProcessPlayerFireUp();
            }
            if (players[i].GetComponent<CPUAi>() != null)
            {
                players[i].GetComponent<CPUAi>().ProcessPlayerFireUp();
            }
        }
    }
    public void ReleaseBall()
    {
        //Debug.Log("Inst Ball");
        StartCoroutine(InstantiateNewBall());
    }

    IEnumerator InstantiateNewBall()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            if (gettingScores || gameOver) yield break;

            //yield return new WaitForSeconds(level.spawnFrequency);
            yield return new WaitForSeconds(0);

            BallCollisionDetector[] noOfBallPresentAtStage = FindObjectsOfType<BallCollisionDetector>();

            if (noOfBallPresentAtStage.Length < level.howManyCanBePresentInCourt)
            {
                if (levelNumber == 14)
                {
                    ballInstantiator11.InstantiateBall();
                }
                else
                {
                    Debug.Log("Inst 1");
                    if (GetComponent<BallInstantiator>() == null) yield break;
                    GetComponent<BallInstantiator>().InstantiateBall(BallTypes.Iron);
                }
            }
        }
    }
    IEnumerator InstantiateSpecialBall()
    //instantiates special ball according to
    //the current level
    {
        if (gameOver || gettingScores) yield break;

        switch (levelNumber)
        {
            case 7:
                specialBallToInst = BallTypes.Ice;
                timeInterval = timeIntervalIceBall;
                break;
            case 3:
                specialBallToInst = BallTypes.Thunder;
                timeInterval = timeIntervalThunderBall;
                break;
            case 9:
                specialBallToInst = BallTypes.Speed;
                timeInterval = timeIntervalSpeedBall;
                break;
            case 5:
                specialBallToInst = BallTypes.Fire;
                timeInterval = timeIntervalFireBall;
                break;
        }
        while (true)
        {
            yield return new WaitForSeconds(timeInterval);
            GetComponent<BallInstantiator>().InstantiateBall(specialBallToInst);
        }
    }
    public void RestartLevel()
    {
        Scene currentActiveScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentActiveScene.buildIndex);
    }

    public void GoToLevels()
    {
        SceneManager.LoadScene(Constants.SCENE_LEVEL_SELECTION);
    }
}
