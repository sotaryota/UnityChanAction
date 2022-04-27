using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    Rigidbody rb;
    Gamepad gamepad;
    MainUIManager ui;

    [Header("Ground Check Sphere")]
    public LayerMask groundMask;             //地面レイヤー
    [SerializeField] float playerThicness;   //スフィアの半径

    [Header("Jump")]
    [SerializeField] float jumpPow;                    //ジャンプ力
    [SerializeField] bool isJump;            //ジャンプできるかどうか
    public bool isGround;                    //地面との接触判定

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    private void Update()
    {
        if(gamepad == null)
        gamepad = Gamepad.current;

        isGround = Physics.CheckSphere(transform.position, playerThicness, groundMask);

        if (isGround)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                isJump = true;
            }
        }
        else
        {
            isJump = false;
        }

    }
    private void FixedUpdate()
    {
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        //地面レイヤーに接地しているか
        //ジャンプボタンが押されているか
        if(isJump)
        {
            rb.AddForce(jumpPow * Vector3.up, ForceMode.Impulse);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, playerThicness);
    }
}
