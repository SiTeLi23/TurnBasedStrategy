using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;


    [SerializeField] private bool isEnemy;


    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawed;
    public static event EventHandler OnAnyUnitDead;


    //current gridposition
    private GridPosition gridPosition;

    //health system
    private HealthSystem healthSystem;


    //store all the available actions within this unit which will be instantiate later through UnitActionSystem
    private BaseAction[] baseActionArray;
   

    [SerializeField]private int actionPoints = ACTION_POINTS_MAX;


    private void Awake()
    {
  
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
        actionPoints = ACTION_POINTS_MAX;


    }

    private void Start()
    {
        //transform unit's current position into gridposition
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition,this);

        //subscribe to turn end event
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        //subscribe to death event
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawed?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {

        #region Tracking Current Grid

        //get unit's current new gridPosition
        GridPosition newgridPosition = LevelGrid.Instance.GetGridPosition(transform.position);


        //if new grid position is not the same as current gridPosition
        if(newgridPosition!= gridPosition) 
        {


            //update current gridPosition to new gridPosition
            //we need to update the grid position before updating grid visual
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newgridPosition; 
            
            //Unit changed Grid Position
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newgridPosition);
        }

        #endregion


    }


    //generic function which only work with class that extend base actionw
    //this function can be used to get any action type we want
    public T GetAction<T>() where T : BaseAction
    {
        //cycle through each actions attached to this unit
         foreach(BaseAction baseAction in baseActionArray) 
        {
           if(baseAction is T) 
            {
                //we nned to cast this valid base action into type T
                return (T)baseAction;
            }

        }
       return null;
    }





    //give other scripts access to current grid where this unit locate
    public GridPosition GetGridPosition() 
    {

        return gridPosition;
    }

    //give other scripts access to current world Position where this unit locate
    public Vector3 GetWorldPosition()
    {

        return transform.position;
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
        // make sure player and enemy will only refersh their action points in their turn
        if ((isEnemy && !TurnSystem.Instance.IsPlayerTurn())||(!isEnemy&&TurnSystem.Instance.IsPlayerTurn()))
        {

            //reset the action Points
            actionPoints = ACTION_POINTS_MAX;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }

    }

    //getter for telling if this unit is an enemy
    public bool IsEnemy() 
    {

        return isEnemy;
    }




    public void Damage(int damageAmount) 
    {

        healthSystem.Damage(damageAmount);
    }

     private void HealthSystem_OnDead(object sender, EventArgs e) 
    {
        //remove the unit from the grid first
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition,this);

        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    
    }

    public float GetHealthNormalized() 
    {
        return healthSystem.GetHealthNormalized();
    
    }


}
