using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class BallInstantiator : MonoBehaviour
{
    public GameObject ironBall;
    public GameObject iceBall;
    public GameObject thunderBall;
    public GameObject speedBall;
    public GameObject fireBall;

    GlobalSceneManager globalSceneManager;

    private float initialBallSpeed = 3f;

    [SerializeField] Vector3 topRightShell = new Vector3(4.3f, 1.7f, 4.3f);
    [SerializeField] Vector3 topLeftShell = new Vector3(-4.2f, 1.7f, 4.3f);
    [SerializeField] Vector3 bottomRightShell = new Vector3(4.3f, 1.7f, -4.3f);
    [SerializeField] Vector3 bottomLeftShell = new Vector3(-4.2f, 1.7f, -4.2f);

    //[SerializeField] private int shell = 0;

    private Vector3[] shellTranforms = new Vector3[4];
    public bool isLevelTen;

    public event EventHandler<onBallSpawnEventArgs> BallData;

    int levelNumber;
    GameObject ballToInst;
    LevelGameManager levelGameManager;

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

        SetInitialSpeed();
    }

    public static int name = 0;
    public static int FireBallname = 0;

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
        }

        Rigidbody clone;
        int randomNum = UnityEngine.Random.Range(0, 4);
        float randomNumForThrow = UnityEngine.Random.Range(-2, 3);
        Vector3 shellSelect = shellTranforms[randomNum];
        clone = Instantiate(ballToInst.GetComponent<Rigidbody>(), shellSelect, Quaternion.identity);
        clone.name = "Ball" + name++;
        // Give the cloned object an initial velocity along the current
        // object's Z axis
        Vector3 throwVelocityDirection;
        switch (randomNum)
        {
            case 0:
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
            case 1:
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z - randomNumForThrow);
                break;
            case 2:
                throwVelocityDirection = new Vector3(shellSelect.x + randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
            case 3:
                throwVelocityDirection = new Vector3(shellSelect.x - randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
            default:
                throwVelocityDirection = new Vector3(shellSelect.x - randomNumForThrow, shellSelect.y, shellSelect.z + randomNumForThrow);
                break;
        }
        if (levelNumber == 10)
        {
            clone.velocity = transform.TransformDirection(throwVelocityDirection.normalized * -initialBallSpeed / 1.5f);
        }
        else
        {
            clone.velocity = transform.TransformDirection(throwVelocityDirection.normalized * -initialBallSpeed * 1.5f);
        }
        Debug.DrawRay(shellSelect, clone.velocity);
    }

}
