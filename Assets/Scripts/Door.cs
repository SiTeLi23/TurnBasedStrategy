using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour , IInteractable
{
   [SerializeField] private bool isOpen;
   public  GameObject hide;
    public List<Unit> enemies;
    private GridPosition gridPosition;
    private Animator animator;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;
    [SerializeField] AudioSource doorOpenSound;

    private void Awake()
    {
        animator = GetComponent<Animator>();


        hide.SetActive(true);
        foreach (Unit enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }


    private void Start()
    {
        //get the grid position and set door within that grid position
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
       
        
           
        
        if (isOpen) 
        {
            OpenDoor();
        }

        else 
        {
            CloseDoor();
        }
        
    }


    private void Update()
    {
        if (!isActive) return;


        timer -= Time.deltaTime;

        if(timer <= 0) 
        {
            isActive = false;
            onInteractionComplete();
        }
    }


    public void Interact(Action onInteractionComplete) 
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 0.5f;

        if (isOpen)
        {
            CloseDoor();
        }

        else
        {
            if (doorOpenSound != null)
            {
              doorOpenSound.Play();
            }
            
            OpenDoor();
        }
    }


    private void OpenDoor() 
    {
        isOpen = true;
        animator.SetBool("IsOpen", true);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
        
        if (hide != null) 
        {
            hide.SetActive(false);
        }

        if (enemies.Count > 0)
        {
            foreach (Unit enemy in enemies)
            {

                enemy.gameObject.SetActive(true);
            }
        }
    }

    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", false);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }

}
