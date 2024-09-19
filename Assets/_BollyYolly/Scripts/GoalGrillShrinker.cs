using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalGrillShrinker : MonoBehaviour
{
    LevelPreProcessing levelPreProcessing;
    bool ip1;
    void Start()
    {
        levelPreProcessing = FindObjectOfType<LevelPreProcessing>();
        StartCoroutine(Shrink());
    }
    public bool IP1()
    {
        StartCoroutine(Shrink());
        return ip1;
    }
    IEnumerator Shrink()
    {
        yield return new WaitForSeconds(0f);
        Debug.Log("trying");
        while(gameObject.transform.localScale.x >= 1.01f)
        {
            Debug.Log("trying 2" + transform.parent.name);
            yield return new WaitForSeconds(0.01f);

            transform.localScale = new Vector3(transform.localScale.x - 0.01f, transform.localScale.y - 0.005f, transform.localScale.z - 0.01f);
        }
        ip1 = true;
    }
}
