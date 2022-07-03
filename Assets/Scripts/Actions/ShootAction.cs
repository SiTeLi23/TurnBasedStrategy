using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    public static event EventHandler<OnShootEventArgs> OnAnyShoot;

   public event EventHandler<OnShootEventArgs> OnShoot;

   //customize an event args for us to pass through some data variable
   public class OnShootEventArgs : EventArgs 
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    
    }

    private enum State 
    {
       Aiming,
       Shooting,
       Cooloff,
    
    }
    [SerializeField] private LayerMask obstacleLayerMask;
    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

 


   
    private int maxShootDistance = 7;

    private void Update()
    {
        if (!isActive) return;


        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                //rotate to target
                Vector3 aimDir = (targetUnit.GetWorldPosition() - transform.position).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.Shooting:
                if (canShootBullet) 
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
               
                break;

        }



        if (stateTimer < 0f)
        {
            NextState();
           
        }

    }

    //state machine for shooting process
    private void NextState() 
    {
        switch (state) 
        {
            case State.Aiming:
                if (stateTimer < 0f)
                {

                    state = State.Shooting;
                    float shootingStateTime = 0.1f;
                    stateTimer = shootingStateTime;
                }

                break;
            case State.Shooting:
                if (stateTimer < 0f)
                {

                    state = State.Cooloff;
                    float coolOffStateTime = 0.5f;
                    stateTimer = coolOffStateTime;
                }
                break;
            case State.Cooloff:
                if (stateTimer < 0f)
                {
                    //shooting action complete
                    ActionComplete();
                }
                break;

        }

      
    
    }


    private void Shoot() 
    {
        //fire event
        OnAnyShoot?.Invoke(this, new OnShootEventArgs { targetUnit = targetUnit, shootingUnit = unit }); ;
        OnShoot?.Invoke(this, new OnShootEventArgs { targetUnit = targetUnit, shootingUnit = unit }); ;
        //cause damage
        targetUnit.Damage(40);
    }



    public override string GetActionName()
    {
        return "Shoot";
    }


    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public  List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        //cycle through all the x and z 
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {

            //getting all the potential grids within the max range
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);

                //get the final gridPosition 
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;


                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // if this final gridPosition is not valid , keep checking the next grid
                    continue;
                }

                //doing circle shoot range, make sure all value are positive 
                int testDistance =Mathf.Abs( x) + Mathf.Abs(z);
                //since testDistance is bigger than shotting range, it will stop on where target is
                if(testDistance > maxShootDistance) 
                {
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

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float unitShoulderHeight = 1.7f;

               if( Physics.Raycast(
                    unitWorldPosition + Vector3.up * unitShoulderHeight,
                    shootDir,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                    obstacleLayerMask))
               {
                    //if this raycast hit something, means it blocked by an obstacle
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

        //get the target unit
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        

        state = State.Aiming;
        float aimingStateTime = 0.5f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

      //we store the function reference we received from other scripts first 
      //make sure all the logic done first then we called the event
        ActionStart(onactionComplete);
    }


    //getter for target unit
    public Unit GetTargetUnit() 
    {
        return targetUnit;
    }

    //getter for shooting range

    public int GetMaxShootDistance() 
    {
        return maxShootDistance;
    
    }



    //how many points will AI get if it take this move
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);


         //enemy AI will  tend to shoot the target with least health point first
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1- targetUnit.GetHealthNormalized()) * 100f)

        };
    }



    public int GetTargetCountAtPosition(GridPosition gridPosition) 
    {
        //return how many shootable grid/unit we have so we can make calculation for enemy ai move
       return GetValidActionGridPositionList(gridPosition).Count;
    }


}
