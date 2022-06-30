using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpinAction : BaseAction
{


    private float totalSpinAmount;


    private void Update()
    {
        if (!isActive) return;

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f) 
        {
            isActive = false;
            //we call the delegate which store a reference to a function within other scripts no matter it's public or private
            onActionComplete();
        }
        
    }


    //action name
    public void Spin(Action onSpinComplete) 
    {
        //we store the function reference we received from other scripts first
        this.onActionComplete = onSpinComplete;

        isActive = true;
        totalSpinAmount = 0f; 
      
    }


    public override string GetActionName()
    {
        return "SPIN";
    }
}
