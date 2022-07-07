using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource slashFx;
    [SerializeField] AudioSource shotFx;
    [SerializeField] AudioSource grenadeFx;

    void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        
    }

    private void OnDisable()
    {
        ShootAction.OnAnyShoot -= ShootAction_OnAnyShoot;
        SwordAction.OnAnySwordHit -= SwordAction_OnAnySwordHit;
        GrenadeProjectile.OnAnyGrenadeExploded -= GrenadeProjectile_OnAnyGrenadeExploded;
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        grenadeFx.Play();
    }

    private void SwordAction_OnAnySwordHit(object sender, EventArgs e)
    {
        slashFx.Play();
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        shotFx.Play();
    }
}
