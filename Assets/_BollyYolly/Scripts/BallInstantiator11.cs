using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class BallInstantiator11 : MonoBehaviour
{
    public GameObject ironBall;
    public GameObject iceBall;
    public GameObject thunderBall;
    public GameObject speedBall;
    public GameObject fireBall;

    public GameObject redBall;
    public GameObject blueBall;
    public GameObject greenBall;
    public GameObject yellowBall;

    public BallTypes[] arrayOfBallTypes;

    GlobalSceneManager globalSceneManager;

    [SerializeField] private float initialBallSpeed = 4f;
    [SerializeField] Vector3 topRightShell = new Vector3(4.3f, 1.7f, 4.3f);
    [SerializeField] Vector3 topLeftShell = new Vector3(-4.2f, 1.7f, 4.3f);
    [SerializeField] Vector3 bottomRightShell = new Vector3(4.3f, 1.7f, -4.3f);
    [SerializeField] Vector3 bottomLeftShell = new Vector3(-4.2f, 1.7f, -4.2f);
    //[SerializeField] private int shell = 0;
    private Vector3[] shellTranforms = new Vector3[4];
    public bool isLevelTen;

    public static int name = 0;
    public static int FireBallname = 0;

    public event EventHandler<onBallSpawnEventArgs> BallData;

    int levelNumber;
    GameObject ballToInst;
    LevelGameManager levelGameManager;

    [SerializeField] float timeIntervalIceBall = 10f;
    [SerializeField] float timeIntervalSpeedBall = 10f;
    [SerializeField] float timeIntervalThunderBall = 10f;
    [SerializeField] float timeIntervalFireBall = 10f;

    float specialBallWaitTime = 10f;
    float ironBallWaitTime = 5f;
    public class onBallSpawnEventArgs : EventArgs
    {
        public Transform ball;
    }
    void SetInitialSpeed()
    {
        if (levelNumber == 10)
        {
            initialBallSpeed = 2;
        }
    }
    void SetLevelNumber()
    //sets current level number int(digit)
    {
        levelNumber = globalSceneManager.GetCurrentLevelNumber();
    }

    void Start()
    {
        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
        levelGameManager = FindObjectOfType<LevelGameManager>();

        SetLevelNumber();

        shellTranforms[0] = topLeftShell;
        shellTranforms[1] = topRightShell;
        shellTranforms[2] = bottomRightShell;
        shellTranforms[3] = bottomLeftShell;

        if(levelNumber == 14)
        {
            arrayOfBallTypes = new BallTypes[] { BallTypes.Red, BallTypes.Green, BallTypes.Blue, BallTypes.Yellow };
        }
        else
        {
            arrayOfBallTypes = new BallTypes[] { BallTypes.Ice, BallTypes.Thunder, BallTypes.Speed, BallTypes.Fire};
        }
        SetInitialSpeed();
        if (levelNumber != 14)
        {
            StartCoroutine(InstantiateIronBall());
        }
        StartCoroutine(CycleBalls());
    }
    public void InitiateGame()
    {
        StartCoroutine(CycleBalls());
    }
    IEnumerator CycleBalls()
    {
        if (levelNumber == 14) yield break;
        //yield return new WaitWhile(() => !LevelReadyCheck.isLevelReady);
        if(levelNumber == 12)
        {
            yield return new WaitForSeconds(8f);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }

        while (true)
        {
            BallTypes ballToInst = arrayOfBallTypes[UnityEngine.Random.Range(0, arrayOfBallTypes.Length)];
            InstantiateBall(ballToInst);
            yield return new WaitForSeconds(specialBallWaitTime);
        }
    }
    public void InstantiateBall()
    {
        BallTypes ballToInst = arrayOfBallTypes[UnityEngine.Random.Range(0, arrayOfBallTypes.Length)];
        InstantiateBall(ballToInst);
    }
    public void InstantiateBall(BallTypes typeOfBall)
    //instantiates ball according to type passed in parameter
    {
        if (levelGameManager.gameOver || levelGameManager.gettingScores) return;

        switch (typeOfBall)
        {
            case BallTypes.Iron:
                ballToInst = ironBall;
                break;
            case BallTypes.Thunder:
                ballToInst = thunderBall;
                break;
            case BallTypes.Ice:
                ballToInst = iceBall;
                break;
            case BallTypes.Speed:
                ballToInst = speedBall;
                break;
            case BallTypes.Fire:
                ballToInst = fireBall;
                break;
            case BallTypes.Red:
                ballToInst = redBall;
                break;
            case BallTypes.Yellow:
                ballToInst = yellowBall;
                break;
            case BallTypes.Blue:
                ballToInst = blueBall;
                break;
            case BallTypes.Green:
                ballToInst = greenBall;
                break;
        }

        Rigidbody clone = null;
        int randomNum = UnityEngine.Random.Range(0, 4);
        float randomNumForThrow = UnityEngine.Random.Range(-2, 3);
        Vector3 shellSelect = shellTranforms[randomNum];
        Vector3 throwVelocityDirection;
        switch (typeOfBall)
        {
            case BallTypes.Ice:
                shellSelect = shellTranforms[2];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "IceBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
            case BallTypes.Thunder:
                shellSelect = shellTranforms[3];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "ThunderBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z - randomNumForThrow);
                break;
            case BallTypes.Fire:
                shellSelect = shellTranforms[1];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "FireBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
            case BallTypes.Speed:
                shellSelect = shellTranforms[0];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "SpeedBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x - randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
            case BallTypes.Red:
                shellSelect = shellTranforms[1];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "RedBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
            case BallTypes.Yellow:
                shellSelect = shellTranforms[2];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "YellowBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z - randomNumForThrow);
                break;
            case BallTypes.Blue:
                shellSelect = shellTranforms[3];
                Debug.Log(ballToInst);
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "BlueBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
            case BallTypes.Green:
                shellSelect = shellTranforms[0];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "GreenBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x - randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;

            default:
                //InstantiateIronBall();
                return;
                throwVelocityDirection = new Vector3(shellSelect.x - randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
        }
            clone.velocity = transform.TransformDirection(throwVelocityDirection.normalized * -initialBallSpeed);

        Debug.DrawRay(shellSelect, clone.velocity);
    }
    IEnumerator InstantiateIronBall()
    {
        if (levelNumber == 12)
        {
            yield return new WaitForSeconds(3.5f);
        }
        yield return new WaitForSeconds(3.5f);
        while (true)
        {
            ballToInst = ironBall;
            Rigidbody clone;
            int randomNum;
            randomNum = UnityEngine.Random.Range(0, shellTranforms.Length);
            float randomNumForThrow = UnityEngine.Random.Range(-2, 3);
            Vector3 shellSelect = shellTranforms[randomNum];
            clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
            clone.name = "IronBall" + name++;
            Vector3 throwVelocityDirection = new Vector3(0, 0, 0);

            switch (randomNum)
            {
                case 0:
                    throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z - randomNumForThrow);
                    break;
                case 1:
                    throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z - randomNumForThrow);
                    break;
                case 2:
                    throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z - randomNumForThrow);
                    break;
                case 3:
                    throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z - randomNumForThrow);
                    break;
            }
            clone.velocity = transform.TransformDirection(throwVelocityDirection.normalized * -initialBallSpeed);
            Debug.DrawRay(shellSelect, clone.velocity);

            yield return new WaitForSeconds(ironBallWaitTime);
        }
    }
    IEnumerator ThunderBallInstantiation()
    {
        while (true)
        {
            InstantiateBall(BallTypes.Thunder);
            yield return new WaitForSeconds(timeIntervalThunderBall);
        }
    }
    IEnumerator SpeedBallInstantiation()
    {
        while (true)
        {
            InstantiateBall(BallTypes.Speed);
            yield return new WaitForSeconds(timeIntervalSpeedBall);
        }
    }
    IEnumerator FireBallInstantiation()
    {
        while (true)
        {
            InstantiateBall(BallTypes.Fire);
            yield return new WaitForSeconds(timeIntervalFireBall);
        }
    }
    IEnumerator IceBallInstantiation()
    {
        while (true)
        {
            InstantiateBall(BallTypes.Ice);
            yield return new WaitForSeconds(timeIntervalIceBall);
        }
    }
}
