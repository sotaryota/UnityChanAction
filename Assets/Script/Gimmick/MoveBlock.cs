using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    [Header("Block Property")]
    [SerializeField] Vector3 pos;           //オブジェクトの配置場所
    [SerializeField] Vector3 scale;         //オブジェクトの大きさ
    [SerializeField] private float lengthX; //X軸の動きの大きさ
    [SerializeField] private float lengthY; //Y軸の動きの大きさ
    [SerializeField] private float lengthZ; //Z軸の動きの大きさ
    [SerializeField] float min;
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
        valueX = Mathf.PingPong(Time.time, lengthX) / min;　//X軸の動き
        valueY = Mathf.PingPong(Time.time, lengthY) / min;  //Y軸の動き
        valueZ = Mathf.PingPong(Time.time, lengthZ) / min;  //Z軸の動き

        MoveBlockDirection();
    }
    private void MoveBlockDirection()
    {
        if (lengthX != 0 && lengthY != 0)        //X・Y軸
        {
            transform.localPosition = new Vector3(pos.x + valueX, pos.y + valueY, pos.z);
        }
        else if (lengthY != 0 && lengthZ != 0) 　//Y・Z軸
        {
            transform.localPosition = new Vector3(pos.x, pos.y + valueY, pos.z + valueZ);
        }
        else if (lengthX != 0 && lengthZ != 0) 　//X・Z軸
        {
            transform.localPosition = new Vector3(pos.x + valueX, pos.y, pos.z + valueZ);
        }
        else if (lengthX != 0)                   //X軸 
        {
            transform.localPosition = new Vector3(pos.x + valueX, pos.y, pos.z);
        }
        else if (lengthY != 0)                   //Y軸
        {
            transform.localPosition = new Vector3(pos.x, pos.y + valueY, pos.z);
        }
        else if (lengthZ != 0)                   //Z軸
        {
            transform.localPosition = new Vector3(pos.x, pos.y, pos.z + valueZ);
        }
    }
}