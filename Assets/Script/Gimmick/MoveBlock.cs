using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    [Header("Block Property")]
    [SerializeField] Vector3 pos;           //�I�u�W�F�N�g�̔z�u�ꏊ
    [SerializeField] Vector3 scale;         //�I�u�W�F�N�g�̑傫��
    [SerializeField] private float lengthX; //X���̓����̑傫��
    [SerializeField] private float lengthY; //Y���̓����̑傫��
    [SerializeField] private float lengthZ; //Z���̓����̑傫��
    float valueX;
    float valueY;
    float valueZ;

    private void Start()
    {
        transform.Find("Block").gameObject.transform.position = pos;
        transform.Find("Block").gameObject.transform.localScale = scale;
    }
    private void Update()
    {
        valueX = Mathf.PingPong(Time.time, lengthX);�@//X���̓���
        valueY = Mathf.PingPong(Time.time, lengthY);  //Y���̓���
        valueZ = Mathf.PingPong(Time.time, lengthZ);  //Z���̓���

        MoveBlockDirection();
    }
    private void MoveBlockDirection()
    {
        if (lengthX != 0 && lengthY != 0)        //X�EY��
        {
            transform.localPosition = new Vector3(pos.x + valueX, pos.y + valueY, pos.z);
        }
        else if (lengthY != 0 && lengthZ != 0) �@//Y�EZ��
        {
            transform.localPosition = new Vector3(pos.x, pos.y + valueY, pos.z + valueZ);
        }
        else if (lengthX != 0 && lengthZ != 0) �@//X�EZ��
        {
            transform.localPosition = new Vector3(pos.x + valueX, pos.y, pos.z + valueZ);
        }
        else if (lengthX != 0)                   //X�� 
        {
            transform.localPosition = new Vector3(pos.x + valueX, pos.y, pos.z);
        }
        else if (lengthY != 0)                   //Y��
        {
            transform.localPosition = new Vector3(pos.x, pos.y + valueY, pos.z);
        }
        else if (lengthZ != 0)                   //Z��
        {
            transform.localPosition = new Vector3(pos.x, pos.y, pos.z + valueZ);
        }
    }
}