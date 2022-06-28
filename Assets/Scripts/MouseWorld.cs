using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{

    public static MouseWorld instance;

    [SerializeField]private LayerMask mousePlaneLayerMask;

    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {

        //move the mouse visual point to where the mouse's world position is
        //transform.position = MouseWorld.GetPosition();
    }

    //get mouse position
    public static Vector3 GetPosition() 
    {
        //shooting a ray from main camera based on the mouse screen pixel position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //use that ray to create a raycast to get hit information
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);

        return raycastHit.point;
    }


}
