using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;



    public static event EventHandler OnAnyActionPointsChanged;


    //current gridposition
    private GridPosition gridPosition;

    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;

    [SerializeField]private int actionPoints = ACTION_POINTS_MAX;


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

        //subscribe to turn end event
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
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



    #region Action Points System

    //try to check if this Unit's action points is enough to pay for the action
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction) 
    {
        if (CanSpendActionPointsToTakeAction(baseAction)) 
        {

            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else 
        {
            return false;
        }
    
    }



    //detect if we can spend certain action points on this action
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction) 
    {

        if(actionPoints>= baseAction.GetActionPointsCost()) 
        {
            return true;
        }
        else 
        {

            return false;
        }
    }


    private void SpendActionPoints(int amount) 
    {

        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }


    //gettter for action points
    public int GetActionPoints() 
    {

        return actionPoints;
    }

    #endregion



    private void TurnSystem_OnTurnChanged(object sender,EventArgs e) 
    {
        //reset the action Points
        actionPoints = ACTION_POINTS_MAX;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);

    }

}
