using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathfindUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructableCrate.OnAnyDestroyed += DestructableCrate_OnAnyDestroyed;
    }
    private void OnDisable()
    {
        DestructableCrate.OnAnyDestroyed -= DestructableCrate_OnAnyDestroyed;
    }

    private void DestructableCrate_OnAnyDestroyed(object sender, EventArgs e)
    {

        DestructableCrate destructableCrate = sender as DestructableCrate;

        Pathfinding.Instance.SetIsWalkableGridPosition(destructableCrate.GetGridPosition(), true);
    }
}
