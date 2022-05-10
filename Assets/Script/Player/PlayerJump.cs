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
    MainUIManager ui;

    [Header("Ground Check Sphere")]
    public LayerMask groundMask;             //�n�ʃ��C���[
    [SerializeField] float playerThicness;   //�X�t�B�A�̔��a
    public bool isGround;                    //�n�ʂƂ̐ڐG����

    [Header("Jump")]
    [SerializeField] float jumpPow;          //�W�����v��
    [SerializeField] bool isJump;            //�W�����v�{�^����������Ă��邩

    // Start is called before the first frame update
    void Start()
    {
        rb              = GetComponent<Rigidbody>();
        playerMove      = GetComponent<PlayerMove>();
        playerAnimation = GetComponent<PlayerAnimation>();
        ui              = GameObject.Find("UIManager").GetComponent<MainUIManager>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (ui.pause)
            return;

        if (playerMove.goalFlag)
        {
            playerAnimation.fall = false;
            return;
        }

        if(gamepad == null)
        gamepad = Gamepad.current;

        IsGround();
    }
    private void FixedUpdate()
    {
        //�W�����v�{�^����������Ă��邩
        if(isJump)
        {
            rb.AddForce(jumpPow * Vector3.up, ForceMode.Impulse);
            playerAnimation.fall = true;
        }
    }
    //////////////////////////////////

    //�ڒn����

    //////////////////////////////////
    void IsGround()
    {
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
}
