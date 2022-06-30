using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem 
{


    //This script mainly contain all the grid information
    //This script mainly responsible for converting World/Grid Position

    private int width;
    private int height;
    private float cellSize;


    //creat a 2 dimentional array
    private GridObject[,] gridObjectArray;

    //constructor
    public GridSystem(int width,int height,float cellSize) 
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];



        //draw the grid
        for(int x = 0; x < width; x++) 
        {
        
           for(int z = 0; z < height; z++) 
            {

                //create a new gridPosition
                GridPosition gridPosition = new GridPosition(x, z);

                //creat an array of new gridObjects which contain information about which gridSystem created it and its current gridPosition
                gridObjectArray[x,z] = new GridObject(this, gridPosition);
       
            }
        
        }

    } 

    //convert the grid position into world position
    public Vector3 GetWorldPosition(GridPosition gridPosition) 
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z)*cellSize;
        
    }

    //convert world position back to grid position
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition( Mathf.RoundToInt(worldPosition.x /cellSize),Mathf.RoundToInt( worldPosition.z/cellSize));

    }



    //showing debug text for each grid
    public void CreateDebugObjects(Transform debugPrefab) 
    {

        for (int x = 0; x < width; x++)
        {

            for (int z = 0; z < height; z++)
            {

                GridPosition gridPosition = new GridPosition(x, z);

                //create  debug object for eahc grid
               Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
              
                //get this debug gridprefab 
               GridDebugObject gridDebugObject=  debugTransform.GetComponent<GridDebugObject>();

                //passing  the grid object's position to that the debug gridprefab
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));

            }

        }
    }



    public GridObject GetGridObject(GridPosition gridPosition) 
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }


    //check if certain gridposition is valid position
    public bool IsValidGridPosition(GridPosition gridPosition) 
    {
        //grid only valid if it not exceed our total grid size
        return gridPosition.x >= 0 &&
               gridPosition.z >= 0 &&
               gridPosition.x < width && 
               gridPosition.z < height;
    }



    //getter for width and height
    public int GetWidth() 
    {
        return width;
    }

    public int GetHeight() 
    {
        return height;
    }


}
