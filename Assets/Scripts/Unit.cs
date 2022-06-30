using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    //current gridposition
    private GridPosition gridPosition;

    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;


    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        //transform unit's current position into gridposition
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition,this);
    }

    private void Update()
    {

        #region Tracking Current Grid

        //get unit's current new gridPosition
        GridPosition newgridPosition = LevelGrid.Instance.GetGridPosition(transform.position);


        //if new grid position is not the same as current gridPosition
        if(newgridPosition!= gridPosition) 
        {
            //Unit changed Grid Position
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newgridPosition);

            //update current gridPosition to new gridPosition
            gridPosition = newgridPosition;
        }

        #endregion


    }



    //give other scripts access to the move action script reference
    public MoveAction GetMoveAction() 
    {
        return moveAction;
    }

    //give other scripts access to the spin action script reference
    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    //give other scripts access to current grid where this unit locate
    public GridPosition GetGridPosition() 
    {

        return gridPosition;
    }

    //give other scripts access to action array where this unit locate
    public BaseAction[] GetBaseActionArray() 
    {
        return baseActionArray;
    }

}
