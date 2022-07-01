using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TurnSystemUI : MonoBehaviour
{

    [SerializeField] private Button endTurnButton;
    [SerializeField]private TextMeshProUGUI turnNumberText;

    private void Start()
    {

        //if we click the end turn button , move to the next turn
        endTurnButton.onClick.AddListener(() =>
        {

            TurnSystem.Instance.NextTurn();

        });

        //subscribe for turn end event
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

       

        updateTurnText();

    }


    private void TurnSystem_OnTurnChanged(object sender,EventArgs e) 
    {
        updateTurnText();
    
    }


    private void updateTurnText() 
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    
    }




    

}
