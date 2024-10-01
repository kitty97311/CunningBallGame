using UnityEngine;
using System;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum PlayerStates
{
    Idle,
    Moving,
}

public class PlayerMechanics : MonoBehaviour
{
    PlatformTiltHandler platformTiltHandler;
    PlayerAudio playerAudio;

    bool touchInAction;

    internal PlayerStates playerState;

    public float playerSpeed = 0f;
    
    [SerializeField] private float swipeThreshold = 0.05f;
    [SerializeField] private float moveTiltTime = 0.5f; // time in which player ship will come to idle state
    [SerializeField] private float yPeriod = 0.5f; // y frequency
    [SerializeField] private float zPeriod = 0.5f; // z frequency
    [SerializeField] private float idleYMoveMagnitude = 0.5f; // max y movement while idle for floating
    [SerializeField] private float idleZRotMagnitude = 0.5f; // max z rotation for ship when idle (for floating)
    [SerializeField] private float maxTilt = 40f; //maximum tilt while moving


    [SerializeField] float playerMovementXLimit;
    [SerializeField] float playerMovementYLimit;
    //public Vector2 initialClickPos, finalClickPos = Vector2.zero; 

    public Vector3 initialClickPosMouse = Vector3.zero; //initial click position, final click position for mouse click 
    public bool hasFinishedClicking = true; // it is true when the player has stopped clicking(to start extra tilt and movement)
    public bool isIdle = true;
    public float clickPressTime, elapsedTime = 0f;
    public float zAngleAtSwipeStop = 0f;   // getting ship's z angle at swipe stop to lerp it to zero
    public float initYValue = 0.5f; // getting y transform of ship for idle motion
    public float initZValue = 0.5f; //getting initial z rot of ship
    public float startYPos; // getting initial y transform of ship to keep the y transform of ship in check while going from idle to moving
    public float yCycles; // y idle frequency
    public float zCycles; // z idle frequency
    public float zAngleValue; //for animation float 
    [SerializeField] bool useMouseControls = false; //not necessary now as we will have only mouse controls


    GlobalSceneManager globalSceneManager;
    public LayerMask lm;
    bool isLevelSix;
    int levelNumber;

    bool isFrozen;
    bool isBurning;
    bool isShocked;
    bool isSpeeding;
    bool isVisible = true;

    Material material1Car;
    Material material2Car;
    [SerializeField] Material iceMaterial;
    [SerializeField] Material upperMaterial;
    [SerializeField] Material lowerMaterial;

    //current level number

    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject PlayerBurnLevel10;

    MeshRenderer[] meshRendererChildren;

    [SerializeField] float timePeriodSpedUp = 5f;
    [SerializeField] float speedMultiple = 3.0f;

    Material[] oldMaterialArrayCharacter;
    Material[] oldMaterialArrayCar;

    float screenHeight;
    float screenWidth;


