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

        //track the best enemy ai action
        EnemyAIAction bestEnemyAIAction = null;
        //store which on is the best action
        BaseAction bestBaseAction = null;


        //cycle through all the base action this unit can take
       foreach( BaseAction baseAction in enemyUnit.GetBaseActionArray()) 
        {
           if(!enemyUnit.CanSpendActionPointsToTakeAction(baseAction)) 
            {
                //if an action can not be afford to take at that moment,ignore it
                continue;
            }

               //get the first affordable action 
           if(bestEnemyAIAction == null) 
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
              //then compare the first stored one and replace it with potential better one later
            else 
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                //to check if the next Best AIAction can be taken and it's value is better then the previous stored one
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue) 
                {
                    //if it is , then this action is the new best action
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;

                }
            
            }

           //if enemy has enough action points to take this action, we get the most valued AI Action
            baseAction.GetBestEnemyAIAction();
        
        }


        if(bestEnemyAIAction!=null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction)) 
        {
            //if enemy have a best action and the action is affordable at that moment
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition,  onEnemyAIActionComplete);
          
            return true;
        
        
        }
        else 
        {
        
            return false;
        
        }


  

    }
}