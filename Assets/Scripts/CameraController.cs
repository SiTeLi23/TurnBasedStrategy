using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    [SerializeField]private CinemachineVirtualCamera cinemachineVirtualCamera;


    //if we want to handle smoothing, we need to keep tracking the value overtime,thus we need to define offset value here
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        //get access to the virtual camera offset 
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    void Update()
    {

        HandleMovement();
        HandleRotation();
        HandleZoom();



    }

    private void HandleMovement() 
    {
        #region Movement Handler
        //Movement Handler
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();



        //we can't directly use the transform.position because if we rotate the camera, then the camra will still move according to its own direction

        float moveSpeed = 10f;

        //so we multipley these two value and now the movement won't be based on the rotation
        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;

        transform.position += moveVector * moveSpeed * Time.deltaTime;

        #endregion

    }


    private void HandleRotation()
    {
        #region Rotate Handler
        //Rotation Handler
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();
     
        float rotationSpeed = 100f;

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
        #endregion

    }


    private void HandleZoom()
    {

        #region Zoom In Out Handler



        float zoomIncreaseAmount = 1f;

        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;

        //set limitation for zoom in out 
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        //update and smooth the follow offset value
        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, zoomSpeed * Time.deltaTime);


        #endregion
    }


}
