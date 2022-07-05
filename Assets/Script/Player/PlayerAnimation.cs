using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;

    PlayerMove   playerMove;
    PlayerJump   playerJump;
    GoalProcess  goal;


    // Start is called before the first frame update
    void Start()
    {
        animator    = GetComponent<Animator>();
        playerMove  = GetComponent<PlayerMove>();
        playerJump  = GetComponent<PlayerJump>();
        goal        = GameObject.Find("GoalManager").GetComponent<GoalProcess>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goal.goalFlag)
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Run", false);
            animator.SetBool("JumpFall", false);
            return;
        }
        AnimationMove();
        AnimationJumpFall();
    }

    private void AnimationMove()
    {
        if (playerMove.isWalk)
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Run", false);
        }
        else if (playerMove.isRun)
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Run", false);
        }
    
    }
    void AnimationJumpFall()
    {
        if(!playerJump.IsGround())
        {
            animator.SetBool("JumpFall", true);
        }
        else
        {
            animator.SetBool("JumpFall", false);
        }
    }
}