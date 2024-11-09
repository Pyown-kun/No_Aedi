using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMControl : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private LayerMask layerMaskHit;
    [SerializeField] private LayerMask layerMaskHitOutler;

    LayerMask layerMask;
  
    private void Update()
    {
        layerMask = layerMaskHit;

        if (Input.GetMouseButton(1))
        {
            layerMask = layerMaskHitOutler;
        }

        Vector3 screenPosition = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            transform.position = raycastHit.point;
        }
        
    }
}
