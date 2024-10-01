using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum PlayerPos { Top, Bottom, Left, Right,}
public class CPUAi : MonoBehaviour
{
    PlayerAudio playerAudio;
    int counterToGetZAngleAfterMovementIsDone = 0;
    public  float lowerRange = 1f;
    public float upperRange = 1f;
    [SerializeField] float minCosValue = 0f;
    public float speed = 5f;
    public float maxTilt = 10f;

    [SerializeField] float shipFixedAxisValue = 1f;
    [SerializeField] private float yPeriod = 0.5f;
    [SerializeField] private float zPeriod = 0.5f;
    [SerializeField] private float idleYMoveMagnitude = 0.5f;
    [SerializeField] private float idleZRotMagnitude = 0.5f;

    Vector3 dirToGo;
    float zAngleAtSwipeStop;
    float percentageComplete, elapsedTime;
    float intersectionValue;
    internal PlayerPos cpuPos;
    internal PlayerStates cpuState;
    float decZRot = 0f;

    float yCycles, zCycles, initYValue, initZValue;

    GlobalSceneManager globalSceneManager;
    int playerNumber;
    bool isLevelSix;
    int levelNumber;
    PlatformTiltHandler platformTiltHandler;
    public LayerMask lm;

    public bool isFrozen = false;
    public bool isBurning = false;
    public bool isShocked = false;
    public bool isSpeeding = false;
    public bool isVisible = true;

    float currentFreezingTimer;

    Material material1Car;
    Material material2Car;
    [SerializeField] Material iceMaterial;

    Material upperMaterial;
    Material lowerMaterial;

    Material[] oldMaterialArrayCharacter;
    Material[] oldMaterialArrayCar;

    IEnumerator freezeCoroutine;


    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject PlayerBurnLevel10;

    [SerializeField] float timePeriodSpedUp = 5f;
    [SerializeField] float speedMultiple = 3.0f;


    AudioSource audioSourcePlayer;
    private void Start()
    {
        audioSourcePlayer = GetComponent<AudioSource>();
        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
        playerAudio = GetComponent<PlayerAudio>();
        SetPlayerName();
        initYValue = transform.position.y;
        initZValue = transform.localEulerAngles.z;
        cpuState = PlayerStates.Idle;
        CheckShipPosition();

        StartCoroutine(GetDefaultMaterialForCar());
    }
    
    void SetPlayerName()
    {
        switch (gameObject.tag)
        {
            case "Player2":
                playerNumber = 2;
                break;
            case "Player3":
                playerNumber = 3;
                break;
            case "Player4":
                playerNumber = 4;
                break;
        }
    }

    public void HandleSfx()
    {
        if (PlayerInstance.Instance.Setting.sfx)
        {
            audioSourcePlayer.volume = 1;
        }
        else
        {
            audioSourcePlayer.volume = 0;
        }
    }

    private void CheckShipPosition()
    {
        //checking cpu ship position
        if (transform.eulerAngles.y == 180f)
        {
            cpuPos = PlayerPos.Top;
        }
        else if (transform.eulerAngles.y == 90f)
        {
            cpuPos = PlayerPos.Left;
        }
        else if (transform.eulerAngles.y == 270f)
        {
            cpuPos = PlayerPos.Right;
        }
        else
        {
            cpuPos = PlayerPos.Bottom;
        }
    }

    private void Update()
    {
        HandleSfx();
        //clamping z angle of ship
        float clampedZAngle = Mathf.Clamp((transform.localEulerAngles.z > 180f ? transform.localEulerAngles.z - 360f : transform.localEulerAngles.z),
                                           -maxTilt, maxTilt);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, clampedZAngle);



        if (cpuState == PlayerStates.Idle)
        {
            ComingBackToIdleMotion();
        }
        else
        {
            elapsedTime = 0f;
            counterToGetZAngleAfterMovementIsDone = 0;
            yCycles = zCycles = 0f;
        }
        if (decZRot == 0f && levelNumber != 10)// decZRot is in comingbacktoidlemotion  method , Once the ship comes to idle motion from moving 
        {
            ShipIdleMotion();
        }

        //if (platformTiltHandler == null) return;

        if (levelNumber != 10) return;

