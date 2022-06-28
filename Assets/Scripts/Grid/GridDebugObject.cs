using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    private GridObject gridObject;

    //constructor
    public void SetGridObject(GridObject gridObject) 
    {

        this.gridObject = gridObject;
    }

    private void Update()
    {
        //cause we override the tostring() within the gridObject to just show the grid position text, so we can just call the tostring()directly
        textMeshPro.text = gridObject.ToString();
    }
}
