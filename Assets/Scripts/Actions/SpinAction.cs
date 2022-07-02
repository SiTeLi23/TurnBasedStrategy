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
          
            //we call the delegate which store a reference to a function within other scripts no matter it's public or private
            ActionComplete();
        }

    }


    //action name
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        //we store the function reference we received from other scripts first
        ActionStart(onActionComplete);
        totalSpinAmount = 0f;

    }


    public override string GetActionName()
    {
        return "SPIN";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {


        //check unit grid position
        GridPosition unitGridPosition = unit.GetGridPosition();

        //return a list that only contain this unit's Grid Position
        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    //action cost 
    public override int GetActionPointsCost()
    {
        return 2;
    }



}
