using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;


    [SerializeField]private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;

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


    private void Start()
    {
        //move action should be the default action
        SetSelectedUnit(selectedUnit);
    }
    private void Update()
    {

        if (isBusy) return;


        //if mouse is over a UI button or game object, do not take action 
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //select a unit , but not moving when selecting unit, so selecting and moving won't happened at same frame
        if (TryHandleUnitSelection())  return; 

       

        HandleSelectedAction();

      

    }



    private void HandleSelectedAction() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            //transform the mouse world position into a mouse gridPosition, so mouse can only work with gridPosition but not the world position
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            //if the selected grid position is  not valid, do nothing
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;

            //if this unit can't afford the selection action cost, do nothing
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction)) return;
            
            //if the grid is valid and unit have enough points to afford for action, run that action
            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            //invoke event when the action has been taken
            OnActionStarted.Invoke(this, EventArgs.Empty);
                
        }
    
    }








    //detect whether there is a action working currently
    private void SetBusy() 
    {

        isBusy = true;
        //fire bool version event
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy() 
    {

        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }








    #region Unit Selection System
    private bool TryHandleUnitSelection() 
    {
        //test if we clicking on a unit
        if (Input.GetMouseButtonDown(0))
        {
            //cast a ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //get the selected Unit if the raycast hit a Unit
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit) 
                    {
                        //if current unit has already been selected, we don't want to select this unit
                        return false;
                    }

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;

    }

    private void SetSelectedUnit(Unit unit) 
    {
        //set current selected unit
        selectedUnit = unit;

        //current unit's action is set to move right now
        SetSelectedAction(unit.GetMoveAction());

        //if there's a event, fire this event to all listeners
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);

        
    }

    public void SetSelectedAction(BaseAction baseAction) 
    {
        selectedAction = baseAction;

        //if there's a event, fire this event to all listeners
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);

    }



    //getter for selected unit and selected Action
    public Unit GetSelectedUnit() 
    {
        return selectedUnit;
    
    }

    public BaseAction GetSelectedAction() 
    {

        return selectedAction;
    }


    #endregion

}
