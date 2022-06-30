using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private void Start()
    {
        //listen to the event
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

        CreateUnitActionButtons();
    }



    private void CreateUnitActionButtons() 
    {
        //clear all button everytime before we update button UI
        foreach(Transform buttonTransform in actionButtonContainerTransform) 
        {
            Destroy(buttonTransform.gameObject);
        }


        Unit selectedUnit =  UnitActionSystem.Instance.GetSelectedUnit();

        //get how many action this unit have , and instantiate button for each
        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray()) 
        {
            //instantiate a button
           Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);

            //get current action and  pass current action function into the button
           ActionButtonUI actionButtonUI= actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        }
       
    
    }


    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) 
    {

        CreateUnitActionButtons();
    
    }
}
