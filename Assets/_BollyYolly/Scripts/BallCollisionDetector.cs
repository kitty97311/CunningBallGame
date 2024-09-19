using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollisionDetector : MonoBehaviour
{
    BallInstantiator ballInstantiator;
    GlobalSceneManager globalSceneManager;
    float destroyTime = 2f;
    float minBallVelocity = 5f;
    LevelGameManager levelGameManager; 
    float yPosBallAfterLanding = -10f;


    public Dictionary<GameObject, Boolean> playerflag;

    int levelNumber;
    bool touchedOnce;
    void Start()
    {
        ballInstantiator = FindObjectOfType<BallInstantiator>();
        levelGameManager = FindObjectOfType<LevelGameManager>();
        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
        destroyTime = levelGameManager.level.spawnFrequency;
        levelNumber = globalSceneManager.GetCurrentLevelNumber();
        LevelSixChecker();
        LevelTenChecker();
    }
    void LevelTenChecker()
    {
        if (levelNumber == 10)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            minBallVelocity = 8f;
            GetComponent<SphereCollider>().material.bounciness = 1;
        }
        if (levelNumber == 6 || levelNumber == 8)
        {
            minBallVelocity = 6f;
        }
    }

    void LevelSixChecker()
    {
        if (GameObject.FindObjectOfType<PlatformTiltHandler>() == null)
        {
            return;
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            minBallVelocity = 8f;
            SetLevelNumber();
            GetComponent<SphereCollider>().material.bounciness = 1;
        }
    }

    void SetLevelNumber()
    //sets level number int(digit)
    {
        levelNumber = globalSceneManager.GetCurrentLevelNumber();
    }
    void Update()
    {
        if (yPosBallAfterLanding > 0f && transform.parent == null)
        {
            transform.position = new Vector3(transform.position.x, yPosBallAfterLanding, transform.position.z);
        }
        if(GetComponent<Rigidbody>().velocity.magnitude <= minBallVelocity && transform.parent == null  && yPosBallAfterLanding > 0f)
        {
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * minBallVelocity;
        }
        if(GetComponent<Rigidbody>().velocity.magnitude >= levelGameManager.maxSpeedBall)
        {
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * levelGameManager.maxSpeedBall;
        }
        // velocity = omega * r (in perfect rotational motion) omega = angular velocity
        Vector3 angularVelocityDirection = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.z, gameObject.GetComponent<Rigidbody>().velocity.y, -gameObject.GetComponent<Rigidbody>().velocity.x).normalized;
        
        gameObject.GetComponent<Rigidbody>().angularVelocity = (gameObject.GetComponent<Rigidbody>().velocity.magnitude / gameObject.GetComponent<SphereCollider>().radius) * angularVelocityDirection ;

        GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * minBallVelocity * 1.5f;

        if(!touchedOnce && levelNumber == 10)
        {
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * minBallVelocity * 3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ScoreManager>() != null)
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(DestroyAndInstantiateNewBall());
        }        
    }
    //public CPUManager ai;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") && (levelNumber != 6))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            //yPosBallAfterLanding = transform.position.y;
            GetComponent<SphereCollider>().material.bounciness = 1;
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * 20;
        }
        if (collision.gameObject.CompareTag("Floor") && (levelNumber == 10))
        {
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity / 20;
            touchedOnce = true;
        }
        if (levelNumber == 10)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            //touchedOnce = true;
            //GetComponent<SphereCollider>().material.bounciness = 0;
            //GetComponent<SphereCollider>().material.dynamicFriction = 1;
            Physics.gravity = new Vector3(0, -30, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        //ai.BallList();
    }

    IEnumerator DestroyAndInstantiateNewBall()
    {
        yield return new WaitForSeconds(destroyTime);
        levelGameManager = FindObjectOfType<LevelGameManager>();
        levelGameManager.ReleaseBall();
        UnityEngine.Object.Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, gameObject.GetComponent<Rigidbody>().velocity);
    }
}
