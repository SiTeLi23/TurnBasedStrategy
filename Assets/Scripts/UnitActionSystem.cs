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
        //move Unit to a new position where mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            //select a unit , but not moving when selecting unit, so selecting and moving won't happened at same frame
           if( TryHandleUnitSelection()) return;

            //move that selected unit
            selectedUnit.Move(MouseWorld.GetPosition());

        }
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
