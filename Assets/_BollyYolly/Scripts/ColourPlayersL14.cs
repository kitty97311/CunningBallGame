using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPlayersL14 : MonoBehaviour
{
    [SerializeField] Material redMaterialU;
    [SerializeField] Material blueMaterialU;
    [SerializeField] Material greenMaterialU;
    [SerializeField] Material yellowMaterialU;

    [SerializeField] Material redMaterialL;
    [SerializeField] Material blueMaterialL;
    [SerializeField] Material greenMaterialL;
    [SerializeField] Material yellowMaterialL;

    void Start()
    {
        StartCoroutine(TraverseChild());
    }
    IEnumerator TraverseChild()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (Transform child in transform)
        {
            ChangeCarMaterial(child.gameObject);
        }
    }
    void ChangeCarMaterial(GameObject go)
    {
        string myPlayerTag = go.tag;

        switch (myPlayerTag)
        {
            case "Player1":
                go.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().materials = new Material[] { yellowMaterialU, yellowMaterialL };
                break;
            case "Player2":
                go.transform.GetChild(2).GetChild(1).GetComponent<Renderer>().materials = new Material[] { redMaterialU, redMaterialL };
                break;
            case "Player3":
                go.transform.GetChild(2).GetChild(1).GetComponent<Renderer>().materials = new Material[] { blueMaterialU, blueMaterialL };
                break;
            case "Player4":
                go.transform.GetChild(2).GetChild(1).GetComponent<Renderer>().materials = new Material[] { greenMaterialU, greenMaterialL };
                break;
        }
    }
}
