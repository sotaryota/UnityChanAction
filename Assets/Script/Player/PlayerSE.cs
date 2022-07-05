using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSE : MonoBehaviour
{
    PlayerMove playermove;
    PlayerJump playerJump;
    GoalProcess goal;
    
    AudioSource audio;
    [SerializeField] AudioClip walkSE;
    [SerializeField] float runWait;
    [SerializeField] float walkWait;

    bool IsWalkSE;
    bool IsRunSE;

    // Start is called before the first frame update
    void Start()
    {
        playermove = GetComponent<PlayerMove>();
        playerJump      = GetComponent<PlayerJump>();
        goal            = GameObject.Find("GoalManager").GetComponent<GoalProcess>();
        audio           = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playermove.isWalk)
        {
            if(playerJump.IsGround())
            {
                if (IsWalkSE)
                {
                    return;
                }
                StartCoroutine("WalkSE");
            }
        }
        if(playermove.isRun && playerJump.IsGround())
        {
            if(IsRunSE)
            {
                return;
            }
            StartCoroutine("RunSE");
        }
        if(goal.goalFlag)
        {
            audio.Stop();
        }
    }
    IEnumerator WalkSE()
    {
        IsWalkSE = true;

        audio.PlayOneShot(walkSE);

        yield return new WaitForSeconds(walkWait);

        audio.Stop();

        IsWalkSE = false;
    }
    IEnumerator RunSE()
    {
        IsRunSE = true;

        audio.PlayOneShot(walkSE);

        yield return new WaitForSeconds(runWait);

        audio.Stop();

        IsRunSE = false;
    }
}
