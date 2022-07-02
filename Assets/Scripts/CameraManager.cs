using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;


        HideActionCamera();
    }

    public void ShowActionCamera() 
    {
        actionCameraGameObject.SetActive(true);
    }

    public void HideActionCamera() 
    {

        actionCameraGameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e) 
    {
        switch (sender) 
        {
            //make sure the  action camera appear at the shoulder height and right offset position

            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit =  shootAction.GetTargetUnit();

                //the shoulder position height
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
                
                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;


                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

                //feet of the shooter unit + camera heigh + slight to the right side + camera push back a bit to see the shooter
                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);
                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                ShowActionCamera();
                break;
        }
    
    }

    private void BaseAction_OnAnyActionCompleted(object sender,EventArgs e) 
    {
        switch (sender)
        {

            case ShootAction shootAction:
                HideActionCamera();
                break;
        }

    }


}