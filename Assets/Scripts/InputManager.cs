#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance { get; private set; }

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        #region Singleton Pattern
        if (Instance != null)
        {
            Debug.Log("There's more than one InputSystem " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion

        //create the new input system
      playerInputActions =  new PlayerInputActions();
        //enable action maps(our custome inputs)
        playerInputActions.Player.Enable();
    }


    public Vector2 GetMouseScreenPosition() 
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    
    }

    public bool IsMouseButtonDownThisFrame() 
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }


    public Vector2 GetCameraMoveVector() 
    {
#if USE_NEW_INPUT_SYSTEM
      return  playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        //Movement Handler
        Vector3 inputMoveDir = new Vector2(0,  0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x += 1f;
        }


        return inputMoveDir;
#endif

    }

    public float GetCameraRotateAmount() 
    {
#if USE_NEW_INPUT_SYSTEM
       return playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
        float rotateAmount = 0f;

        if (Input.GetKey(KeyCode.Q)) 
        {
            rotateAmount = +1;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotateAmount = -1;
        }
        return rotateAmount;
#endif

    }


    public float GetCameraZoomAmount() 
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
        float zoomAmount = 0f;

        if (Input.mouseScrollDelta.y > 0)
        {
            //if we scroll up the mouse, we zoom in
            zoomAmount = -1f;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            //if we scroll down the mouse, we zoom out
            zoomAmount =+1f;
        }

        return zoomAmount;
#endif
    }

    }
