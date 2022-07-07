using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


//adding keyword abstract will not let other scripts to create an instance of this specific class,
//just to make sure not accidently creat and instance of this class
public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

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

    //cycle through the list to get all valid positions
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

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    
    }


    protected void ActionComplete() 
    {
        //disactive this action
        isActive = false;
       //clear the busy state
        onActionComplete();

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    
    }


    //getter for this unit
    public Unit GetUnit() 
    {
        return unit;
    }




    public EnemyAIAction GetBestEnemyAIAction() 
    {
        List<EnemyAIAction> enemyAiActionList = new List<EnemyAIAction>();
        List<GridPosition>  validActionGridPositionList= GetValidActionGridPositionList();

        //cycle through all valid gridposition
        foreach(GridPosition gridPosition in validActionGridPositionList) 
        {
            //generate all potential  ai actiosn and add them to list
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAiActionList.Add(enemyAIAction);
        }


        //calculate all the ai actions points
        if (enemyAiActionList.Count > 0)
        {
            //sort the list to find the best aiaction at that moment(which has the most action points value)
            enemyAiActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);

            //return the value which has already been sorted and put at the 1st index
            return enemyAiActionList[0];
        }
        else 
        {
            //no possible enemy ai actions
            return null;
           
        }

    }


    //each action have this function, get an AIAction and calculate the score for certain grid position
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition); 
  
    
}
