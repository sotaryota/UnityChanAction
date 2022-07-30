using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour
{
    Gamepad gamepad;
    [SerializeField] MainUIManager mainUI;
    [SerializeField] AudioSource bgm;
    FadeManager fadeManager;

    public bool goalFlag = false;       //�S�[�����Ă��邩
    private bool initial = true;        //�S�[�����Ɉ�x�������������邽�߂̕ϐ�
    private bool initialClick = true;        //�{�^���������d�������Ȃ����߂̃t���O
    private bool exitGameFlag = false;  //true�Ȃ�Q�[�����I���ł���
    public Text resultText;             //���U���g�̕\���e�L�X�g

    private int time;                   //�c��^�C��
    private int timeScore = 10;         //�^�C���̊�{�X�R�A
    private int timeTotalScore;         //�^�C���̍��v�X�R�A
    private int item;                   //�A�C�e���l����
    private int itemScore = 100;        //�A�C�e���̊�{�X�R�A
    private int itemTotalScore;         //�A�C�e���̍��v�X�R�A
    private int totalScore;             //�^�C���ƃA�C�e���̍��v�X�R�A

    [SerializeField] private GameObject gameUIPanel; //�Q�[���̃��C��UI
    [SerializeField] private GameObject goalPanel;   //�S�[�����ɕ\������UI
    [SerializeField] private float waitTime;         //�e�L�X�g�̕\���Ԋu

    void Start()
    {
        mainUI      = mainUI.GetComponent<MainUIManager>();
        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>(); 
        goalPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //�S�[�����Ă��Ȃ��Ȃ�return
        if (!goalFlag) { return; }

        if (gamepad == null)
            gamepad = Gamepad.current;

        //�S�[�������ۂɈ�x��������
        if (initial)
        {
            //�\���������l�̌v�Z�Ɨp��
            //--------------------------------------------
            time = (int)mainUI.nowTime;
            item = mainUI.itemNum;
            timeTotalScore = time * timeScore;
            itemTotalScore = item * itemScore;
            totalScore = timeTotalScore + itemTotalScore;
            //--------------------------------------------

            StartCoroutine("ResultScore");
            initial = false;
        }

        ExitGame();
    }

    private void ExitGame()
    {
        if (exitGameFlag)
        {
            Debug.Log("�Q�[�����I���ł��܂�");
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                if (!initialClick) { return; }
                initialClick = false;
                fadeManager.FadeOut("TitleScene", 1, 1, 1, 1f);
            }
        }
    }

    //-----------------------------------------------------
    //�e�L�X�g�̕\���ƃQ�[���I���Ɉڍs���鏈��
    //-----------------------------------------------------
    IEnumerator ResultScore()
    {
        while(bgm.volume > 0)
        {
            bgm.volume -= 0.0001f;
            yield return 0;
        }

        gameUIPanel.SetActive(false);
        goalPanel.SetActive(true);

        yield return new WaitForSeconds(waitTime);


        resultText.text = "�c��^�C��\n" + time.ToString() + " �~ " + timeScore.ToString() +  " = " + timeTotalScore.ToString() + "\n\n";

         yield return new WaitForSeconds(waitTime);

        resultText.text += "�A�C�e���l����\n" + item.ToString() + " �~ " + itemScore.ToString() + " = " + itemTotalScore.ToString() + "\n\n";


        yield return new WaitForSeconds(waitTime);

        resultText.text += "���v�X�R�A\n" + timeTotalScore.ToString() + " + " + itemTotalScore.ToString() + " = " + totalScore.ToString() + "\n\n";

        yield return new WaitForSeconds(waitTime);

        exitGameFlag = true;
    }
}
