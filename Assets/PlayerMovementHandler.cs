using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovementHandler : NetworkBehaviour
{
    PlayerMechanics playerMechanics;
    void Start()
    {
        playerMechanics = transform.GetComponentInChildren<PlayerMechanics>();
    }

    public override void FixedUpdateNetwork()
    {
        //if ( GetInput(out NetworkInputData networkInputData))
        //{
        //    playerMechanics.PlayerMovement();
        //}
    }
}
