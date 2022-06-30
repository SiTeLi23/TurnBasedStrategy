using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    
    public event EventHandler OnSelectedUnitChanged;


    [SerializeField]private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;


    private bool isBusy;

    private void Awake()
    {
        #region Singleton Pattern
        if (Instance != null) 
        {
            Debug.Log("There's more than one UnitActionSystem " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion
    }



    private void Update()
    {

        if (isBusy) return;

        //move Unit to a new position where mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            //select a unit , but not moving when selecting unit, so selecting and moving won't happened at same frame
           if( TryHandleUnitSelection()) return;

           //transform the mouse world position into a mouse gridPosition, so mouse can only work with gridPosition but not the world position
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            //if the selected grid position is valid, move that selected unit to that grid position
            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition)) 
            {
                SetBusy();
                selectedUnit.GetMoveAction().Move(mouseGridPosition,ClearBusy);
            }

        }

        if (Input.GetMouseButtonDown(1)) 
        {
            SetBusy();
            selectedUnit.GetSpinAction().Spin(ClearBusy);
        }

    }




    //detect whether there is a action working currently
    private void SetBusy() 
    {

        isBusy = true;
    }

    private void ClearBusy() 
    {

        isBusy = false;
    }








    #region Unit Selection System
    private bool TryHandleUnitSelection() 
    {
        //cast a ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //get the selected Unit if the raycast hit a Unit
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;

    }

    private void SetSelectedUnit(Unit unit) 
    {
        selectedUnit = unit;

        //if there's a event, fire this event whenever selected an unit 
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);

        
    }

    //getter for selected unit
    public Unit GetSelectedUnit() 
    {
        return selectedUnit;
    
    }


    #endregion

}
