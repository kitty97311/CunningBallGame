using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBallKiller : MonoBehaviour
{
    LevelGameManager levelGameManager;
    private void Start()
    {
        levelGameManager = FindObjectOfType<LevelGameManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        levelGameManager.totalNoOfBalls--;
        Destroy(collision.gameObject);
    }
}
