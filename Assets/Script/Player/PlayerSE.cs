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
        //�S�[�����Ă�����SE���~�߂�
        if (goal.goalFlag)
        {
            audio.Stop();
            return;
        }

        MoveSE();
    }

    //-----------------------------------------------------
    //���s�Ƒ��s��SE�̐؂�ւ�
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
    //�ȉ��͕��s�����d�������Ȃ����߂̏���
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
