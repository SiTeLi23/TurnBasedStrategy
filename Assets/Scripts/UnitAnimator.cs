using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;
    private void Awake()
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            //subscribe to events 
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving +=  MoveAction_OnStopMoving;
            
        }

       if(TryGetComponent<ShootAction>(out ShootAction shootAction)) 
        {
            //subscribe shoot event
            shootAction.OnShoot += ShootAction_OnShoot;
        
        }

        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += swordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += swordAction_OnSwordActionCompleted;

        }

    }


    private void Start()
    {
        EquipRifle();
    }

    private void swordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }

    private void swordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();
        animator.SetTrigger("SwordSlash");
    }

    private void MoveAction_OnStartMoving(object sender,EventArgs e) 
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void ShootAction_OnShoot(object sender,ShootAction.OnShootEventArgs e) 
    {

        animator.SetTrigger("Shoot");

      Transform bulletProjectileTransform =  Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
      BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        //grab information from our customized EventArgs and pass to bullet

        //tweak shooting height
        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = shootPointTransform.position.y;
        bulletProjectile.SetUp(targetUnitShootAtPosition);
    }


   

    public void EquipSword() 
    {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }


    public void EquipRifle() 
    {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);

    }


}
