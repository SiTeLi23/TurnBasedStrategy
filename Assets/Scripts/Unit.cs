using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]private Animator unitAnimator;
    private Vector3 targetPosition;


    private void Awake()
    {
        targetPosition = transform.position;
    }


    private void Update()
    {
       
        //make sure unit stop when reaching target position without jiggling
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {

            //get the direction toward target position and move to there
            Vector3 moveDiretion = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDiretion * moveSpeed * Time.deltaTime;

            //rotate player to where they face
            float rotateSpeed = 10f;
            transform.forward =  Vector3.Lerp(transform.forward,moveDiretion,rotateSpeed* Time.deltaTime);

            //play running animation
            unitAnimator.SetBool("IsWalking", true);
        }
        else 
        {
            //stop running animation
            unitAnimator.SetBool("IsWalking", false);

        }


      

    }



    //tell the Unit to move to target position
    public void Move(Vector3 targetPosition) 
    {
        this.targetPosition = targetPosition;
    }



}
