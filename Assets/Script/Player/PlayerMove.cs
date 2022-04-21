using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Gamepad gamepad;
    PlayerAnimation playerAnimation;

    [Header("�v���C�����")]
    [SerializeField] GameObject startPos; //�L�����̏����ʒu
    Vector3 moveDirection;
    [SerializeField] float walkSpeed; �@  //���s���̃L�����̈ړ��X�s�[�h
    [SerializeField] float runSpeed; �@   //�_�b�V�����̃L�����̈ړ��X�s�[�h
    private float speed;                  //�ړ��X�s�[�h
    public float charaDir = 0.0f;   �@    //�L�����̈ʒu
    float horizontal;                �@   //X��
    float vertical;                �@�@   //Z��

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

        horizontal = gamepad.leftStick.x.ReadValue(); //X���̓���
        vertical = gamepad.leftStick.y.ReadValue();   //Z���̓���

        //�X�e�B�b�N���j���[�g�����łȂ��Ȃ�
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
        
        //����̃R�}���h�ŃX�^�[�g�n�_�A�܂��͒��Ԓn�_�ɖ߂�
        if (gamepad.startButton.wasPressedThisFrame)
        {
            transform.position = startPos.transform.position;
        }
        
    }
    //�v���C���̌������J�����̕����ɍ��킹��
    void PlayerForward()
    {
        // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 moveForward = cameraForward * vertical + Camera.main.transform.right * horizontal;

        // �ړ������ɃX�s�[�h���|����
        moveDirection = moveForward * speed;

        // �L�����N�^�[�̌�����i�s������
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }
    
    //�X�e�B�b�N���͂̑傫���Ń_�b�V�����邩����
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
