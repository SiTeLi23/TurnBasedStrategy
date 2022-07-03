using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrenadeProjectile : MonoBehaviour
{

    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;


    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float moveSpeed = 15f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;


        //if hit target , do an AOE damage
        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance) 
        {
            float damageRadius = 4f;
            //return an array stored all targets in the area
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach(Collider collider in colliderArray) 
            {
               if( collider.TryGetComponent<Unit>(out Unit targetUnit)) 
                {

                    targetUnit.Damage(30);
                }
            
            }

            Destroy(gameObject);

            onGrenadeBehaviourComplete();
        
        }

    }


    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete) 
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }



}