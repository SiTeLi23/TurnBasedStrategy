using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;

    //assigng action function to this button
    public void SetBaseAction(BaseAction baseAction) 
    {
        textMeshPro.text = baseAction.GetActionName().ToUpper();
       
    }

}
