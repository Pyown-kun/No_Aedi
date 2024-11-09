using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    

    [SerializeField] private Material AIMMaterial;
    [SerializeField] private Material changeAIMMaterial;

    Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
    }

    private void Update()
    {
   
        renderer.sharedMaterial = AIMMaterial;

        if (Input.GetMouseButton(1))
        {
 
            renderer.sharedMaterial = changeAIMMaterial;
        }

  
    }
}
