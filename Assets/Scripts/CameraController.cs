using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  
    void Update()
    {
        //Movement Handler
        Vector3 inputMoveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) 
        {
            inputMoveDir.z += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x += 1f;
        }


        //we can't directly use the transform.position because if we rotate the camera, then the camra will still move according to its own direction

        float moveSpeed = 10f;

        //so we multipley these two value and now the movement won't be based on the rotation
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;

        transform.position += moveVector * moveSpeed * Time.deltaTime;




        //Rotation Handler
        Vector3 rotationVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y += 1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y -= 1f;
        }

        float rotationSpeed = 100f;

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;


    }
}
