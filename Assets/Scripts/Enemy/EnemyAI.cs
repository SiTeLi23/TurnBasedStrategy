using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,

    }

    private State state;

    private float timer;


    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }




    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        //if its player turn, do nothing
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer < 0f)
                {
                    
                    //once enemy finish current action, it will call the SetStateTakingTurn Function
                    if (TryTakeEnemyAIAction(SetStateTakingTurn)) 
                    {
                        //if there's enemy can take action
                        state = State.Busy;
                    }
                    else 
                    {
                        // if no enemy can take action
                        TurnSystem.Instance.NextTurn();
                    
                    }

                }
                break;
            case State.Busy:
                break;


        }




    }

    private void SetStateTakingTurn()
    {

        timer = 0.5f;
        state = State.TakingTurn;
    }


    private void TurnSystem_OnTurnChanged(object send, EventArgs e)
    {
        //if it's enemy turn
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;

        }


    }



    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            //if we can take enemy ai action return true
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }

        }
        return false;

    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
         SpinAction spinAction = enemyUnit.GetSpinAction();

       
        GridPosition actionGridPosition = enemyUnit.GetGridPosition();

        //test if its valid grid position
        if (!spinAction.IsValidActionGridPosition(actionGridPosition)) return false;

        //if this unit can't afford the selection action cost, do nothing
        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)) return false;

    
        spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        return true;

    }
}