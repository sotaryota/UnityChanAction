using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float hp_;
    [SerializeField] private float walkSpeed_;
    [SerializeField] private float RunSpeed_;
    [SerializeField] private float jumpPow_;

    public float getHp()
    {
        return hp_;
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

    public void Damage(float damage)
    {
        hp_ -= damage;
    }
    public void Health(float health)
    {
        hp_ += health;
    }

}
