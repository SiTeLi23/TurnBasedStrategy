using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVfxPrefab;

    private Vector3 targetPosition;


   public void SetUp(Vector3 targetPosition) 
    {
        this.targetPosition = targetPosition;
    
    }
    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        float moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving) 
        {
            //make sure bullet not go through target
            transform.position = targetPosition;

            //unpack the trail render before destroying all projectil object , just to make the trail render disapear more smoothly after destroy
            trailRenderer.transform.parent = null;
            Destroy(gameObject);

            //hit effect
            Instantiate(bulletHitVfxPrefab, targetPosition, Quaternion.identity);
        }
    }

    


}
