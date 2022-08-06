using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [Header("スクリプト")]
    SEManager seManager; 
    PlayerStatus status;
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] GoalManager goal;
    [SerializeField] MainUIManager ui;

    [Header("プレイヤ情報")]
    [SerializeField] GameObject startPos; //キャラの初期位置
    [SerializeField] GameObject onBlock;  //プレイヤが乗っているブロック

    private void Start()
    {
        status          = GetComponent<PlayerStatus>();
        ui              = ui.GetComponent<MainUIManager>();
        goal            = goal.GetComponent<GoalManager>();
        tutorialManager = tutorialManager.GetComponent<TutorialManager>();
        seManager       = GameObject.Find("SEManager").GetComponent<SEManager>();

        //初期位置をスタート地点にする
        transform.position = startPos.transform.position;
    }

    //-----------------------------------------------------
    //オブジェクトとの接触判定
    //-----------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            ui.itemNum++;
            seManager.ItemSE();
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Goal")
        {
            goal.goalFlag = true;
        }
        if (other.gameObject.tag == "Tutorial" && tutorialManager.imageNum < tutorialManager.tutorialImage.Length + 1)
        {
            tutorialManager.tutorialFlag = true;
            other.gameObject.SetActive(false);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fall")
        {
            status.Damage(status.getMaxHp() / 4);
            transform.position = startPos.transform.position;
        }
    }

    //-----------------------------------------------------
    //動く床に乗った時の処理
    //-----------------------------------------------------

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
