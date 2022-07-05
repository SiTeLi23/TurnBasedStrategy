using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static event EventHandler OnAnySwordHit;

    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;

    private enum State 
    {
       SwingSwordBeforeHit,
       SwingSwordAfterHit
    
    }

    private int maxSwordDistance = 1;
    private State state;
    private float stateTimer;
    private Unit targetUnit;

    private void Update()
    {
        if (!isActive) 
        {

            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingSwordBeforeHit:
                //rotate to target
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingSwordAfterHit:
             
                break;
         

        }



        if (stateTimer < 0f)
        {
            NextState();

        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingSwordBeforeHit:
                    state = State.SwingSwordAfterHit;
                    float afterHitStateTime = 0.5f;
                    stateTimer = afterHitStateTime;
                    targetUnit.Damage(100);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                    break;

            case State.SwingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
      

        }



    }

    public override string GetActionName()
    {
        return "Swrod";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        //cycle through all the x and z 
        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {

            //getting all the potential grids within the max range
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);

                //get the final gridPosition 
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;


                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // if this final gridPosition is not valid , keep checking the next grid
                    continue;
                }


                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //if this grid has no unit, keep checking the next grid
                    continue;
                }

                //get a unit from valid grid positions
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);


                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //the enemy also going to share this function
                    //both units on same 'team'
                    //to check if the target unit is enemy , is different from the Isenemy() of this unit

                    continue;
                }

                //if it is valid, add this grid position into the valid gridposition list
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onactionComplete)
    {

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onactionComplete);
    }


    public int GetMaxSwordDistance() 
    {
        return maxSwordDistance;
    }
}
