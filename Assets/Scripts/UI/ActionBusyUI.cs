using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionBusyUI : MonoBehaviour
{
    
    void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;

        Hide();
    }





    private void Show() 
    {
        gameObject.SetActive(true);
    }


    private void Hide() 
    {
        gameObject.SetActive(false);
    
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        gameObject.SetActive(isBusy);


    }



}
