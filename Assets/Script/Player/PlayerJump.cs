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
    public LayerMask groundMask;             //地面レイヤー
    [SerializeField] float playerThicness;   //スフィアの半径
    public bool isGround;                    //地面との接触判定

    [Header("Jump")]
    [SerializeField] float jumpPow;          //ジャンプ力
    [SerializeField] bool isJump;            //ジャンプできるかどうか

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

        //ジャンプボタンが押されているか
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
