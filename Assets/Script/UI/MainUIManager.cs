using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    Gamepad gamepad;
    public bool pause = false;

    [Header("Audio")]
    private AudioSource select;
    [SerializeField] AudioClip selectSE;
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioSource bgm;

    [Header("Main Menu")]
    [SerializeField] GameObject panel;         //���j���[��ʗp�̃p�l��
    private bool panelFlag;                    //�p�l���̃I���E�I�t
    [SerializeField] Button returnButton;      //�Q�[���ɖ߂�
    [SerializeField] Button restartButton;     //�ăX�^�[�g
    private Scene nowScene;                    //���݂̃V�[��
    [SerializeField] Button titleButton;       //�^�C�g���ɖ߂�
    [SerializeField] Button exitButton;        //�Q�[���I��
    private GameObject nowButton;              //���ݑI�𒆂̃{�^��
    private GameObject beforeButton;           //�{�^����؂�ւ������ǂ���
    private bool initial = true;               //��x�������������邽�߂̕ϐ�
    [SerializeField] GameObject selectIcon;    //�I���A�C�R��
    [SerializeField] Vector3 selectIconPos;    //�I���A�C�R���̈ʒu

    [Header("Item")]
    private int befScore = 0;                  //�ЂƂO�̊l���A�C�e����
    public int itemScore = 0;                  //���݂̊l���A�C�e����
    [SerializeField] GameObject[] itemPoeces;  //�X�R�A�̃X�v���C�g

    [Header("Time")]
    private int befTime = 0;
    private float nowTime = 0f;
    [SerializeField] GameObject[] timeNum;
    [SerializeField] GameObject[] timeNum2;

    // Start is called before the first frame update
    void Start()
    {
        select        = GetComponent<AudioSource>();
        playerAudio   = GameObject.Find("player").GetComponent<AudioSource>();
        bgm           = GameObject.Find("MainCamera").GetComponent<AudioSource>();
        returnButton  = GameObject.Find("/Canvas/MainMenuPanel/ReturnButton").GetComponent<Button>();
        restartButton = GameObject.Find("/Canvas/MainMenuPanel/RestartButton").GetComponent<Button>();
        titleButton   = GameObject.Find("/Canvas/MainMenuPanel/TitleButton").GetComponent<Button>();
        exitButton    = GameObject.Find("/Canvas/MainMenuPanel/ExitButton").GetComponent<Button>();
        panel.SetActive(false); 
        for(int i = 1;i <= 9;++i)
        {
            itemPoeces[i].SetActive(false);
            timeNum[i].SetActive(false);
            timeNum2[i].SetActive(false);
        }
        returnButton.Select();
        nowScene = SceneManager.GetActiveScene();
    }

        // Update is called once per frame
    void Update()
    {
        if (gamepad == null)
            gamepad = Gamepad.current;
        if(gamepad.startButton.wasPressedThisFrame)
        {
            if(!panelFlag)
            {
                Time.timeScale = 0f;
                playerAudio.Stop();
                bgm.volume = 0.01f;

                panel.SetActive(true);
                panelFlag = true;
                pause = true;
            }
            else
            {
                Time.timeScale = 1f;
                bgm.volume = 0.05f;

                panel.SetActive(false);
                panelFlag = false; 
                StartCoroutine("PauseWait");
            }
        }
        SelectButtonPos();
        ItemCount();
        TimeCnt();
    }
    //////////////////////////////////

    //�Z���N�g���̃{�^�����킩�鏈��

    //////////////////////////////////
    void SelectButtonPos()
    {
        nowButton = EventSystem.current.currentSelectedGameObject;
        if (initial)
        {
            beforeButton = nowButton;
            selectIcon.transform.position = nowButton.transform.position - selectIconPos;
            initial = false;
        }
        if(nowButton != beforeButton)
        {
            selectIcon.transform.position = nowButton.transform.position - selectIconPos;
            select.PlayOneShot(selectSE);
            beforeButton = nowButton;
        }

    }

    //////////////////////////////////

    //�^�C���̕\���ƃJ�E���g

    //////////////////////////////////
    void TimeCnt()
    {
        if (nowTime <= 99)
        {
            nowTime += Time.deltaTime;
            if ((int)nowTime != befTime)
            {
                timeNum2[(int)nowTime % 10].SetActive(true);
                timeNum2[(int)befTime % 10].SetActive(false);
                befTime = (int)nowTime;
            }
        }
    }
    //////////////////////////////////

    //�A�C�e���l����

    //////////////////////////////////
    void ItemCount()
    {
        if(itemScore != befScore)
        {
            itemPoeces[befScore].SetActive(false);
            itemPoeces[itemScore].SetActive(true);
            befScore = itemScore; 
        }
    }
    //////////////////////////////////

    //�|�[�Y�������ɃW�����v���Ȃ����߂̏���

    //////////////////////////////////
    IEnumerator PauseWait()
    {
        yield return new WaitForSeconds(0.1f);
        pause = false;
    }
    //////////////////////////////////

    //�{�^���̏���

    //////////////////////////////////
    public void OnReturnButton()
    {
        panelFlag = false;
        StartCoroutine("PauseWait");
        panel.SetActive(false);
        bgm.volume = 0.05f;
        Time.timeScale = 1;
    }
    public void OnRestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nowScene.name);
    }
    public void OnTitleButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }
    public void OnExitButton()
    {
        Application.Quit();
    }
}
