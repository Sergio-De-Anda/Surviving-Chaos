using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    Animator animator;
    
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Animate(playerInput.Vertical, playerInput.Horizontal);
    }

    //Animates the character and root motion handles the movement
    public void Animate(float forward, float strafe)
    {
        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }
}
