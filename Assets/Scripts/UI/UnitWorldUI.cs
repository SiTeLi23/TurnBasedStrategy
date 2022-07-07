using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;


    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamage;
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void OnDisable()
    {
        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged -= HealthSystem_OnDamage;
    }

    private void UpdateActionPointsText() 
    {

        actionPointsText.text = unit.GetActionPoints().ToString();
    }



    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e) 
    {
        UpdateActionPointsText();
    
    }

    private void HealthSystem_OnDamage(object sender, EventArgs e) 
    {
        UpdateHealthBar();
    
    }


    private void UpdateHealthBar() 
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
