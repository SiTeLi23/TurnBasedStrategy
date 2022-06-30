using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


//adding keyword abstract will not let other scripts to create an instance of this specific class,
//just to make sure not accidently creat and instance of this class
public abstract class BaseAction : MonoBehaviour
{


    //protected can prevent other class change this value, but class that extend this class will be able to get access to those data
    protected Unit unit;
    protected bool isActive;

    //a field for delegate, store the type of function reference we received
    protected Action onActionComplete;


    //using virtual so child class can override such method
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    //abstract mean force other child script to instantiate this function
    public abstract string GetActionName();




}
