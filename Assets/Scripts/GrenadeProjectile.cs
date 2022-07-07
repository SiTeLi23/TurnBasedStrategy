using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrenadeProjectile : MonoBehaviour
{

    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Transform grenadeExplodeVfxPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYanimationCurve;

    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;
    private float totalDistance;
    private Vector3 positionXZ;


    private void Update()
    {
        //we only work with x,z vector
        Vector3 moveDir = (targetPosition - positionXZ).normalized;
        float moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        //get the current distance
        float distance = Vector3.Distance(positionXZ, targetPosition);
        //calculate distance normalize
        float distanceNormalized = 1- distance / totalDistance;
        float maxHeight = totalDistance/ 4f;
        //we let animation arc to handle with y
       float positionY = arcYanimationCurve.Evaluate(distanceNormalized)*maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        //if hit target , do an AOE damage
        float reachedTargetDistance = .2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance) 
        {
            float damageRadius = 4f;
            //return an array stored all targets in the area
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach(Collider collider in colliderArray) 
            {
               if( collider.TryGetComponent<Unit>(out Unit targetUnit)) 
                {

                    targetUnit.Damage(35);
                   
                }

                if (collider.TryGetComponent<DestructableCrate>(out DestructableCrate destructableCrate))
                {

                    destructableCrate.Damage();

                }

            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            trailRenderer.transform.parent = null;

            Instantiate(grenadeExplodeVfxPrefab, targetPosition + Vector3.up*1f, Quaternion.identity);
            Destroy(gameObject);

            onGrenadeBehaviourComplete();
        
        }

    }


    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete) 
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);


        positionXZ = transform.position;
        positionXZ.y = 0;

        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }



}
