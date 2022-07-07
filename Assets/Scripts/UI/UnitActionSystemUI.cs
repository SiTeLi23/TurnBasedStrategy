using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    //a list to store all action button UI
    private List<ActionButtonUI> actionButtonUIList;


    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }


    private void Start()
    {
        //subscribe to events
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void OnDisable()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted -= UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
    }



    private void CreateUnitActionButtons() 
    {
        //clear all button everytime before we update button UI
        foreach(Transform buttonTransform in actionButtonContainerTransform) 
        {
            Destroy(buttonTransform.gameObject);
        }

        //clear the list of all actionButtonUI
        actionButtonUIList.Clear();


        Unit selectedUnit =  UnitActionSystem.Instance.GetSelectedUnit();

        //get how many action this unit have , and instantiate button for each
        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray()) 
        {
            //instantiate a button
           Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);

            //get current action and  pass current action function into the button
           ActionButtonUI actionButtonUI= actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            //add the actionbuttonUI to list
            actionButtonUIList.Add(actionButtonUI);
        }
       
    
    }

    #region Listener Functions
    //listener function called when receivding event
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) 
    {

        CreateUnitActionButtons();

        //when we recreate buttons , we also need to update selected button visual
        UpdateSelectedVisual();
        //update selected Unit's action points
        UpdateActionPoints();

    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e) 
    {
        //since we already spent the action points before this event sent, we could just update the text for action points left
        UpdateActionPoints();
    
    }


    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        //selected Action showed a green border
        UpdateSelectedVisual();

    }


    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        //every time the turn end, update current Action Points text
        UpdateActionPoints();
        

    }

    private void  Unit_OnAnyActionPointsChanged(object sender,EventArgs e) 
    {
        //make sure once the action points changed , we will update the text
        //this can prevent certain case when ActionPoints not updated correctly
        UpdateActionPoints();
    
    }

    #endregion



    //the selected button will show its green border
    private void UpdateSelectedVisual() 
    {
    
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList) 
        {
            actionButtonUI.UpdateSelectedVisual();
        }


    }


    //show the current selected Unit's cost
    private void UpdateActionPoints() 
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints(); 
    }
}
