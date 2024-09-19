using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level13WallHandler : MonoBehaviour
{
    [SerializeField] GameObject wallReference;
    [SerializeField] Transform[] transformsForWall;
    GameObject wallInGame;
    private void Start()
    {
        wallInGame = Instantiate(wallReference, transform);
        StartCoroutine(CycleWalls());
    }
    IEnumerator CycleWalls()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            int posNumber = Random.Range(0, 11);
            wallInGame.transform.position = transformsForWall[posNumber].position;
            wallInGame.transform.rotation = transformsForWall[posNumber].rotation;

            wallInGame.SetActive(true);
            yield return new WaitForSeconds(4f);
            wallInGame.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
    }
}
