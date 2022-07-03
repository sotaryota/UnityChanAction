using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;

    public bool run = false;   //走る
    public bool walk = false;  //歩く
    public bool fall = false;　//落下


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Run", run);
        animator.SetBool("Walking", walk);
        animator.SetBool("JumpFall", fall);
    }
}