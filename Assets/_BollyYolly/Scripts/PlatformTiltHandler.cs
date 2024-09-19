using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTiltHandler : MonoBehaviour
{
    [SerializeField] float tiltLimitXZ = 10f;
    //jerk tilt limit angle
    [SerializeField] float idleTiltLimit = 1f;
    //idle titl limit angle
    [SerializeField] float yLimitFloat = 0.1f;

    [SerializeField] GameObject platformToTilt;
    [SerializeField] GameObject playersParent;
    [SerializeField] GameObject platformParentToTilt;


    float tiltLimIdleXZ = 2f;

    Vector3 targetPos;
    Vector3 targetRot;

    Vector3 currentPosPlat;

    float idleX = 3;
    float idleZ = 3;
    float zInc = -0.031f;
    float xInc = 0.031f;
    void Start()
    {
        targetPos = new Vector3(0, yLimitFloat, 0);
        targetRot = new Vector3(idleTiltLimit, 0, -idleTiltLimit);
    }
    private void FixedUpdate()
    {
        FloatingUpDownMovement();
        FloatTiltMovement();
    }
    IEnumerator TiltPlatform()
    {
        while (gameObject.activeSelf)
        {
            Debug.Log("Trying to tilt");
            float timeToWait = Random.Range(3, 7);
            yield return new WaitForSeconds(timeToWait);

            float xAngle = Random.Range(-tiltLimitXZ, tiltLimitXZ);
            float zAngle = Random.Range(-tiltLimitXZ, tiltLimitXZ);
            platformToTilt.transform.localEulerAngles = new Vector3(xAngle, 0, zAngle);

            yield return new WaitForSeconds(2f);
        }
    }
    void FloatingUpDownMovement()
    //handles up down movement of the platform
    {
        currentPosPlat = platformToTilt.transform.position;
        platformToTilt.transform.position = Vector3.Lerp(currentPosPlat, targetPos, 0.02f);
        if ((Vector3.Distance(currentPosPlat, targetPos) <= 0.2f))
        {
            targetPos = -targetPos;
        }
    }
    void FloatTiltMovement()
    //handles floating/tilting movement of
    //the platform
    {
        {
            idleX += xInc;
            idleZ += zInc;
            if (idleX > 1.96 && idleX < 2.04)
            {
                xInc = -xInc;
                zInc = -zInc;
            }
            if (idleZ < -1.96 && idleZ > -2.04)
            {
                xInc = -xInc;
                zInc = -zInc;
            }
            platformToTilt.transform.localEulerAngles = new Vector3(idleX, 0, idleZ);
        }
    }
}
