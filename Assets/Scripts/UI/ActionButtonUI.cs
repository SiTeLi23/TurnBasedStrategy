using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;


    //store current base action
    private BaseAction baseAction;




    //assigng action function to this button
    public void SetBaseAction(BaseAction baseAction) 
    {
        //set up current base Action
        this.baseAction = baseAction;

        textMeshPro.text = baseAction.GetActionName().ToUpper();

        //listener for anoymousFunction/lambda experission
        button.onClick.AddListener(() =>
        {
            //each button will know what which action it has been assigned with
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
        
        
       
    }

    public void UpdateSelectedVisual() 
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();

        //set the selected visual effect to show only when selected action is this base action, so other base action won't show up
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    
    }


}
