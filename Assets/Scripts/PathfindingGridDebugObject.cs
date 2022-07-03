using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;

    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
        
    }


    protected override void Update()
    {
        base.Update();

        gCostText.text = pathNode.GetGcost().ToString();
        hCostText.text = pathNode.GetHcost().ToString();
        fCostText.text = pathNode.GetFcost().ToString();
        isWalkableSpriteRenderer.color = pathNode.IsWalkable()? Color.green : Color.red;
    }
}
