using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Gamepad gamepad;

    [Header("�X�N���v�g")]
    PlayerStatus status; 
    [SerializeField] GoalManager goal;

    private Vector3 cameraForward;        //�J�����̕���
    private Vector3 moveForward;          //�v���C���̕���

    [Header("�v���C�����")]
    private Vector3 moveDirection;        //�v���C���̈ړ���
    private float nowSpeed;               //���݂̈ړ��X�s�[�h
    private float horizontal;             //L�X�e�B�b�NX��
    private float vertical;               //L�X�e�B�b�NY��
    public bool isWalk;
    public bool isRun;

    // Start is called before the first frame update
    void Start()
    {
        status             = GetComponent<PlayerStatus>();
        rb                 = GetComponent<Rigidbody>();
        goal               = goal.GetComponent<GoalManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�S�[�����Ă�����return
        if (goal.goalFlag) { return; }

        if (gamepad == null)
            gamepad = Gamepad.current;

        PlayerWalkANDRun();
        PlayerForward();
    }

    //-----------------------------------------------------
    //�v���C���̈ړ�����
    //-----------------------------------------------------

    void PlayerWalkANDRun()
    {
        horizontal = gamepad.leftStick.x.ReadValue();
        vertical = gamepad.leftStick.y.ReadValue();

        //�X�e�B�b�N���j���[�g�����łȂ��Ȃ�
        if (vertical != 0.0f || horizontal != 0.0f)
        {
            if (gamepad.rightShoulder.isPressed && RunFlag(-0.7f, 0.7f, -0.7f, 0.7f))
            { //����
                nowSpeed = status.getRunSpeed();
                isWalk = false;
                isRun = true;
            }
            else
            { //����
                nowSpeed = status.getWalkSpeed();
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

    //-----------------------------------------------------
    //�v���C���̌������J�����̕����ɍ��킹�鏈��
    //-----------------------------------------------------

    void PlayerForward()
    {
        //�J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //�����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        moveForward = cameraForward * vertical + Camera.main.transform.right * horizontal;

        //�ړ������ɃX�s�[�h���|����
        moveDirection = moveForward * nowSpeed;

        //�L�����N�^�[�̌�����i�s������
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward); ;
        }

        //�ړ�������
        rb.AddForce(moveDirection);
    }

    //-----------------------------------------------------
    //�X�e�B�b�N���͂̑傫���Ń_�b�V�����邩����
    //-----------------------------------------------------

    bool RunFlag(float Xlow, float Xhigh, float Ylow, float Yhigh)
    {
        return Xlow >= horizontal || horizontal >= Xhigh || Ylow >= vertical || vertical >= Yhigh;
    }
}