using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }




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

        //create a gridSystem
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

    //hanlde unit move gridposition
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition) 
    {
        RemoveUnitAtGridPosition(fromGridPosition,unit);

        AddUnitAtGridPosition(toGridPosition,unit);
    
    }





    // lambda => is an automatical way of returning some value 
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    

}
