using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Gamepad gamepad;
    PlayerAnimation playerAnimation;

    [Header("プレイヤ情報")]
    [SerializeField] GameObject startPos; //キャラの初期位置
    Vector3 moveDirection;
    [SerializeField] float walkSpeed; 　  //歩行時のキャラの移動スピード
    [SerializeField] float runSpeed; 　   //ダッシュ時のキャラの移動スピード
    private float speed;                  //移動スピード
    public float charaDir = 0.0f;   　    //キャラの位置
    float horizontal;                　   //X軸
    float vertical;                　　   //Z軸

    [SerializeField] LayerMask layer;
    [SerializeField] GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos.transform.position;
        playerAnimation = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        horizontal = gamepad.leftStick.x.ReadValue(); //X軸の動き
        vertical = gamepad.leftStick.y.ReadValue();   //Z軸の動き

        //スティックがニュートラルでないなら
        if (vertical != 0.0f || horizontal != 0.0f)
        {
            if (gamepad.rightShoulder.isPressed && RunFlag(-0.7f,0.7f,-0.7f,0.7f))
            {
                playerAnimation.run = true;
                playerAnimation.walk = false;
                speed = runSpeed;
            }
            else
            {
                playerAnimation.walk = true;
                playerAnimation.run = false;
                speed = walkSpeed;
            }
        }
        else
        {
            playerAnimation.run = false;
            playerAnimation.walk = false;
        }

        PlayerForward();

        rb.AddForce(moveDirection);
        
        //特定のコマンドでスタート地点、または中間地点に戻る
        if (gamepad.startButton.wasPressedThisFrame)
        {
            transform.position = startPos.transform.position;
        }
        
    }
    //プレイヤの向きをカメラの方向に合わせる
    void PlayerForward()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * vertical + Camera.main.transform.right * horizontal;

        // 移動方向にスピードを掛ける
        moveDirection = moveForward * speed;

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }
    
    //スティック入力の大きさでダッシュするか判定
    bool RunFlag(float Xlow,float Xhigh,float Ylow,float Yhigh)
    {
        return Xlow >= horizontal || horizontal >= Xhigh || Ylow >= vertical || vertical >= Yhigh;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sea")
        {
            transform.position = startPos.transform.position;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "MoveBlock")
        {
            gameObject = collision.gameObject.transform.parent.gameObject;
            transform.SetParent(gameObject.transform);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "MoveBlock")
        {
            gameObject = null;
            transform.SetParent(null);
        }
    }
}
