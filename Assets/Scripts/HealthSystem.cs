using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{


    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

   [SerializeField] private int health = 100;
    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }


    public void Damage(int damageAmount) 
    {

        health -= damageAmount;

        if (health < 0) 
        {
            health = 0;
        }
        //whenever take damage , fire the event
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if(health == 0)
        {
            Die();
        
        }


        Debug.Log(health);

    }



    private void Die() 
    {
        //fire death event
        OnDead?.Invoke(this, EventArgs.Empty);
    }


    public float GetHealthNormalized() 
    {
        return  (float)health / healthMax;
    
    }

}
