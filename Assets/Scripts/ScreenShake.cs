using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

    public static ScreenShake Instance { get; private set; }

    private void Awake()
    {
        #region Singleton Pattern
        if (Instance != null)
        {
            Debug.Log("There's more than one ScreenShake " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion

        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

    }
    private CinemachineImpulseSource cinemachineImpulseSource;

   


    public void Shake(float intensity = 1f) 
    {
    
    cinemachineImpulseSource.GenerateImpulse(intensity);
    
    }

}
