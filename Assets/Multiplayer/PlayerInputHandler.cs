using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerMechanics playerMechanics;

    Vector3 positionVector;
    Vector3 rotationVector;
    public bool isUpdated;
    private void Start()
    {
        playerMechanics = transform.GetComponentInChildren<PlayerMechanics>();

        //positionVector = playerMechanics.transform.position;
        //rotationVector = playerMechanics.transform.eulerAngles;
    }
    private void Update()
    {
        Debug.Log("Start Update Method InputHandler");

        positionVector = playerMechanics.transform.position;
        rotationVector = playerMechanics.transform.eulerAngles;

        playerMechanics.PlayerMovement();
        isUpdated = true;
    }

    public NetworkInputData GetNetworkInputData()
    {
        Debug.Log("Start GetNetworkInputData Input Handler");
        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.position = positionVector;
        networkInputData.rotation = rotationVector;

        return networkInputData;
    }
}