        Vector3 newPos = new Vector3(transform.localPosition.x, GetYPos() + 0.6f, transform.localPosition.z);
        transform.localPosition = newPos;
    }
    float GetYPos()
    //gets y coord for car's position, shoots a ray
    //downwards and gets the coordinates it hits the
    //base/platform and returns its y coord for the
    //this car/player to use as its ypos
    {
        RaycastHit hit;
        Vector3 newpos = new Vector3(0,0,0);

        switch (playerNumber)
        {
            case 2:
                newpos = new Vector3(transform.position.x - 1, transform.position.y + 5, transform.position.z);
                break;
            case 3:
                newpos = new Vector3(transform.position.x + 1, transform.position.y + 5, transform.position.z);
                break;
            case 4:
                newpos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z + 1);
                break;
        }
        //z+- and x+- so as to shoot ray from the rear of the car
        //to get perfect postion according to the tilt

        Debug.DrawRay(newpos, -gameObject.transform.up, Color.blue);

        if (Physics.Raycast(newpos, -gameObject.transform.up, out hit, Mathf.Infinity, lm))
        {
            if (hit.transform.name == "Base")
            {
                return (hit.point).y;
            }
        }
        return 0f;
    }

    //there should be a box collider in trigger
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<BallCollisionDetector>() != null)
        {
            //BallTypes colourOfBall = other.GetComponent<Ball>().ball.ballType;

            //if (GetComponent<PlayerColourData>() != null)
            //{
            //    if (GetComponent<PlayerColourData>().colourOfThePlayer != colourOfBall) return;
            //}

            //get the velocity of car and dot product with the forward vector and check whether it is
            float dotProduct = Vector3.Dot(other.GetComponent<Rigidbody>().velocity.normalized, transform.forward);
            Vector3 bVel = other.GetComponent<Rigidbody>().velocity.normalized;

            if (dotProduct < minCosValue) //Checking whether ball is coming towards the player
            {
                cpuState = PlayerStates.Moving;

                ShipMovement(other, bVel);

            }
            else
            {
                cpuState = PlayerStates.Idle;
            }
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        cpuState = PlayerStates.Idle;
    }

    private void ShipIdleMotion() //same as player mechanics
    {
        if (isFrozen) return;
        if (isBurning) return;
        if (isShocked) return;

        yCycles = (yCycles + Time.deltaTime);
        zCycles = (zCycles + Time.deltaTime);

        float y1 = yCycles / yPeriod;
        float z1 = zCycles / zPeriod;

        float sinMovementY = Mathf.Sin(y1 * Mathf.PI * 2);
        float sinMovementZ = Mathf.Sin(z1 * Mathf.PI * 6);

        float yValue = initYValue + idleYMoveMagnitude * sinMovementY;
        float zRot = initZValue + idleZRotMagnitude * sinMovementZ;

        transform.position = new Vector3(transform.position.x, yValue, transform.position.z);
        transform.localEulerAngles += new Vector3(0f, 0f, zRot);
    }


    public void FreezeOnIceBallTaken()
    //call freezing coroutine
    {
        freezeCoroutine = ProcessFreezing();

        if (isFrozen)
        {
            StopCoroutine(freezeCoroutine);
            MeshRenderer mr = transform.GetChild(2).GetChild(1).GetComponent<MeshRenderer>();
            Renderer[] renderer = GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < renderer.Length; i++)
            {
                if (renderer[i].name == "Haver") continue;
                renderer[i].material = new Material(oldMaterialArrayCharacter[i]);
            }
            isFrozen = false;
        }
        StartCoroutine(freezeCoroutine);
    }
    IEnumerator GetDefaultMaterialForCar()
    {
        yield return new WaitForSeconds(5f);

        Renderer[] renderer = GetComponentsInChildren<Renderer>(true);
        oldMaterialArrayCharacter = new Material[renderer.Length];
        for (int i = 0; i < renderer.Length; i++)
        {
            if (renderer[i].name == "Haver") continue;
            oldMaterialArrayCharacter[i] = renderer[i].material;
        }
        MeshRenderer mr = transform.GetChild(2).GetChild(1).GetComponent<MeshRenderer>();

        oldMaterialArrayCar = new Material[] { mr.materials[0], mr.materials[1] };
    }
    IEnumerator ProcessFreezing()
    //gets the renderer component of all the
    //children and sets materials to ice and
    //then changes them to original after a
    //set period of time
    {
        isFrozen = true;

        GetComponent<AudioSource>().clip = playerAudio.freeze;
        GetComponent<AudioSource>().Play(0);
        MeshRenderer mr =  transform.GetChild(2).GetChild(1).GetComponent<MeshRenderer>();

        //Material[] oldMaterials = new Material[] { mr.materials[0], mr.materials[1] };
        Material[] newMaterials = new Material[] { iceMaterial, iceMaterial };

        mr.materials = newMaterials;
        Renderer[] renderer = GetComponentsInChildren<Renderer>(true);

        oldMaterialArrayCharacter = new Material[renderer.Length];
        for (int i = 0; i < renderer.Length; i++)
        {
            if (renderer[i].name == "Haver") continue;
            oldMaterialArrayCharacter[i] = renderer[i].material;
        }

        //saves all old material in array
        //except for hover as it has 2 materials

        foreach (Renderer rdr in renderer)
        {
            if (rdr.name == "Haver") continue;
            rdr.material = new Material(iceMaterial);
        }


        yield return new WaitForSeconds(6f);

        //Material[] oldMaterials = new Material[] { upperMaterial, lowerMaterial };
        mr.materials = oldMaterialArrayCar;
        for (int i = 0; i < renderer.Length; i++)
        {
            if (renderer[i].name == "Haver") continue;
            renderer[i].material = new Material(oldMaterialArrayCharacter[i]);
        }
        //sets old materials back
        isFrozen = false;
    }
    public void ProcessThunderKilling()
    //calls thunder kiing coroutine
    {
        if (!isVisible) return;
        StartCoroutine(ThunderKill());
    }
    IEnumerator ThunderKill()
    //process killing after a set period of time
    {
        isShocked = true;
        GetComponentInChildren<Animator>().SetBool("isKilled", true);
        yield return new WaitForSeconds(3.0f); 

        GetComponent<AudioSource>().clip = playerAudio.explosion;
        GetComponent<AudioSource>().Play(0);

        GameObject go = Instantiate(explosionEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.3f);
        Destroy(go, 0.5f);
        SetPlayerVisibility(false);
        GetComponentInChildren<Animator>().SetBool("isKilled", false);
        yield return new WaitForSeconds(5f);
        SetPlayerVisibility(true);
        isShocked = false;
    }
    public void ProcessSpeedUp()
    //calls speedup coroutine
    {
        if (isSpeeding)
        {
            StopCoroutine(SpeedUp());
            isSpeeding = false;
        }
        StartCoroutine(SpeedUp());
    }
    IEnumerator SpeedUp()
    //multiplies current player speed to preset value
    //and then sets it back to original
    {
        if (isSpeeding)
        {
            speed = -speed;
            lowerRange = -lowerRange;
            upperRange = -upperRange;
        }
        isSpeeding = true;
        speed = -speed;
        lowerRange = -lowerRange;
        upperRange = -upperRange;
        yield return new WaitForSeconds(timePeriodSpedUp);
        lowerRange = -lowerRange;
        upperRange = -upperRange;
        speed = -speed;
        isSpeeding = false;
    }
    public void ProcessPlayerFireUp()
    {
        if (!isVisible) return;
        StartCoroutine(FireUp());
    }
    IEnumerator FireUp()
    {
        isBurning = true;

        GetComponent<AudioSource>().clip = playerAudio.fire;
        GetComponent<AudioSource>().Play(0);

        GameObject go = Instantiate(PlayerBurnLevel10, transform);
        yield return new WaitForSeconds(3f);
        Destroy(go, 0.5f);
        SetPlayerVisibility(false);
        GetComponentInChildren<Animator>().SetBool("isKilled", false);
        yield return new WaitForSeconds(5f);
        SetPlayerVisibility(true);
        isBurning = false;
    }
    void SetPlayerVisibility(bool isVisible)
    //sets visibility of the player
    {
        this.isVisible = isVisible;
        MeshRenderer[] allRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer item in allRenderers)
        {
            item.enabled = isVisible;
        }
        SkinnedMeshRenderer[] allSkinnedRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer item in allSkinnedRenderers)
        {
            item.enabled = isVisible;
        }
        Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider item in allColliders)
        {
            item.enabled = isVisible;
        }
    }

    private void ComingBackToIdleMotion()
    {
        // lerping z angle of ship to zero
        if (transform.localEulerAngles.z != 0f)
        {
            if (counterToGetZAngleAfterMovementIsDone == 0)
            {
                zAngleAtSwipeStop = transform.localEulerAngles.z > 180f ? (transform.localEulerAngles.z - 360f) : transform.localEulerAngles.z;
                counterToGetZAngleAfterMovementIsDone = 1;
            }
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / 0.25f;
            decZRot = Mathf.Lerp(zAngleAtSwipeStop, 0f, percentageComplete);

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, decZRot);
        }
    }

    private void ShipMovement(Collider other, Vector3 bVel)
    {

        // basically all we are doing is calculating the value of local x position of ship where the ball will be crossing then translating to that point
        // equation used z = mx + c  // m = slope of velocity, c = line constant 
        if (isFrozen) return;
        if (isShocked) return;
        if (isBurning) return;

        if (cpuPos == PlayerPos.Right)
        {
            float slope = Vector3.Angle(new Vector3(bVel.x, 0f, bVel.z), Vector3.back);
            float m = Mathf.Tan(slope * Mathf.PI / 180f);//slopeValue
            float c = other.transform.position.x + m * other.transform.position.z;
            //z = mx + c
            intersectionValue = -(shipFixedAxisValue - c) / m;
            if (intersectionValue > lowerRange && intersectionValue < upperRange)
            {
                dirToGo = (new Vector3(0f, 0f, intersectionValue - transform.position.z)) * speed * Time.deltaTime * 30;
                transform.Translate(dirToGo, Space.World); // translating to intersection value

                float distanceBtwShipAndTarget = dirToGo.magnitude;
                if (distanceBtwShipAndTarget <= 0.01f)
                {
                    cpuState = PlayerStates.Idle;
                }
                // ship tilt code
                if (Vector3.Dot(new Vector3(0f, 0f, intersectionValue - transform.position.z), Vector3.back) >= 0f)
                {
                    transform.localEulerAngles += new Vector3(0f, 0f, distanceBtwShipAndTarget * maxTilt);
                }
                else
                {
                    //print("against axis");
                    transform.localEulerAngles += new Vector3(0f, 0f, -distanceBtwShipAndTarget * maxTilt);
                    //transform.localEulerAngles = new Vector3(0f, 90f, -90f);
                }
            }

        }

        else if (cpuPos == PlayerPos.Left)
        {
            float slope = Vector3.Angle(new Vector3(bVel.x, 0f, bVel.z), Vector3.forward);
            float m = Mathf.Tan(slope * Mathf.PI / 180f);//slopeValue
            float c = -other.transform.position.x - m * other.transform.position.z;
            intersectionValue = (-shipFixedAxisValue - c) / m;;
            if (intersectionValue > lowerRange && intersectionValue < upperRange)
            {
                dirToGo = (new Vector3(0f, 0f, intersectionValue - transform.position.z)) * speed * Time.deltaTime * 30;
                transform.Translate(dirToGo, Space.World);

                float distanceBtwShipAndTarget = dirToGo.magnitude;
                if (distanceBtwShipAndTarget <= 0.01f)
                {
                    cpuState = PlayerStates.Idle;
                }
                if (Vector3.Dot(new Vector3(0f, 0f, intersectionValue - transform.position.z), Vector3.forward) >= 0f)
                {
                    transform.localEulerAngles += new Vector3(0f, 0f, distanceBtwShipAndTarget * maxTilt);
                }
                else
                {
                    transform.localEulerAngles += new Vector3(0f, 0f, -distanceBtwShipAndTarget * maxTilt);
                }
            }
        }

        else if (cpuPos == PlayerPos.Top)
        {
            float slope = Vector3.Angle(new Vector3(bVel.x, 0f, bVel.z), Vector3.right);
            float m = Mathf.Tan(slope * Mathf.PI / 180f); //slopeValue
            float c = other.transform.position.z - m * other.transform.position.x;
            // x = z-c/m
            intersectionValue = ((shipFixedAxisValue) - c) / m;

            if (intersectionValue > lowerRange && intersectionValue < upperRange)
            {
                dirToGo = new Vector3(intersectionValue - transform.position.x, 0f, 0f) * speed * Time.deltaTime * 30;
                transform.Translate(dirToGo, Space.World);
            }
            float distanceBtwShipAndTarget = dirToGo.magnitude;

            if (distanceBtwShipAndTarget <= 0.01f)
            {
                cpuState = PlayerStates.Idle;
            }
            if (Vector3.Dot(new Vector3(intersectionValue - transform.localPosition.x, 0f, 0f), transform.right) >= 0f)
            {
                transform.localEulerAngles += new Vector3(0f, 0f, -distanceBtwShipAndTarget * maxTilt);
            }
            else
            {
                transform.localEulerAngles += new Vector3(0f, 0f, distanceBtwShipAndTarget * maxTilt);
            }
        }
    }
}
