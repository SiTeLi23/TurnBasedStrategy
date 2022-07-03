using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;

    //generic objct
    private object gridObject;

    //constructor
    public virtual void SetGridObject(object gridObject) 
    {

        this.gridObject = gridObject;
    }

    protected virtual void Update()
    {
        //cause we override the tostring() within the gridObject to just show the grid position text, so we can just call the tostring()directly
        textMeshPro.text = gridObject.ToString();
    }
}
