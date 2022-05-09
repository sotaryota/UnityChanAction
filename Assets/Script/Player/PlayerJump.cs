using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    Rigidbody rb;
    Gamepad gamepad;
    PlayerMove playerMove;
    PlayerAnimation playerAnimation;

    [Header("Ground Check Sphere")]
    public LayerMask groundMask;             //�n�ʃ��C���[
    [SerializeField] float playerThicness;   //�X�t�B�A�̔��a
    public bool isGround;                    //�n�ʂƂ̐ڐG����

    [Header("Jump")]
    [SerializeField] float jumpPow;          //�W�����v��
    [SerializeField] bool isJump;            //�W�����v�ł��邩�ǂ���

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMove = GetComponent<PlayerMove>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (playerMove.goalFlag)
        {
            playerAnimation.fall = false;
            return;
        }

        if(gamepad == null)
        gamepad = Gamepad.current;

        isGround = Physics.CheckSphere(transform.position, playerThicness, groundMask);

        if (isGround)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                isJump = true;
            }
            playerAnimation.fall = false;
        }
        else
        {
            isJump = false;
            playerAnimation.fall = true;
        }

    }
    private void FixedUpdate()
    {
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        //�W�����v�{�^����������Ă��邩
        if(isJump)
        {
            rb.AddForce(jumpPow * Vector3.up, ForceMode.Impulse);
            playerAnimation.fall = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, playerThicness);
    }
}
