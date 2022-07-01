using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnSystem : MonoBehaviour
{
   
    public static TurnSystem Instance { get; private set; }


    public event EventHandler OnTurnChanged;

     private int turnNumber = 1;


    private void Awake()
    {
        #region Singleton Pattern
        if (Instance != null)
        {
            Debug.Log("There's more than one TurnSystem " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion

    }

    public void NextTurn() 
    {

        turnNumber++;
        
        //fire an Turn changed event
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }


    //getter for current turn number
    public int GetTurnNumber() 
    {
        return turnNumber;
    }
}
