using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCamController : MonoBehaviour
{
    Vector3 cameraInitPostion;
    Vector3 cameraInitLocalPostion;
    Vector3 cameraInitRotation;
    Vector3 cameraInitLocalRotation;

    private void Start()
    {
        cameraInitPostion = transform.position;
        cameraInitLocalPostion = transform.localPosition;
        cameraInitRotation = transform.eulerAngles;
        cameraInitLocalRotation = transform.localEulerAngles;
    }
    private void Update()
    {
        transform.position = cameraInitPostion;
        transform.localPosition = cameraInitLocalPostion;
        transform.eulerAngles = cameraInitRotation;
        transform.localEulerAngles  = cameraInitLocalRotation;
    }
}
