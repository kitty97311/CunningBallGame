using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialScriptShader : MonoBehaviour
{

    Material[] carMaterials;
    Renderer[] renderers;
    public Texture iceTexture;
    public Material[] carMaterialsNew;
    public Renderer car;

    public Material iceMat;

    // Start is called before the first frame update
    void Start()
    {
        //carMaterails = gameObject.GetComponentsInChildren<Renderer>().material;
        //print(carMaterails +  "  hell0");
        //print("hemlo");
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            //print(renderer);
            carMaterials = renderer.materials;
            print(carMaterials);
        }
        //carMaterials = GetComponentInChildren<Renderer>().materials;
        //carMaterials = car.materials;
        //carMaterials[0] = carMaterialsNew[0];
        //carMaterials[1] = carMaterialsNew[1];
        //car.materials = carMaterials;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                //carMaterialsNew = renderer.materials;
                carMaterials = car.materials;
                //carMaterials[0] = carMaterialsNew[0];
                //carMaterials[1] = carMaterialsNew[1];

                car.materials = carMaterialsNew;
                renderer.material = iceMat;
                print(renderer);
            }
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                //print(renderer);
                foreach(Material material in carMaterials)
                {
                    print(material.name);
                }
            }
        }
    }
}
