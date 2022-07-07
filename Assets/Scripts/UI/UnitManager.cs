using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }



    [SerializeField] private List<Unit> unitList;

    [SerializeField]private List<Unit> friendlyUnitList;

    [SerializeField] private List<Unit> enemyUnitList;

    

    private void Awake()
    {
        #region Singleton Pattern
        if (Instance != null)
        {
            Debug.Log("There's more than one UnitManager " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion

        //innitialzing all the lists
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        //subscribe to the spawn and death of unit
        Unit.OnAnyUnitSpawed += Unit_OnAnyUnitSpawed;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;


    }

    private void OnDisable()
    {
        Unit.OnAnyUnitSpawed -= Unit_OnAnyUnitSpawed;
        Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
    }

  

    private void Unit_OnAnyUnitSpawed(object sender,EventArgs e) 
    {
        //all the sender of this event are unit
        Unit unit = sender as Unit;

        unitList.Add(unit);

        if (unit.IsEnemy()) 
        {
            enemyUnitList.Add(unit);
        }
        else 
        {

            friendlyUnitList.Add(unit);
        }
    
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {

            friendlyUnitList.Remove(unit);
        }

    }



    //getter for all the list

    public List<Unit> GetUnitList() 
    {
        return unitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }


}
