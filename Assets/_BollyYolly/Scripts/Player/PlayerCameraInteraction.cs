using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraInteraction : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject scoreCanvas;
    [SerializeField] private float cameraShake = 2f;
    PlayerStates playerState;
    float zAngleValue;


    private void Update()
    {
        playerState = GetComponent<PlayerMechanics>().playerState;
        //Debug.Log(playerState);
        
        if (playerState == PlayerStates.Moving)
        {
            if (transform.localEulerAngles.z > 180f)
            {
                zAngleValue = transform.localEulerAngles.z - 360f;
            }
            else
            {
                zAngleValue = transform.localEulerAngles.z;
            }
            if (playerCamera && scoreCanvas != null)
            {
                playerCamera.transform.localEulerAngles = new Vector3(playerCamera.transform.localEulerAngles.x, playerCamera.transform.localEulerAngles.y, zAngleValue / cameraShake);
                scoreCanvas.transform.localEulerAngles = new Vector3(scoreCanvas.transform.localEulerAngles.x, scoreCanvas.transform.localEulerAngles.y, -zAngleValue / cameraShake);
            }
        }
        else
        {
            zAngleValue = 0f;
        }

    }
}
