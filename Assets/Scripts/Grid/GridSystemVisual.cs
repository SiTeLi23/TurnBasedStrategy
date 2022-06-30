using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    //creating a 2d array to store all the visualprefab
    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

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

    }

    private void Start()
    {
        //instantiate an array first
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(),LevelGrid.Instance.GetHeight()];


        //cycle through all the grids we set in the scene
        for(int x = 0; x<LevelGrid.Instance.GetWidth(); x++) 
        {
            for(int z =0; z<LevelGrid.Instance.GetHeight(); z++) 
            {
                GridPosition gridPosition = new GridPosition(x, z);

            //create a visual prefab on each grid position
            Transform gridSystemVisualSingleTransform =
            Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                //get each visual prefab's reference and add it to the array
                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            
            }
        
        }
    }


    private void Update()
    {
        UpdateGridVisual();
    }



    public void HideAllGridPosition() 
    {
       for(int x=0; x < LevelGrid.Instance.GetWidth(); x++) 
        {
           for(int z=0; z < LevelGrid.Instance.GetHeight(); z++) 
            {
                gridSystemVisualSingleArray[x, z].Hide();
            
            
            }
        
        
        }


    }


    public void ShowGridPositionList(List<GridPosition> gridPositionList) 
    {
        foreach(GridPosition gridPosition in gridPositionList) 
        {
            //only show the gridpositio which is within the list
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        
        }

    }



    private void UpdateGridVisual() 
    {
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        ShowGridPositionList(selectedUnit.GetMoveAction().GetValidActionGridPositionList());
    
    }


}
