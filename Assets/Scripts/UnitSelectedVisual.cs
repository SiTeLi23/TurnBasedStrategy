using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UpdateVisual();

        //set up listener, so whenever an event has been sent, it will automatically called this function
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }


    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty) 
    {
        UpdateVisual();
    }


    private void UpdateVisual() 
    {
      //if this unit is the current selected unit, show the selected visual, otherwise not
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else 
        {

            meshRenderer.enabled = false;
        }
    
    }

    private void OnDestroy()
    {
        //unsubscribe the event after current unit dead and being destroyed
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }

}
