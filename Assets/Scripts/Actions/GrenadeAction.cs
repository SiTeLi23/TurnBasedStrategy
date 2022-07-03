using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{

    [SerializeField] private Transform grenadeProjectilePrefab;
    [SerializeField] private int maxThrowDistance = 7;
  

    private void Update()
    {
        if (!isActive) return;

       
    }


    public override string GetActionName()
    {
        return "Grenade";
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
        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {

            //getting all the potential grids within the max range
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
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
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                //since testDistance is bigger than shotting range, it will stop on where target is
                if (testDistance > maxThrowDistance)
                {
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

       Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile= grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition,OnGrenadeBehaviourComplete);
        
        ActionStart(onactionComplete);


    }


    private void OnGrenadeBehaviourComplete() 
    {
        ActionComplete();
    
    }
}
