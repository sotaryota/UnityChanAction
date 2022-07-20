using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    private AudioSource select;
    [SerializeField] AudioClip selectSE;
    [SerializeField] GameObject controllerPanel; //�p�l��
    [SerializeField] GameObject selectIcon;      //�I���A�C�R��
    [SerializeField] Vector3 selectIconPos;      //�I���A�C�R���̈ʒu
    private GameObject nowButton;                //���ݑI�𒆂̃{�^��
    private GameObject beforeButton;             //�ЂƂO�ɑI�����Ă����{�^��
    public bool ctrlFlag = false;
    private bool initial = true;                 //��x�������������邽�߂̕ϐ�

    private void Start()
    {
        select           = GetComponent<AudioSource>();
        startButton      = GameObject.Find("/Canvas/ButtonPanel/StartButton").GetComponent<Button>();
        exitButton       = GameObject.Find("/Canvas/ButtonPanel/ExitButton").GetComponent<Button>();
        tutorialButton   = GameObject.Find("/Canvas/ButtonPanel/TutorialButton").GetComponent<Button>();
        controllerButton = GameObject.Find("/Canvas/ButtonPanel/ControllerButton").GetComponent<Button>();
        startButton.Select(); 
        controllerPanel.SetActive(false);
    }

    private void Update()
    {
        if (gamepad == null)
            gamepad = Gamepad.current;

        ControllerTutorial();
        SelectButtonPos();
    }

    //////////////////////////////////

    //�Z���N�g���̃{�^�����킩�鏈��

    //////////////////////////////////
    
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
            select.PlayOneShot(selectSE);
            //���݂̃{�^����ۑ�
            beforeButton = nowButton;
        }
    }

    //////////////////////////////////

    //������@�̕\������

    //////////////////////////////////
    
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
                controllerPanel.SetActive(false);
                buttonPanel.SetActive(true);
                StartCoroutine("PanelFalseWait");
            }
        }
    }

    //////////////////////////////////

    //�{�^���̏���

    //////////////////////////////////

    public void OnStartButton()
    {
        SceneManager.LoadScene("MainGameScene");
    }
    public void OnExitButton()
    {
        Application.Quit();
    }
    public void OnTutorialButton()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void OnControllerButton()
    {
        StartCoroutine("PanelTureWait");
        buttonPanel.SetActive(false);
        controllerPanel.SetActive(true);
    }

    //////////////////////////////////

    //�{�^�����肪�d�����Ȃ����߂̃R���[�`��

    //////////////////////////////////
    
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
