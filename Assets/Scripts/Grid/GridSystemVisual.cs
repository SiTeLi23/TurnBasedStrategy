using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystemVisual : MonoBehaviour
{

    public static GridSystemVisual Instance { get; private set; }

    [Serializable]
    public struct GridVisualTypeMaterial 
    {
        public GridVisualType gridVisualType;
        public Material material;
    
    }


    public enum GridVisualType 
    {
       White,
       Blue,
       Red,
       Yellow,
       RedSoft
    }


    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

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

        //subscribe to events , so no need to update grid every frame
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMoveGridPosition += LevelGrid_OnAnyUnitMoveGridPosition;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
       

        UpdateGridVisual();

    }

    

    private void OnDisable()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMoveGridPosition -= LevelGrid_OnAnyUnitMoveGridPosition;
        TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
        Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        List<Unit> RemainUnitList = UnitManager.Instance.GetFriendlyUnitList();
        if (RemainUnitList.Count > 0)
        {
            Unit nextUnit = RemainUnitList[0];
            UnitActionSystem.Instance.SetSelectedUnit(nextUnit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit deadUnit = sender as Unit;
        UpdateGridVisual();
        if (deadUnit.IsEnemy()) return;
        List<Unit> RemainUnitList = UnitManager.Instance.GetFriendlyUnitList();
        if (RemainUnitList.Count > 0)
        {
            Unit nextUnit = RemainUnitList[0];
            UnitActionSystem.Instance.SetSelectedUnit(nextUnit);
        }
        
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


    //show potential shooting range
    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {

        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {

                //don't include target's grid
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                //test if the gridPosition is valid
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                //add all potential shooting range grids except for the target's current grid
                gridPositionList.Add(testGridPosition);
            }

        }

        //show the final grid list
        ShowGridPositionList(gridPositionList, gridVisualType);

    }

    //show potential shooting range
    private void ShowGridPositionRange(GridPosition gridPosition,int range, GridVisualType gridVisualType) 
    {

        List<GridPosition> gridPositionList = new List<GridPosition>();

       for(int x = -range; x <= range; x++) 
        {
           for(int z = -range; z <= range; z++) 
            {

                //don't include target's grid
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                //test if the gridPosition is valid
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) 
                {
                    continue;
                }

                //draw a circle range
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                //add all potential shooting range grids except for the target's current grid
                gridPositionList.Add(testGridPosition);
            } 
        
        }

        //show the final grid list
        ShowGridPositionList(gridPositionList, gridVisualType);
    
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType) 
    {
        foreach(GridPosition gridPosition in gridPositionList) 
        {
            //only show the gridpositio which is within the list according to the gridvisual type
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        
        }

    }



    private void UpdateGridVisual()
    {
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType;

        //Different action will show differnt color grid 
        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);
                break;
            case InteractAction interactAction:
                gridVisualType = GridVisualType.Blue;
                break;


        }
        //Show all the valid grid
        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(),gridVisualType); 
    }



    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) 
    {
        UpdateGridVisual();
    
    }

    private void LevelGrid_OnAnyUnitMoveGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();

    }


    //assign materials to grid
    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType) 
    {
        foreach(GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList) 
        {
            if(gridVisualTypeMaterial.gridVisualType == gridVisualType) 
            {
                return gridVisualTypeMaterial.material;
            }
        }

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType "+ gridVisualType);
        return null;
    
    }


}
