using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Gamepad gamepad;
    [Header("スクリプト")]
    [SerializeField]
    PlayerAnimation playerAnimation;
    [SerializeField]
    TutorialManager tutorialManager;
    [SerializeField]
    GoalProcess goal;
    [SerializeField]
    MainUIManager ui;

    private Vector3 cameraForward;        //カメラの方向
    private Vector3 moveForward;          //プレイヤの方向

    [Header("プレイヤ情報")]
    [SerializeField]
    GameObject startPos; //キャラの初期位置
    [SerializeField]
    GameObject onBlock;  //プレイヤが乗っているブロック
    private Vector3 moveDirection;        //プレイヤの移動量
    [SerializeField]
    float walkSpeed; 　  //歩行時の移動スピード
    [SerializeField] 
    float runSpeed; 　   //走行時の移動スピード
    private float speed;                  //現在の移動スピード
    private float horizontal;             //LスティックX軸
    private float vertical;               //LスティックY軸
    public bool isWalk;
    public bool isRun;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos.transform.position;
        playerAnimation    = GetComponent<PlayerAnimation>();
        rb                 = GetComponent<Rigidbody>();
        ui                 = GetComponent<MainUIManager>();
        tutorialManager    = GetComponent<TutorialManager>();
        goal               = GetComponent<GoalProcess>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (goal.goalFlag)    
            return;
    

        gamepad = Gamepad.current;
        if (gamepad == null) return;

        PlayerWalkANDRun();
        PlayerForward();

        rb.AddForce(moveDirection);
    }

    //////////////////////////////////

    //プレイヤの移動処理

    //////////////////////////////////
    
    void PlayerWalkANDRun()
    {
        horizontal = gamepad.leftStick.x.ReadValue();
        vertical = gamepad.leftStick.y.ReadValue();

        //スティックがニュートラルでないなら
        if (vertical != 0.0f || horizontal != 0.0f)
        {
            if (gamepad.rightShoulder.isPressed && RunFlag(-0.7f, 0.7f, -0.7f, 0.7f))
            { //走る
                speed = runSpeed;
                isWalk = false;
                isRun = true;
            }
            else
            { //歩く
                speed  = walkSpeed;
                isWalk = true;
                isRun = false;
            }
        }
        else
        {
            isWalk = false;
            isRun = false;
        }
    }

    //////////////////////////////////

    //プレイヤの向きをカメラの方向に合わせる処理

    //////////////////////////////////
    
    void PlayerForward()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        moveForward = cameraForward * vertical + Camera.main.transform.right * horizontal;

        // 移動方向にスピードを掛ける
        moveDirection = moveForward * speed;

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward); ;
        }
    }

    //////////////////////////////////

    //スティック入力の大きさでダッシュするか判定

    //////////////////////////////////
   
    bool RunFlag(float Xlow, float Xhigh, float Ylow, float Yhigh)
    {
        return Xlow >= horizontal || horizontal >= Xhigh || Ylow >= vertical || vertical >= Yhigh;
    }

    //////////////////////////////////

    //オブジェクトとの接触判定

    //////////////////////////////////
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
            ui.itemScore++;
        }
        if (other.gameObject.tag == "Goal")
        {
            goal.goalFlag = true;
        }
        if (other.gameObject.tag == "Tutorial" && tutorialManager.imageNum < 3)
        {
            tutorialManager.tutorialFlag = true;
            other.gameObject.SetActive(false);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sea")
        {
            transform.position = startPos.transform.position;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "MoveBlock")
        {
            onBlock = collision.gameObject.transform.parent.gameObject;
            transform.SetParent(onBlock.transform);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "MoveBlock")
        {
            onBlock = null;
            transform.SetParent(null);
        }
    }
}