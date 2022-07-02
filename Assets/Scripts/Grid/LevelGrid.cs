using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelGrid : MonoBehaviour
{

 //This script mainly responsible for passing through grid information to other scripts
 
    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMoveGridPosition;


    [SerializeField] private Transform gridDebugObjectPrefab;

    private GridSystem gridSystem;
    private void Awake()
    {

        #region Singleton Pattern
        if (Instance != null)
        {
            Debug.Log("There's more than one LevelGrid " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion

        //create a grid
        gridSystem = new GridSystem(10, 10, 2f);

        //tell the gridSystem to create debug Object
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }


    //we need this script to handle getting and setting units on each grid Position
    public void AddUnitAtGridPosition(GridPosition gridPosition,Unit unit) 
    {
        //get the gridObject
         GridObject gridObject =  gridSystem.GetGridObject(gridPosition);
        //set the unit to this gridobject
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        //get the gridObject
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        //get the unit which is within this gridObject
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition,Unit unit) 
    {
        //get the gridObject
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        //set the unit to null
        gridObject.RemoveUnit(unit);
    }

    //hanlde unit move gridposition, we call this function whenever unit change grid pposition
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition) 
    {
        RemoveUnitAtGridPosition(fromGridPosition,unit);

        AddUnitAtGridPosition(toGridPosition,unit);

        //fire event when any unit move to a new grid
        OnAnyUnitMoveGridPosition?.Invoke(this, EventArgs.Empty);

    
    }



    #region PassThrough Functions

    // lambda => is an automatical way of returning some value 

    //getter for gridPosition from gridSystem
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    //getter for WorldPosition from gridSystem
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    // getter for the value which checking if a grid is valid or not
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    //getter for width of grid from gridSystem
    public int GetWidth() => gridSystem.GetWidth();
    //getter for height of grid from gridSystem
    public int GetHeight() => gridSystem.GetHeight();

    //check if there's any unit in certain gridposition
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition) 
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();

    }

    //get the unit from certain grid position
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();

    }

    #endregion

}
