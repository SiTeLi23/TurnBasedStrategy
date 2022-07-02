using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MoveAction : BaseAction
{

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;


    private Vector3 targetPosition;


    [SerializeField] private int maxMoveDistance = 4;


    protected override void Awake()
    {
        base.Awake();

        targetPosition = transform.position;
      
       
    }

    
    void Update()
    {

        if (!isActive) return;

        #region Unit Movement Logic     
        
        //get the direction toward target position and move to there
         Vector3 moveDiretion = (targetPosition - transform.position).normalized;
        //make sure unit stop when reaching target position without jiggling
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
 
            float moveSpeed = 4f;
            transform.position += moveDiretion * moveSpeed * Time.deltaTime;
    
        }
        else
        {

            //fire event of stop moving
            OnStopMoving?.Invoke(this, EventArgs.Empty);

            //use delegate to called the reference function to set back isBusy to false
            ActionComplete();

        }  
        //rotate player to where they face
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDiretion, rotateSpeed * Time.deltaTime);

        #endregion

    }

    //tell the Unit to move to target position
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {

           ActionStart(onActionComplete);

           //set up target position
            this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

        //fire event
        OnStartMoving?.Invoke(this, EventArgs.Empty);
     
        }




    //return a list of all valid grids 
    public override List<GridPosition> GetValidActionGridPositionList() 
    {
      
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();


        //cycle through all the x and z 
        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++) 
        {

            //getting all the potential grids within the max range
            for (int z = -maxMoveDistance;z <= maxMoveDistance; z++) 
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);

                //get the final gridPosition 
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

               
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                { 
                    // if this final gridPosition is not valid , keep checking the next grid
                    continue;
                }

                if(unitGridPosition == testGridPosition) 
                {
                    //if this is the same grid position where unit current at, keep checking the next grid
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) 
                {
                    //if this grid has already occupied by an unit, keep checking the next grid
                    continue;
                }

                //if it is valid, add this grid position into the valid gridposition list
                validGridPositionList.Add(testGridPosition);
                

            }
           
        }


        return validGridPositionList;
    
    }



    //action Name
    public override string GetActionName()
    {
        return "Move";
    }


}
