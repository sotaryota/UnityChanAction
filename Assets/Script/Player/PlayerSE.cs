using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSE : MonoBehaviour
{
    PlayerMove playermove;
    PlayerJump playerJump;
    [SerializeField] GoalManager goal;
    
    AudioSource audio;
    [SerializeField] AudioClip walkSE;
    [SerializeField] float runWait;
    [SerializeField] float walkWait;

    bool IsWalkSE;
    bool IsRunSE;

    // Start is called before the first frame update
    void Start()
    {
        playermove      = GetComponent<PlayerMove>();
        playerJump      = GetComponent<PlayerJump>();
        audio           = GetComponent<AudioSource>();
        goal            = goal.GetComponent<GoalManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //ゴールしていたらSEを止める
        if (goal.goalFlag)
        {
            audio.Stop();
            return;
        }

        MoveSE();
    }

    //-----------------------------------------------------
    //歩行と走行のSEの切り替え
    //-----------------------------------------------------

    void MoveSE()
    {
        if (playermove.isWalk)
        {
            if (playerJump.IsGround())
            {
                if (IsWalkSE)
                {
                    return;
                }
                StartCoroutine("WalkSE");
            }
        }
        if (playermove.isRun && playerJump.IsGround())
        {
            if (IsRunSE)
            {
                return;
            }
            StartCoroutine("RunSE");
        }
    }

    //-----------------------------------------------------
    //以下は歩行音を重複させないための処理
    //-----------------------------------------------------

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
