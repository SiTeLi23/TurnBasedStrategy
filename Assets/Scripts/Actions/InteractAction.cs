using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractAction : BaseAction
{

    private int maxInteractDistance = 1;


    private void Update()
    {
        if (!isActive) return;

       
    }


    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        //cycle through all the x and z 
        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {

            //getting all the potential grids within the max range
            for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);

                //get the final gridPosition 
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;


                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // if this final gridPosition is not valid , keep checking the next grid
                    continue;
                }

                //get the door
                Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);

                if(door == null) 
                {
                    //no door on this grid position, ignore
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
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);

        door.Interact(OnInteractComplete);
        ActionStart(onactionComplete);
    }

    private void OnInteractComplete() 
    {
        ActionComplete();
    }

}
