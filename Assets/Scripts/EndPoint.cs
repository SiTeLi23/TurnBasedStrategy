using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour, IInteractable
{
    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;
    public Animator endAnim;
    private void Start()
    {
        //get the grid position and set door within that grid position
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void OnDisable()
    {
        Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit DeadUnit = sender as Unit;
        if (DeadUnit.IsEnemy()) return;
        CheckEnd();
    }

    private void Update()
    {
        if (!isActive) return;


        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isActive = false;
            onInteractionComplete();
        }
    }
    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = .5f;
        endAnim.Play("EndFade");
        StartCoroutine("EndGame");

    }

    

    IEnumerator EndGame() 
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene("EndScene");
        yield return null;
    }


    public void CheckEnd() 
    {

        List<Unit> friendlyUnitList = UnitManager.Instance.GetFriendlyUnitList();
        if (friendlyUnitList.Count == 0)
        {
            endAnim.Play("EndFade");
            StartCoroutine("GameOver");
        }


    }


    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene("GameOver");
        yield return null;
    }
}