    AudioSource audioSourcePlayer;
    IEnumerator freezeCoroutine;
    PlayerPos cpuPos;
    private void Start()
    {
        audioSourcePlayer = GetComponent<AudioSource>();
        screenHeight = Display.main.renderingHeight;
        screenWidth = Display.main.renderingWidth;

        globalSceneManager = FindObjectOfType<GlobalSceneManager>();
        playerAudio = GetComponent<PlayerAudio>();

        startYPos = transform.position.y;
        initYValue = transform.position.y;
        initZValue = transform.localEulerAngles.z;

        SetLevelNumber();

        platformTiltHandler = FindObjectOfType<PlatformTiltHandler>();

        StartCoroutine(GetDefaultMaterialForCar());
        CheckShipPosition();

    }
    void SetLevelNumber()
    //sets current level number int(digit)
    {
        levelNumber = PlayerInstance.Instance.CurrentLevelNumber;
    }
    float GetYPos()
    //gets y coord for car's position, shoots a ray
    //downwards and gets the coordinates it hits the
    //base/platform and returns its y coord for the
    //this car/player to use as its ypos
    {
        RaycastHit hit;

        Vector3 newpos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z - 1);
        // z-1 so as to shoot ray form the rear part
        //of the to get accurate position for car
        if (Physics.Raycast(newpos, -gameObject.transform.up, out hit, Mathf.Infinity, lm))
        {
            if (hit.transform.name == "Base")
            {
                return (hit.point).y;
            }
        }
        return 0f;
        //returns 0 if didn't hit any collider 
    }
    void Update()
    {
        HandleSfx();
        zAngleValue = transform.localEulerAngles.z > 180f ? transform.localEulerAngles.z - 360f : transform.localEulerAngles.z;

        if (!isIdle && levelNumber != 10)
        {
            GetComponentInChildren<Animator>().SetFloat("Ship_Angle", -zAngleValue / (maxTilt / 4f)); //animator of the character model in ship
        }
        else
        {
            GetComponentInChildren<Animator>().SetFloat("Ship_Angle", 0f);
        }
       
        if (hasFinishedClicking)
        {
            ExtraTiltAndMovement();
        }
        if (isIdle && levelNumber != 10)
        {
            ShipIdleMovement();
        }
            PlayerMovement();


        if (levelNumber != 10) return;

        Vector3 newPos = new Vector3(transform.localPosition.x, GetYPos() + 0.5f, transform.localPosition.z);
        //sets y value according to platform tilt
        transform.localPosition = newPos;
        //sets y value according to platform tilt
        playerSpeed = 0.00030f;
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
        else if (transform.eulerAngles.y == 0f)
        {
            cpuPos = PlayerPos.Bottom;
        }
        Debug.Log("Ship positon is " + cpuPos);
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
        MeshRenderer mr = transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>();

        oldMaterialArrayCar = new Material[] { mr.materials[0], mr.materials[1] };
    }
    public void FreezeOnIceBallTaken()
    //call freezing coroutine
    {
        freezeCoroutine = ProcessFreezing();

        if (isFrozen)
        {
            StopCoroutine(freezeCoroutine);
            MeshRenderer mr = transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>();
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
    IEnumerator ProcessFreezing()
    //gets the renderer component of all the
    //children and sets nateruals to ice and
    //then changes them to original after a
    //set period of time
    {
        isFrozen = true;

        GetComponent<AudioSource>().clip = playerAudio.freeze;
        GetComponent<AudioSource>().Play(0);

        MeshRenderer mr = transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>();

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
        isFrozen = false;
    }
    public void ProcessThunderKilling()
    //calls thunder killing coroutine
    {
        if (!isVisible) return;
        StartCoroutine(Kill());
    }
    IEnumerator Kill()
    //process killing after a set period of time
    {
        isShocked = true;
        PlayerInstance.Instance.IncreaseDeath();
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
    //////////////changed to confuse ball
    {
        isSpeeding = true;
        playerSpeed = -playerSpeed;
        yield return new WaitForSeconds(timePeriodSpedUp);
        playerSpeed = -playerSpeed;
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
        PlayerInstance.Instance.IncreaseDeath();
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
    public void HandleSfx()
    {
        if (PlayerInstance.Instance.Setting.sfx)
        {
            if(audioSourcePlayer != null)
            {
                audioSourcePlayer.volume = 1;
            }
        }
        else
        {
            if (audioSourcePlayer != null)
            {
                audioSourcePlayer.volume = 0;
            }
        }
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

    public void ShipIdleMovement()
    {
        if (isFrozen) return;
        if (isShocked) return;
        if (isBurning) return;

        playerState = PlayerStates.Idle;
        yCycles = (yCycles + Time.deltaTime); // one sin movement for y tranform and one sine movement for z rotation to make it feel like floating
        zCycles = (zCycles + Time.deltaTime);
        float y1 = yCycles / yPeriod;
        float z1 = zCycles / zPeriod;

        float sinMovementY = Mathf.Sin(y1 * Mathf.PI * 2);
        float sinMovementZ = Mathf.Sin(z1 * Mathf.PI * 6);
        float yValue = initYValue + idleYMoveMagnitude * sinMovementY;
        float zRot = initZValue + idleZRotMagnitude * sinMovementZ;
        transform.position = new Vector3(transform.position.x, yValue, transform.position.z);
        transform.localEulerAngles = new Vector3(0f, 0f, zRot);
    }

    private void ExtraTiltAndMovement()
    {
        if (useMouseControls)
        {
            if (clickPressTime != 0)
            {

                Vector2 distDiff = (Input.mousePosition - initialClickPosMouse) / clickPressTime; // gets an approx vel of ship : initial - final/time
                elapsedTime += Time.deltaTime; // for lerping
                float percentageComplete = elapsedTime / moveTiltTime; //for lerping
                float decZRot = Mathf.Lerp(zAngleAtSwipeStop, 0f, percentageComplete); // lerping z angle from zangleswipestop to zero
                float decXVal = Mathf.Lerp(distDiff.x / 1000f, 0, percentageComplete); // lerping x vel from vel above calc to z
                float clampedDecXVal = Mathf.Clamp((decXVal) + transform.position.x, -3f, 3f); // clamping the lerp value

                transform.position = new Vector3(clampedDecXVal, transform.position.y, transform.position.z); // adding the clamped value to tranform
                transform.localEulerAngles = new Vector3(0f, 0f, decZRot); // assigning lerping value to z rotation
                if (decXVal == 0) // once lerped to zero the doing the following actions
                {
                    hasFinishedClicking = false;  
                    isIdle = true;
                    percentageComplete = 0f;
                    elapsedTime = 0f;
                    clickPressTime = 0f;
                    zAngleAtSwipeStop = 0f;
                    initYValue = transform.position.y;
                    transform.localEulerAngles = Vector3.zero;
                    initialClickPosMouse = Vector3.zero;
                }
            }
        }
    }


    public void PlayerMovement()
    {
        if (Time.timeScale == 0) return;
        if (useMouseControls && !isFrozen && !isBurning && !isShocked)
        {
            if(Input.touchCount <= 1)
                MouseControls();
        }
    }
    //void CheckIfInsideTouchArea()
    //{
    //    if ((Input.mousePosition.x <= screenWidth - 50) && (Input.mousePosition.x >= 0 + 50)) 
    //        && (Input.mousePosition.y <= screenHeight - 50 && Input.mousePosition.y >= 0 + 50)
    //}

    /// <summary>
    /// Controls the movement of the player 
    /// including clamping and smoothing out movement.
    /// Handles mouse button state related functioning 
    /// of player
    /// </summary>
    public void MouseControls()
    {

        //Debug.Log("Moving 1");
        if (Input.GetMouseButtonDown(0) && !touchInAction) // once the player has started clicking (the first frame)
        {
            //Debug.Log("Moving 2");
            touchInAction = true;
            hasFinishedClicking = false;

            initialClickPosMouse = Input.mousePosition;
        }
        if (Input.GetMouseButton(0)) // while the player has kept the mouse button clicking
        {
            if (cpuPos == PlayerPos.Bottom)
            {
                float initDownPos = Input.GetAxis("Mouse X"); // get the change in x
                float clampedInitDownPos = Mathf.Clamp(initDownPos, -1f, 1f); // clamping it for z rotation
                if (Mathf.Abs(initDownPos) > swipeThreshold)
                {
                    // some necessary checks 
                    isIdle = false;
                    playerState = PlayerStates.Moving;
                    transform.localEulerAngles = Vector3.zero;
                    transform.position = new Vector3(transform.position.x, startYPos, transform.position.z);
                    zCycles = 0f;
                    yCycles = 0f;
                    // checks end

                    clickPressTime += 1;                                                                               // calculating time of effective mouse clicking
                    float changedXVal = transform.position.x + (initDownPos * 10f * Time.deltaTime); // modifying the changed x with the player speed
                    float clampedXVal = Mathf.Clamp(changedXVal, playerMovementXLimit, playerMovementYLimit);         // clamping x transfrom
                                                                                                                      //transform.position = new Vector3(clampedXVal, transform.position.y, transform.position.z);       // adding the changed x to transform
                    Vector3 newPos = new Vector3(clampedXVal, transform.position.y, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, newPos, 1f);
                    transform.eulerAngles = new Vector3(0f, 0f, -clampedInitDownPos * maxTilt);                       // changing z rotation of ship
                }
            }
            else
            if (cpuPos == PlayerPos.Left)
            {
                Debug.Log("Moving 3");
                float initDownPos = Input.GetAxis("Mouse Y"); // get the change in x
                float clampedInitDownPos = Mathf.Clamp(initDownPos, -1f, 1f); // clamping it for z rotation
                                                                              //print(clampedInitDownPos);
                if (Mathf.Abs(initDownPos) > swipeThreshold)
                {
                    // some necessary checks 
                    isIdle = false;
                    playerState = PlayerStates.Moving;
                    transform.localEulerAngles = Vector3.zero;
                    transform.position = new Vector3(transform.position.x, startYPos, transform.position.z);
                    zCycles = 0f;
                    yCycles = 0f;
                    // checks end

                    clickPressTime += 1;                                                                               // calculating time of effective mouse clicking
                    float changedXVal = transform.position.z + (initDownPos * playerSpeed * 10f * Time.deltaTime); // modifying the changed x with the player speed
                    /*float clampedXVal = Mathf.Clamp(changedXVal, playerMovementXLimit, playerMovementYLimit);         // clamping x transfrom*/
                                                                                                                      //transform.position = new Vector3(clampedXVal, transform.position.y, transform.position.z);       // adding the changed x to transform
                    Vector3 newPos = new Vector3(transform.position.x, transform.position.y, changedXVal);
                    transform.position = Vector3.MoveTowards(transform.position, newPos, 1f);
                    transform.eulerAngles = new Vector3(0f, 0f, -clampedInitDownPos * maxTilt);                       // changing z rotation of ship
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            touchInAction = false;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);

            //hasFinishedClicking = true;
            //StartCoroutine(StopJitteryMovement()); // once mouse click is lifted we have to enable extratiltandmovement method so cheking its bool 
            //calculating z and swipe stop which will be used in extra tilt and movement method

            hasFinishedClicking = true;
            if (transform.localEulerAngles.z > 180f)
            {
                zAngleAtSwipeStop = transform.localEulerAngles.z - 360f;
            }
            else
            {
                zAngleAtSwipeStop = transform.localEulerAngles.z;
            }
        }
        //Debug.Log("Moving 4");
    }

    IEnumerator StopSnappingMovement()
    {
        yield return new WaitForSeconds(0.5f);
        touchInAction = true;
    }

    //private void TouchControls()
    //{
    //    if (Input.touchCount == 1)
    //    {
    //        Touch tZero = Input.GetTouch(0);
    //        if (Input.touches[0].phase == TouchPhase.Began)
    //        {
    //            isDraging = true;
    //            hasFinishedClicking = false;
    //            startTouch = Input.touches[0].position;
    //            initialClickPos = Input.touches[0].position;
    //        }
    //        else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
    //        {
    //            isDraging = false;
    //            finalClickPos = Input.touches[0].position;
    //            isIdle = false;
    //            startTouch = swipeDelta = Vector2.zero;
    //            if (transform.localEulerAngles.z > 180f)
    //            {
    //                zAngleAtSwipeStop = transform.localEulerAngles.z - 360f;
    //            }
    //            else
    //            {
    //                zAngleAtSwipeStop = transform.localEulerAngles.z;
    //            }
    //            hasFinishedClicking = true;
    //        }
            
    //        else
    //        {
                
    //        }

    //        swipeDelta = Vector2.zero;
    //        if (isDraging)
    //        {
    //            if (Input.touches.Length == 1)
    //                swipeDelta = Input.touches[0].position - startTouch;
    //        }
    //        if (tZero.deltaPosition.magnitude < swipeThreshold)
    //        {
    //            //swipeDelta = Vector2.zero;
    //            startTouch = Input.touches[0].position;
                
    //        }
    //        else
    //        {
    //            clickPressTime += 1;
    //            isIdle = false;
    //            playerState = PlayerStates.Moving;
    //            initYValue = startYPos;
    //            transform.localEulerAngles = Vector3.zero;
    //            transform.position = new Vector3(transform.position.x, startYPos, transform.position.z);
    //            zCycles = 0f;
    //            yCycles = 0f;
    //            float clampedSwipeDelta = Mathf.Clamp(swipeDelta.x, -1f, 1f);
    //            float xTouchDistance = swipeDelta.x * playerSpeed;
    //            float xValVehicle = xTouchDistance + transform.position.x;
    //            float clampedXValTouch = Mathf.Clamp(xValVehicle, -3f, 3f);

    //            transform.position = new Vector3(clampedXValTouch, transform.position.y, transform.position.z);
    //            transform.eulerAngles = new Vector3(0f, 0f, -clampedSwipeDelta * maxTilt);
                
    //        }



    //    }
    //}
}




