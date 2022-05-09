using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSE : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    PlayerJump playerJump;
    PlayerMove playerMove;
    
    AudioSource audio;
    [SerializeField] AudioClip walkSE;
    [SerializeField] float runWait;
    [SerializeField] float walkWait;

    bool IsWalkSE;
    bool IsRunSE;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerJump      = GetComponent<PlayerJump>();
        playerMove      = GetComponent<PlayerMove>();
        audio           = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerAnimation.walk)
        {
            if(playerJump.isGround)
            {
                if (IsWalkSE)
                {
                    return;
                }
                StartCoroutine("WalkSE");
            }
        }
        if(playerAnimation.run && playerJump.isGround)
        {
            if(IsRunSE)
            {
                return;
            }
            StartCoroutine("RunSE");
        }
        if(playerMove.goalFlag)
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
