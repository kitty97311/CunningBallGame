using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class BallInstantiator15 : MonoBehaviour
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

    [SerializeField] float timeIntervalIceBall = 5f;
    [SerializeField] float timeIntervalSpeedBall = 5f;
    [SerializeField] float timeIntervalThunderBall = 5f;
    [SerializeField] float timeIntervalFireBall = 5f;

    float waitTime = 2f;

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
    
    void Start()
    {
        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
        levelGameManager = FindObjectOfType<LevelGameManager>();

        shellTranforms[0] = topLeftShell;
        shellTranforms[1] = topRightShell;
        shellTranforms[2] = bottomRightShell;
        shellTranforms[3] = bottomLeftShell;

        if (levelNumber == 15)
        {
            arrayOfBallTypes = new BallTypes[] { BallTypes.Red, BallTypes.Green, BallTypes.Blue, BallTypes.Yellow, BallTypes.Ice, BallTypes.Thunder, BallTypes.Speed, BallTypes.Iron };
        }

        SetInitialSpeed();
        StartCoroutine(CycleBalls());
    }
    IEnumerator CycleBalls()
    {
        //yield return new WaitWhile(() => !LevelReadyCheck.isLevelReady);
        if (levelNumber == 12)
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
            yield return new WaitForSeconds(waitTime);
        }
    }
    public void InstantiateBall(BallTypes typeOfBall)
    //instantiates ball according to type passed in parameter
    {
        if (levelGameManager.gameOver || levelGameManager.gettingScores) return;

        switch (typeOfBall)
        {
            case BallTypes.Thunder:
                ballToInst = thunderBall;
                break;
            case BallTypes.Ice:
                ballToInst = iceBall;
                break;
            case BallTypes.Speed:
                ballToInst = speedBall;
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
            case BallTypes.Iron:
                ballToInst = ironBall;
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
                shellSelect = shellTranforms[0];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "ThunderBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z - randomNumForThrow);
                break;
            case BallTypes.Speed:
                shellSelect = shellTranforms[1];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "SpeedBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x - randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
            case BallTypes.Red:
                shellSelect = shellTranforms[3];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "RedBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
            case BallTypes.Yellow:
                shellSelect = shellTranforms[3];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "YellowBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z - randomNumForThrow);
                break;
            case BallTypes.Blue:
                shellSelect = shellTranforms[3];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "BlueBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
            case BallTypes.Green:
                shellSelect = shellTranforms[3];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "GreenBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x - randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;

            default:
                InstantiateIronBall();
                return;
                shellSelect = shellTranforms[UnityEngine.Random.Range(0,shellTranforms.Length)];
                clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
                clone.name = "IronBall" + name++;
                throwVelocityDirection = new Vector3(shellSelect.x - randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
        }
        clone.velocity = transform.TransformDirection(throwVelocityDirection.normalized * -initialBallSpeed);

        Debug.DrawRay(shellSelect, clone.velocity);
    }
    void InstantiateIronBall()
    {
        Rigidbody clone;
        int randomNum;
        randomNum = UnityEngine.Random.Range(0, shellTranforms.Length);
        float randomNumForThrow = UnityEngine.Random.Range(-2, 3);
        Vector3 shellSelect = shellTranforms[randomNum];
        clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
        clone.name = "IronBall" + name++;
        Vector3 throwVelocityDirection = new Vector3(0,0,0);

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
    }
}
