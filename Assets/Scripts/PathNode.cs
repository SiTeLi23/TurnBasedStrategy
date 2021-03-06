using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 

{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    //store the refernce of last path node we came from
    private PathNode cameFromPathNode;
    private bool isWalkable = true;


    public PathNode(GridPosition gridPosition) 
    {
        this.gridPosition = gridPosition;
    
    }


    public override string ToString()
    {
    
        //type all current units within the grid
        return gridPosition.ToString();
    }


    public int GetGcost() 
    {

        return gCost;
    }

    public int GetHcost()
    {

        return hCost;
    }

    public int GetFcost()
    {

        return fCost;
    }


    public void SetGCost(int gCost) 
    {

        this.gCost = gCost;
    }

    public void SetHCost(int hCost)
    {

        this.hCost = hCost;
    }

    public void CalculateFCost() 
    {
        fCost = gCost + hCost;
    }

    public void ResetCameFromPath() 
    {
        cameFromPathNode = null;
    }


    public void SetCameFromPathNode(PathNode pathNode) 
    {
        cameFromPathNode = pathNode;
    
    }

    public PathNode GetCameFromPathNode()
    {
         return cameFromPathNode;

    }


    public GridPosition GetGridPosition() 
    {

        return gridPosition;
    }

    public bool IsWalkable() 
    {
        return isWalkable;
    }

    public void SetIsWalkable(bool isWalkable) 
    {
        this.isWalkable = isWalkable;
    }

}
