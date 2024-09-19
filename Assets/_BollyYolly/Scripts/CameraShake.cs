using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cineCam;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    //[SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] private float shakeTimer = 1f;
    private float shakeTimerTotal;
    float timer;
    bool kickPressed;
    [SerializeField] private float startIntensity = 1f;

    private void Awake()
    {
        cineCam = GetComponent<CinemachineVirtualCamera>();
        //CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        shakeTimerTotal = shakeTimer;
        timer = shakeTimer;
    }

    public void ShakeCamera()
    {
        kickPressed = true;
    }

    private void Update()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (kickPressed)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, (1 - (timer / shakeTimerTotal)));
            }
            else
            {
                kickPressed = false;
                timer = shakeTimer;
            }
        }
    }
}
