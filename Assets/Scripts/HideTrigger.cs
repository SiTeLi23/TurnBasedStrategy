using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTrigger : MonoBehaviour
{
    public List<GameObject> Hides;
    public List<Unit> Enemies;

    private void Awake()
    {
         foreach(GameObject hide in Hides) 
        {

            hide.SetActive(true);
        }

        foreach (Unit enemy in Enemies)
        {

            enemy.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Unit>() == null) return;


        if (other.GetComponentInParent<Unit>().IsEnemy() == false) 
        {
            ShowNextHide();
            gameObject.SetActive(false);
        }
        

        
    }

    private void OnTriggerStay(Collider other)
    {
       
    }



    public void ShowNextHide() 
    {
        foreach (GameObject hide in Hides)
        {

            hide.SetActive(false);
        }

        foreach (Unit enemy in Enemies)
        {

            enemy.gameObject.SetActive(true);
        }

    }
}
