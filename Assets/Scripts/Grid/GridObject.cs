using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    //this class contain all the grid information


    //know which gridsSystem creat this grid object
    private GridSystem gridSystem;

    //know this gridobjects' grid position
    private GridPosition gridPosition;

    //get a list of current units on this grid
    private List<Unit> unitList;
    
    public GridObject(GridSystem gridSystem,GridPosition gridPosition) 
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


    #endregion


}
