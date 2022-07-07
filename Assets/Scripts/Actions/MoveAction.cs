using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MoveAction : BaseAction
{

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;


    private List<Vector3> positionList;
    private int currentPositionIndex;

    [SerializeField] private int maxMoveDistance = 4;



    
    void Update()
    {

        if (!isActive) return;
        

        #region Unit Movement Logic     

        //get the direction toward target position and move to there
         Vector3 targetPosition = positionList[currentPositionIndex];
         Vector3 moveDiretion = (targetPosition - transform.position).normalized;

        //rotate player to where they face
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDiretion, rotateSpeed * Time.deltaTime);


        //make sure unit stop when reaching target position without jiggling
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
 
            float moveSpeed = 4f;
            //move position to target position
            transform.position += moveDiretion * moveSpeed * Time.deltaTime;
    
        }
        else
        {

            currentPositionIndex++;

            //if we reach the last position
            if (currentPositionIndex >= positionList.Count)
            {

                //fire event of stop moving
                OnStopMoving?.Invoke(this, EventArgs.Empty);

                //use delegate to called the reference function to set back isBusy to false
                ActionComplete();
            }

        }  
      

        #endregion

    }

    //tell the Unit to move to target position
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {

        //apply path finding , and store the return list
          List<GridPosition> pathGridPositionList =  Pathfinding.Instance.FindPath(unit.GetGridPosition(),gridPosition, out int pathLength);
          

        currentPositionIndex = 0;

        //create a new list for  storing  world position
        positionList = new List<Vector3>();

        foreach(GridPosition pathGridPosition in pathGridPositionList) 
        {
            //transforming each path finding gridposition into world position , and add to the new list we created
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        
        }

           //fire event
           OnStartMoving?.Invoke(this, EventArgs.Empty);

          ActionStart(onActionComplete);
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

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition)) 
                {
                    //if this grid is not walkable , keep checking the next grid
                    continue;
                
                }
                if (!Pathfinding.Instance.HasPath(unitGridPosition,testGridPosition))
                {
                    //if there is no path , keep checking the next grid
                    continue;

                }

                int pathfindingDistanceMultiplier = 20; //because our preset straignt and diagonal move value has multiply 20 , so when calculating path length, we also need to multiply 10
               if( Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier) 
                {
                    //path length too far , prevent moving across a long way to pass through obstacles
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



    //how many points will AI get if it take this move
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
      int targetCountAtGridPosition =  unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition*10,

        };
    }


}
