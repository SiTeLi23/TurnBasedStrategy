using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestructableCrate : MonoBehaviour
{

    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;
    private GridPosition gridPosition;


    private void Start()
    {
      gridPosition =  LevelGrid.Instance.GetGridPosition(transform.position);
    }




    //getter
    public GridPosition GetGridPosition() 
    {
        return gridPosition;
    }


    public void Damage() 
    {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);
        Destroy(gameObject);
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childrigidBody))
            {

                childrigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }




            //make sure next and other levels of childs also get that explosion force
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);

        }



    }



}
