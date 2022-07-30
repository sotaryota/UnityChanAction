using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float hp_;         //HP
    [SerializeField] private float maxHp_;      //�ő�HP
    [SerializeField] private float walkSpeed_;  //���s�X�s�[�h
    [SerializeField] private float RunSpeed_;   //���s�X�s�[�h
    [SerializeField] private float jumpPow_;    //�W�����v��

    public float getHp()
    {
        return hp_;
    }

    public float getMaxHp()
    {
        return maxHp_;
    }

    public float getWalkSpeed()
    {
        return walkSpeed_;
    } 

    public float getRunSpeed()
    {
        return RunSpeed_;
    }

    public float getJumpPow()
    {
        return jumpPow_;
    }

    public void Damage(float damage_)
    {
        hp_ -= damage_;
    }
    public void Health(float health_)
    {
        hp_ += health_;
    }

}
