using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


//adding keyword abstract will not let other scripts to create an instance of this specific class,
//just to make sure not accidently creat and instance of this class
public abstract class BaseAction : MonoBehaviour
{


    //protected can prevent other class change this value, but class that extend this class will be able to get access to those data
    protected Unit unit;
    protected bool isActive;

    //a field for delegate, store the type of function reference we received
    protected Action onActionComplete;


    //using virtual so child class can override such method
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    //abstract mean force other child script to instantiate this function
    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onactionComplete);
    

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition) 
    {
        //get the list of all valid grids
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();

        //if this gridPosition is within the list, then this gridPosition is valid gridPosiiton
        return validGridPositionList.Contains(gridPosition);

    }

    public abstract List<GridPosition> GetValidActionGridPositionList();


    public virtual int GetActionPointsCost()
    {
        return 1;
    
    }



    protected void ActionStart(Action onActionComplete) 
    {
       //active this action
        isActive = true;
        //get the clear the busy state function  and stored in the script as a reference
        this.onActionComplete = onActionComplete;
    
    }


    protected void ActionComplete() 
    {
        //disactive this action
        isActive = false;
       //clear the busy state
        onActionComplete();
    
    }

    
}
