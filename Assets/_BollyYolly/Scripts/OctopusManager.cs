using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusManager : MonoBehaviour
{

    [SerializeField] float octopusSpawnTime = 5f;
    [SerializeField] float yPosOctopus = 1f;
    [SerializeField] float octoRotValue = 5f;
    [SerializeField] float ballThrowIntervalWhileRotation = 5f;
    [SerializeField] int totalBallToBeThrown = 9;
    [SerializeField] float ballThrowSpeed = 1f;
    [SerializeField] float octopusNeckPos = 0.3f;
    [SerializeField] float octopusIdleMoveAmplitude = 500f;
    [SerializeField] float xRange = 2f;
    [SerializeField] float zRange = 2f;
    [SerializeField] GameObject octopusPrefab;
    [SerializeField] GameObject ball;
    GameObject octopus;
    float ballThrowTime = 0f;
    float randX, randZ, newRandX, newRandZ;
    int randAngle;
    bool isBallThrown = false;
    bool octopusSpawned = false;
    float elapsedTime,  timeToDie;
    float timeToRotate = 3f;
    float timePeriod;
    float fixedSpawnTime;
    float percentageComplete = 0f;
    //[SerializeField] Transform octopusMouth;
    private void Start()
    {
        fixedSpawnTime = octopusSpawnTime;
        octopus = Instantiate(octopusPrefab, new Vector3(0f, yPosOctopus, 0f), Quaternion.identity);
        octopus.SetActive(false);
    }

    private void FixedUpdate()
    {
        // always angry face for octopus
        if(octopus != null)
        {
            octopus.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(5, 100);
        }
        // octopus spawning timer
        if (octopusSpawnTime > 0f)
        {
            octopusSpawnTime -= Time.deltaTime;
        }
        //after spawn behaviour
        if(octopusSpawnTime < 0f)
        {
            SpawnOctopus();

            RotateAndThrowBall();

            OctopusIdleMotion();

            DeactivateOctopus();
        }
    }

    private void DeactivateOctopus()
    {
        if (percentageComplete > 1f)
        {
            timeToDie += Time.deltaTime;
            if (timeToDie > 2f)
            {
                octopus.SetActive(false);
                octopusSpawnTime = fixedSpawnTime;
                octopusSpawned = false;
                percentageComplete = 0f;
                elapsedTime = 0f;
                timeToRotate = 3f;
                timeToDie = 0f;
                isBallThrown = false;
                ballThrowTime = 0f;
            }
        }
    }

    private void OctopusIdleMotion()
    {
        timePeriod += Time.deltaTime;
        float octoYPos = Mathf.Sin(timePeriod * Mathf.PI * 2f);
        if (percentageComplete == 0f || percentageComplete > 1f)
        {
            octopus.transform.position += new Vector3(0f, octoYPos / octopusIdleMoveAmplitude, 0f);
        }
    }

    private void RotateAndThrowBall()
    {
        timeToRotate -= Time.deltaTime;
        if (timeToRotate < 0f && percentageComplete < 1f)
        {
            octopus.GetComponent<Animator>().SetTrigger("TentaclesUp");
            if (percentageComplete < 1f)
            {
                octopus.transform.Rotate(0f, -octoRotValue, 0f);
                elapsedTime += Time.deltaTime;
                ballThrowTime += Time.deltaTime;
                isBallThrown = false;
                //print(ballThrowTime + " " + percentageComplete);
                percentageComplete = elapsedTime / (ballThrowIntervalWhileRotation * (totalBallToBeThrown +1));
                octopus.transform.position = new Vector3(Mathf.Lerp(randX, newRandX, percentageComplete),
                                                         yPosOctopus,
                                                         Mathf.Lerp(randZ, newRandZ, percentageComplete));

                //spawn a ball through his mouth during travel
                if (ballThrowTime > ballThrowIntervalWhileRotation)
                {
                    //print("hmlo");
                    Rigidbody clone;
                    clone = Instantiate(ball.GetComponent<Rigidbody>(), octopus.transform.position + new Vector3(0f, octopusNeckPos, 0f), Quaternion.identity);
                    octopus.GetComponent<Animator>().SetTrigger("ThrowBall");
                    clone.AddForce(new Vector3(octopus.transform.position.x, 0f, octopus.transform.position.z).normalized * ballThrowSpeed, ForceMode.Impulse);
                    //isBallThrown = true;
                    ballThrowTime = 0f;
                }

                //print(percentageComplete);
                if (percentageComplete > 0.8f)
                {
                    
                    octopus.GetComponent<Animator>().SetTrigger("TentaclesDown");
                }

            }
        }
        
    }

    private void SpawnOctopus()
    {
        if (!octopusSpawned)
        {
            randX = Random.Range(-xRange, xRange);
            randZ = Random.Range(-zRange, zRange);
            randAngle = Random.Range(-180, 180);
            if (octopus == null)
            {
                octopus = Instantiate(octopusPrefab, new Vector3(randX, yPosOctopus, randZ), Quaternion.identity);
            }
            octopus.transform.position = new Vector3(randX, yPosOctopus, randZ);
            octopus.transform.eulerAngles = new Vector3(0f, randAngle, 0f);
            octopus.SetActive(true);
            newRandX = Random.Range(-xRange, xRange);
            newRandZ = Random.Range(-zRange, zRange);
            while(Mathf.Abs(newRandX - randX) < 0.5f || Mathf.Abs(newRandZ - randZ) < 0.5f)
            {
                newRandX = Random.Range(-xRange, xRange);
                newRandZ = Random.Range(-zRange, zRange);
            }
        }
        octopusSpawned = true;
    }
}
