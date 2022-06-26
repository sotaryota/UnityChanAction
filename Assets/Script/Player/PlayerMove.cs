using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Gamepad gamepad;
    PlayerAnimation playerAnimation;
    TutorialManager tutorialManager;
     GoalProcess goal;
    MainUIManager ui;

    private Vector3 cameraForward;        //�J�����̕���
    private Vector3 moveForward;          //�v���C���̕���

    [Header("�v���C�����")]
    [SerializeField] GameObject startPos; //�L�����̏����ʒu
    [SerializeField] GameObject onBlock;  //�v���C��������Ă���u���b�N
    private Vector3 moveDirection;        //�v���C���̈ړ���
    [SerializeField] float walkSpeed; �@  //���s���̈ړ��X�s�[�h
    [SerializeField] float runSpeed; �@   //���s���̈ړ��X�s�[�h
    private float speed;                  //���݂̈ړ��X�s�[�h
    private float horizontal;             //L�X�e�B�b�NX��
    private float vertical;               //L�X�e�B�b�NY��

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos.transform.position;
        playerAnimation    = GetComponent<PlayerAnimation>();
        rb                 = GetComponent<Rigidbody>();
        ui                 = GameObject.Find("UIManager").GetComponent<MainUIManager>();
        tutorialManager    = GameObject.Find("UIManager").GetComponent<TutorialManager>();
        goal               = GameObject.Find("GoalManager").GetComponent<GoalProcess>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (goal.goalFlag)
        {
            playerAnimation.walk = false;
            playerAnimation.run  = false;
            return;
        }

        gamepad = Gamepad.current;
        if (gamepad == null) return;

        horizontal = gamepad.leftStick.x.ReadValue();
        vertical = gamepad.leftStick.y.ReadValue();

        //�X�e�B�b�N���j���[�g�����łȂ��Ȃ�
        if (vertical != 0.0f || horizontal != 0.0f)
        {
            if (gamepad.rightShoulder.isPressed && RunFlag(-0.7f, 0.7f, -0.7f, 0.7f))
            { //����
                playerAnimation.run = true;
                playerAnimation.walk = false;
                speed = runSpeed;
            }
            else
            { //����
                playerAnimation.walk = true;
                playerAnimation.run = false;
                speed = walkSpeed;
            }

            //StairsCheck();
        }
        else
        {
            playerAnimation.run = false;
            playerAnimation.walk = false;
        }

        PlayerForward();

        rb.AddForce(moveDirection);
    }
    //////////////////////////////////

    //�v���C���̌������J�����̕����ɍ��킹�鏈��

    //////////////////////////////////
    void PlayerForward()
    {
        // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        moveForward = cameraForward * vertical + Camera.main.transform.right * horizontal;

        // �ړ������ɃX�s�[�h���|����
        moveDirection = moveForward * speed;

        // �L�����N�^�[�̌�����i�s������
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward); ;
        }
    }
    //////////////////////////////////

    //�X�e�B�b�N���͂̑傫���Ń_�b�V�����邩����

    //////////////////////////////////
    bool RunFlag(float Xlow, float Xhigh, float Ylow, float Yhigh)
    {
        return Xlow >= horizontal || horizontal >= Xhigh || Ylow >= vertical || vertical >= Yhigh;
    }
    //////////////////////////////////

    //�I�u�W�F�N�g�Ƃ̐ڐG����

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