using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Gamepad gamepad;
    PlayerAnimation playerAnimation;

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
    public bool goalFlag = false;

    //�������i���Ɉ���������Ȃ�����
    //------------------------------------------------------------------------------------------------
    //Vector3 velocity;
    //[SerializeField] private Transform stepRay;             //�i��������ׂ̃��C���΂��ʒu
    //[SerializeField] private float stepDistance = 0.5f;     //���C���΂�����
    //[SerializeField] private float stepOffset = 0.3f;       //�����i��
    //[SerializeField] private float slopeLimit = 65f;        //�����p�x
    //[SerializeField] private float slopeDistance = 1f;      //�����i���̈ʒu�����΂����C�̋���
    //------------------------------------------------------------------------------------------------

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
        if (goalFlag)
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
    //�v���C���̌������J�����̕����ɍ��킹�� 
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
    //�������i���Ɉ���������Ȃ�����
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //private void StairsCheck()
    //{
    //    Debug.DrawLine(transform.position + new Vector3(0f, stepOffset, 0f), transform.position + new Vector3(0f, stepOffset, 0f) + transform.forward * stepOffset, Color.green);

    //    //�X�e�b�v�p�̃��C���n�ʂɐڐG���Ă��邩�ǂ���
    //    if (Physics.Linecast(stepRay.position, stepRay.position + stepRay.forward * stepDistance, out var stepHit, LayerMask.GetMask("Ground")))
    //    {
    //        //�i�s�����̒n�ʂ̊p�x���w��ȉ��A�܂��͏����i����艺�������ꍇ�̈ړ�����
    //        if(!Physics.Linecast(transform.position + new Vector3(0f, stepOffset, 0f), transform.position + new Vector3(0f, stepOffset, 0f) + transform.forward * slopeDistance, LayerMask.GetMask("Ground")))
    //        {
    //            velocity = new Vector3(0f, (Quaternion.FromToRotation(Vector3.up, stepHit.normal) * transform.forward * 1.5f).y, 0f) + transform.forward * 1.5f;

    //        }
    //    }
    //}
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    //�X�e�B�b�N���͂̑傫���Ń_�b�V�����邩����
    bool RunFlag(float Xlow, float Xhigh, float Ylow, float Yhigh)
    {
        return Xlow >= horizontal || horizontal >= Xhigh || Ylow >= vertical || vertical >= Yhigh;
    }

    //�I�u�W�F�N�g�Ƃ̐ڐG����
    //----------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "Goal")
        {
            goalFlag = true;
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
    //----------------------------------------------------------------------------
}