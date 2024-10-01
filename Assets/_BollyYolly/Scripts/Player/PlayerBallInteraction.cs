    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBallInteraction : MonoBehaviour
{
    GlobalSceneManager globalSceneManager;

    [SerializeField] float collisionFactor = -0.05f; // how much ship's angle (which is proportional to speed of car) will effect ball velocity after collision
    [SerializeField] float collisionForceValue = 1f;  // how much the ball velocity will change after the collision
    [SerializeField] float explosionForce = 1f; 
    [SerializeField] float magnetForce = 1f;
    [SerializeField] float explosionRadius = 1f;
    [SerializeField] float magnetRadius = 1f;
    [SerializeField] float timeBeforeBallLeavesShip = 0.01f; // time after ball throw so that 

    //[SerializeField] CapsuleCollider magnetCollider;
    float randomPos;
    Vector3 ballForceDirection;
    CameraShake cameraShake;
    [SerializeField] bool isMagnetOn = false; //   
    //[SerializeField] bool isFireBallOn = false;
    [SerializeField] bool isBallAttached = false;
    [SerializeField] bool throwingBall = false;
    [SerializeField] int ballCount = 0;
    float kickTimerSerialised = 3f; // time for kicking after first ball is attached
    float kickTimer;
    int countForRandomPosition = 0;
    CPUAi cpuAi;
    public bool randomMotionOnBool = false;

    [SerializeField] GameObject PlayerBurnLevel10;
    [SerializeField] GameObject magnetTimerCanvas;

    public int timeLeftToReleaseBall;
    bool handlingTimerTime;
    int levelNumber;
    private void Start()
    {

        kickTimer = kickTimerSerialised;
        timeLeftToReleaseBall = 3;
        cameraShake = FindObjectOfType<CameraShake>();
        cpuAi = GetComponent<CPUAi>();
        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
        //print(PlayerPrefs.GetString("SelectedLevel"));
        //if(PlayerPrefs.GetString("SelectedLevel") == "Level 1" || PlayerPrefs.GetString("SelectedLevel") == "Level 2" || PlayerPrefs.GetString("SelectedLevel") == "Level 3" || PlayerPrefs.GetString("SelectedLevel") == "Level 6" || PlayerPrefs.GetString("SelectedLevel") == "Level 7" || PlayerPrefs.GetString("SelectedLevel") == "Level 8" || PlayerPrefs.GetString("SelectedLevel") == "Level 9" || PlayerPrefs.GetString("SelectedLevel") == "Level 10")
        levelNumber = PlayerInstance.Instance.CurrentLevelNumber;

        if (levelNumber == 6 || levelNumber == 8)
        {
            magnetRadius = magnetForce = explosionForce = explosionRadius = 3f;
        }
        else
        {
            magnetRadius = magnetForce = explosionForce = explosionRadius = 0f;
        }
    }

    private void Update()
    {
        if (!throwingBall && ballCount <= 3)
        {
            if (levelNumber == 8 || levelNumber == 6)
            {
                MagnetOn(); // on magnet power
            }
        }
        
        if (isMagnetOn)
        {
            ballCount++; 
            kickTimer -= Time.deltaTime;
            if (kickTimer < 1) //after one second of grabbing the ball
            {
                RandomPositionToGoAfterGrabbingTheBall();
            }
        }
        if (kickTimer <= 0f)
        {
            AddExplosion();
            randomMotionOnBool = false;
            countForRandomPosition = 0;
        }
        if(throwingBall)
        {
            //after throwing the ball resetting the timer
            kickTimer += Time.deltaTime;
            if (kickTimer >= kickTimerSerialised)
            {
                throwingBall = false;
            }
            if (levelNumber == 8 || levelNumber == 6)
            {
                HandelTimerCanvas(true);
                StopCoroutine(TimerMagnet());
                handlingTimerTime = false;
                timeLeftToReleaseBall = 3;
            }
        }
    }
    IEnumerator TimerMagnet()
    {
        handlingTimerTime = true;
        while (isBallAttached)
        {
            yield return new WaitForSeconds(1f);
            timeLeftToReleaseBall--;
        }
    }

    void HandelTimerCanvas(bool destroy)
    {
        if (levelNumber != 5 && levelNumber != 4) return;

        if (transform.GetComponentInChildren<TimerMagnet>() != null)
        {
            Destroy(transform.GetComponentInChildren<TimerMagnet>().gameObject);
        }
        GameObject go;
        if (gameObject.CompareTag("Player1"))
        {
            go = Instantiate(magnetTimerCanvas, new Vector3(transform.position.x + 0.75f, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
        }
        else
        {
            go = Instantiate(magnetTimerCanvas, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
        }
        go.transform.SetParent(transform);
        if (destroy)
        {
            timeLeftToReleaseBall = 3;
            StopCoroutine(TimerMagnet());
            Destroy(go);
            return;
        }
        StartCoroutine(TimerMagnet());
    }



    private void OnCollisionEnter(Collision collision)
    {
        //print(collision.transform.position);
        if(isMagnetOn) 
        {
            collision.gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative; //requirements for kinematic bodies
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true; // making ball kinematic 
            while(collision.transform.parent == null) //making the ship the parent of ball
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f); // making z angle of ship zero for proper attachment
                collision.transform.parent = transform; //setting parent
            }

            collision.transform.position = new Vector3(collision.transform.position.x, transform.position.y + 0.2f, collision.transform.position.z);

            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            
            
            
            //isMagnetOn = false;
            isBallAttached = true;
            if (!handlingTimerTime)
            {
                HandelTimerCanvas(false);
            }
        }
        else
        {
            // how much ship will effect ball velocity and its direction after the collision
            if (transform.localEulerAngles.z > 180f)
            {
                ballForceDirection = collision.gameObject.GetComponent<Rigidbody>().velocity + new Vector3((transform.localEulerAngles.z - 360f) * collisionFactor, 0f, 0f);
                collision.gameObject.GetComponent<Rigidbody>().velocity =( ballForceDirection.normalized * collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude) * collisionForceValue;
            }
            else
            {
                ballForceDirection = collision.gameObject.GetComponent<Rigidbody>().velocity + new Vector3(transform.localEulerAngles.z * collisionFactor, 0f, 0f);
                collision.gameObject.GetComponent<Rigidbody>().velocity = (ballForceDirection.normalized * collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude) * collisionForceValue;
            }
            if(!isBallAttached)
            {
                collision.gameObject.GetComponent<AudioSource>().Play();
            }
            Debug.DrawRay(collision.gameObject.transform.position, collision.gameObject.GetComponent<Rigidbody>().velocity);
        }
    }
    public void AddExplosion()
    {
        throwingBall = true;
        isMagnetOn = false;
        BallCollisionDetector[] ballsAttached = GetComponentsInChildren<BallCollisionDetector>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Ball") /*&& hit.transform.parent != null*/ )
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                rb.transform.parent = null;
                rb.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                rb.gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                //if (isBallAttached)
                {
                    SphereCollider ballCollider = rb.GetComponent<SphereCollider>();
                    ballCollider.material.bounceCombine = PhysicMaterialCombine.Maximum;
                    rb.AddExplosionForce(2 * explosionForce, transform.position, 0, 0, ForceMode.Impulse);
                    //cameraShake.ShakeCamera(); //TODO

                }
                //else
                //{
                //    rb.AddExplosionForce(explosionForce, transform.position, 0, 0);
                //}
            }
            ballCount = 0;
        }
        StartCoroutine(TimeBeforeBallDetachesFromShip());
    }

    IEnumerator TimeBeforeBallDetachesFromShip()
    {
        yield return new WaitForSeconds(timeBeforeBallLeavesShip);
        isBallAttached = false;
    }

    public void MagnetOn()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, magnetRadius);
        foreach (Collider hit in colliders)
        {
            //Rigidbody rb = hit.GetComponent<Rigidbody>();
            //if(rb == null)
            //{
            //    Debug.Log(rb + " " + hit.name +"name");
            //}
            //if (rb != null && rb.gameObject.transform.parent == null)
            if (hit.gameObject.CompareTag("Ball"))
            {
                isMagnetOn = true;
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                SphereCollider ballCollider = rb.GetComponent<SphereCollider>();
                ballCollider.material.bounceCombine = PhysicMaterialCombine.Minimum;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
                rb.AddExplosionForce(-magnetForce, transform.position, 0, 0, ForceMode.Impulse);
                rb.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }


    private void RandomPositionToGoAfterGrabbingTheBall()
    {
        randomMotionOnBool = true;
        if (gameObject.GetComponent<CPUAi>() != null)
        {

            if (countForRandomPosition == 0)
            {
                if (cpuAi.cpuState == PlayerStates.Idle)
                {
                    randomPos = Random.Range(-3, 3);
                    while (Mathf.Abs(randomPos - transform.localPosition.x) < 1f)
                    {
                        randomPos = Random.Range(-3, 3);
                    }

                    countForRandomPosition = 1;
                }

            }
            if (cpuAi.cpuPos == PlayerPos.Top)
            {
                Vector3 directionVector = new Vector3(randomPos - transform.position.x, 0f, 0f) * cpuAi.speed;
                transform.Translate(directionVector * cpuAi.speed, Space.World);
                if (Vector3.Dot(new Vector3(randomPos - transform.localPosition.x, 0f, 0f), transform.right) >= 0f)
                {
                    //print(tiltTime);
                    transform.localEulerAngles += new Vector3(0f, 0f, -directionVector.magnitude * cpuAi.maxTilt);
                    //transform.localEulerAngles = new Vector3(0f, 180f, -90f);
                }
                else
                {
                    // print("against axis");
                    transform.localEulerAngles += new Vector3(0f, 0f, directionVector.magnitude * cpuAi.maxTilt);
                }
            }
            else if (cpuAi.cpuPos == PlayerPos.Right)
            {
                //print(randomPos - transform.position.z);
                Vector3 directionVector = new Vector3(0f, 0f, randomPos - transform.position.z) * cpuAi.speed;
                transform.Translate(directionVector * cpuAi.speed, Space.World);
                if (Vector3.Dot(new Vector3(0f, 0f, randomPos - transform.position.z), Vector3.back) >= 0f)
                {
                    transform.localEulerAngles += new Vector3(0f, 0f, directionVector.magnitude * cpuAi.maxTilt);
                }
                else
                {
                    //print("against axis");
                    transform.localEulerAngles += new Vector3(0f, 0f, -directionVector.magnitude * cpuAi.maxTilt);
                    //transform.localEulerAngles = new Vector3(0f, 90f, -90f);
                }
            }
            else
            {
                //print(randomPos - transform.position.z);
                Vector3 directionVector = new Vector3(0f, 0f, randomPos - transform.position.z) * cpuAi.speed;
                transform.Translate(directionVector * cpuAi.speed, Space.World);
                if (Vector3.Dot(new Vector3(0f, 0f, randomPos - transform.position.z), Vector3.forward) >= 0f)
                {
                    //print("along axis");
                    transform.localEulerAngles += new Vector3(0f, 0f, directionVector.magnitude * cpuAi.maxTilt);
                    //transform.localEulerAngles = new Vector3(0f, 90f, 90f);
                }
                else
                {
                    //print("against axis");
                    transform.localEulerAngles += new Vector3(0f, 0f, -directionVector.magnitude * cpuAi.maxTilt);
                    //transform.localEulerAngles = new Vector3(0f, 90f, -90f);
                }
            }
        }
    }
}
