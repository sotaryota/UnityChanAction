using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    Rigidbody rb;
    Gamepad gamepad;
    PlayerAnimation playerAnimation;
    [SerializeField] GoalManager goal;
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] MainUIManager ui;

    [Header("Ground Check Sphere")]
    public LayerMask groundMask;             //地面レイヤー
    [SerializeField] float playerThicness;   //スフィアの半径
    [SerializeField] bool isGround;

    [Header("Jump")]
    [SerializeField] float jumpPow;          //ジャンプ力
    [SerializeField] bool isJump;            //ジャンプボタンが押されているか

    // Start is called before the first frame update
    void Start()
    {
        rb              = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        ui              = ui.GetComponent<MainUIManager>();
        goal            = goal.GetComponent<GoalManager>();
        tutorialManager = tutorialManager.GetComponent<TutorialManager>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (ui.pause || tutorialManager.tutorialFlag || goal.goalFlag) { return; }

        if (gamepad == null)
            gamepad = Gamepad.current;

        IsJump();
    }

    //////////////////////////////////

    //接地判定

    //////////////////////////////////
    
    public bool IsGround()
    {
        return Physics.CheckSphere(transform.position, playerThicness, groundMask);
    }

    //////////////////////////////////

    //ジャンプする

    //////////////////////////////////
   
    void IsJump()
    {
        if (IsGround())
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                rb.AddForce(jumpPow * Vector3.up, ForceMode.Impulse);
            }
        }
    }
}
