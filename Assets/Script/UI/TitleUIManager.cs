using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TitleUIManager : MonoBehaviour
{
    Gamepad gamepad;

    Button startButton;
    Button exitButton;
    Button tutorialButton;
    Button controllerButton;
    [SerializeField] GameObject buttonPanel;
    SEManager seManager;
    FadeManager fadeManager;

    [SerializeField] GameObject controllerPanel; //�p�l��
    [SerializeField] GameObject selectIcon;      //�I���A�C�R��
    [SerializeField] Vector3 selectIconPos;      //�I���A�C�R���̈ʒu
    private GameObject nowButton;                //���ݑI�𒆂̃{�^��
    private GameObject beforeButton;             //�ЂƂO�ɑI�����Ă����{�^��
    private bool ctrlFlag = false;               //������@���\������Ă��邩�ǂ���
    private bool initial = true;                 //��x�������������邽�߂̕ϐ�
    private bool initialClick = true;            //�{�^���������d�������Ȃ����߂̃t���O

    private void Start()
    {
        seManager        = GameObject.Find("SEManager").GetComponent<SEManager>();
        fadeManager      = GameObject.Find("FadeManager").GetComponent<FadeManager>();
        startButton      = GameObject.Find("/Canvas/ButtonPanel/StartButton").GetComponent<Button>();
        exitButton       = GameObject.Find("/Canvas/ButtonPanel/ExitButton").GetComponent<Button>();
        tutorialButton   = GameObject.Find("/Canvas/ButtonPanel/TutorialButton").GetComponent<Button>();
        controllerButton = GameObject.Find("/Canvas/ButtonPanel/ControllerButton").GetComponent<Button>();
        startButton.Select(); 
        controllerPanel.SetActive(false);
    }

    private void Update()
    {
        //�V�[�����؂�ւ���Ă���Ȃ�return
        if (!initialClick) { return; }

        if (gamepad == null)
            gamepad = Gamepad.current;

        ControllerTutorial();
        SelectButtonPos();
    }

    //-----------------------------------------------------
    //�Z���N�g���̃{�^�����킩�鏈��
    //-----------------------------------------------------

    void SelectButtonPos()
    {
        //���ݑI�𒆂̃{�^��
        nowButton = EventSystem.current.currentSelectedGameObject;
        //��x�����s������
        if (initial)
        {
            //�Ƃ肠�������݂̃{�^����ۑ�
            beforeButton = nowButton;
            //�A�C�R���̏����ʒu�����߂�
            selectIcon.transform.position = nowButton.transform.position - selectIconPos;
            initial = false;
        }
        //���ݑI�����Ă���{�^�����ۑ������{�^���ƈႤ�Ȃ�
        if (nowButton != beforeButton)
        {
            //�A�C�R���̈ʒu��ύX
            selectIcon.transform.position = nowButton.transform.position - selectIconPos;
            seManager.SelectSE();
            //���݂̃{�^����ۑ�
            beforeButton = nowButton;
        }
    }

    //-----------------------------------------------------
    //������@�̕\������
    //-----------------------------------------------------

    void ControllerTutorial()
    {
        if (!ctrlFlag)
        {
            return;
        }
        else
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                seManager.ClickSE();
                controllerPanel.SetActive(false);
                buttonPanel.SetActive(true);
                StartCoroutine("PanelFalseWait");
            }
        }
    }

    //-----------------------------------------------------
    //�{�^�����̏���
    //-----------------------------------------------------

    public void OnStartButton()
    {
        if (!initialClick) { return; }
        initialClick = false;
        seManager.ClickSE();
        fadeManager.FadeOut("MainGameScene", 1, 1, 1, 1f);
    }
    public void OnExitButton()
    {
        seManager.ClickSE();
        Application.Quit();
    }
    public void OnTutorialButton()
    {
        if (!initialClick) { return; }
        initialClick = false;
        seManager.ClickSE();
        fadeManager.FadeOut("TutorialScene", 1, 1, 1, 1f);
    }

    public void OnControllerButton()
    {
        seManager.ClickSE();
        StartCoroutine("PanelTureWait");
        buttonPanel.SetActive(false);
        controllerPanel.SetActive(true);
    }

    //-----------------------------------------------------
    //�{�^�����肪�d�����Ȃ����߂̏���
    //-----------------------------------------------------

    IEnumerator PanelTureWait()
    {
        yield return new WaitForSeconds(0.1f);
        ctrlFlag = true;

    }
    IEnumerator PanelFalseWait()
    {
        yield return new WaitForSeconds(0.1f);
        ctrlFlag = false;
    }

}
