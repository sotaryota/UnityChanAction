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
    public LayerMask groundMask;             //�n�ʃ��C���[
    [SerializeField] float playerThicness;   //�X�t�B�A�̔��a

    [Header("Jump")]
    [SerializeField] float jumpPow;                    //�W�����v��
    [SerializeField] bool isJump;            //�W�����v�ł��邩�ǂ���
    public bool isGround;                    //�n�ʂƂ̐ڐG����

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

        //�n�ʃ��C���[�ɐڒn���Ă��邩
        //�W�����v�{�^����������Ă��邩
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
