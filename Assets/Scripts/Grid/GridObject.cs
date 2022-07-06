using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{
    


    //know which gridsSystem creat this grid object
    private GridSystem<GridObject> gridSystem;

    //know this gridobjects' grid position
    private GridPosition gridPosition;

    //get a list of current units on this grid
    private List<Unit> unitList;

    private IInteractable interactable;
    
    public GridObject(GridSystem<GridObject> gridSystem,GridPosition gridPosition) 
    {
        this.gridSystem = gridSystem;

        this.gridPosition = gridPosition;

        unitList = new List<Unit>();
    
    }


    //cause we have override the tostring function within our custom data type[gridPosition] , we can call toString() directly to show what we want
    public override string ToString()
    {
        string unitString = "";
        foreach(Unit unit in unitList) 
        {
            unitString += unit + "\n";
        
        }
        //type all current units within the grid
        return gridPosition.ToString()+ "\n" + unitString ;
    }




    #region Handle Unit

    public void AddUnit(Unit unit) 
    {

        unitList.Add(unit);
        
    }


    public List<Unit> GetUnitList() 
    {
        return unitList;
    }


    public void RemoveUnit(Unit unit) 
    {

        unitList.Remove(unit);
    }


    public bool HasAnyUnit() 
    {

        return unitList.Count > 0;
    }

    public Unit GetUnit() 
    {
        if (HasAnyUnit()) 
        {
            //we only want to get the first unit cause there's chance when another unit moving to the same grid
            return unitList[0];
        }

        else 
        {

            return null;
        }
    
    }

    #endregion


    public IInteractable GetInteractable() 
    {
        return interactable;
    }

    public void SetInteractable(IInteractable interactable) 
    {
        this.interactable = interactable;
        
    }


}
