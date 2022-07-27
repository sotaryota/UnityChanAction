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
    [SerializeField] GoalManager goal;
    FadeManager fadeManager;

    [Header("Audio")]
    private AudioSource select;
    [SerializeField] AudioClip selectSE;
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioSource bgm;

    [Header("Main Menu")]
    [SerializeField] GameObject menuPanel;     //���j���[��ʗp�̃p�l��
    [SerializeField] Button returnButton;      //�Q�[���ɖ߂�
    [SerializeField] Button restartButton;     //�ăX�^�[�g
    [SerializeField] Button titleButton;       //�^�C�g���ɖ߂�
    [SerializeField] Button exitButton;        //�Q�[���I��
    [SerializeField] GameObject selectIcon;    //�I���A�C�R��
    [SerializeField] Vector3 selectIconPos;    //�I���A�C�R���̈ʒu

    private Scene nowScene;                    //���݂̃V�[��
    private bool panelFlag;                    //�p�l���̃I���E�I�t
    private GameObject nowButton;              //���ݑI�𒆂̃{�^��
    private GameObject beforeButton;           //�{�^����؂�ւ������ǂ���
    private bool initial = true;               //��x�������������邽�߂̕ϐ�
    private bool initialClick = true;          //�{�^���������d�������Ȃ����߂̃t���O
    public bool pause = false;                 //���j���[��ʂ��J����Ă��邩

    [Header("Item")]
    public int itemNum = 0;                    //���݂̊l���A�C�e����
    public Text itemText;                      //�l�����̕\���e�L�X�g

    [Header("Time")]
    public float nowTime = 99f;                //���݂̃^�C��
    public Text timeText;                      //�^�C���̕\���e�L�X�g

    // Start is called before the first frame update
    void Start()
    {
        select        = GetComponent<AudioSource>();
        playerAudio   = playerAudio.GetComponent<AudioSource>();
        bgm           = bgm.GetComponent<AudioSource>();
        goal          = goal.GetComponent<GoalManager>();
        fadeManager   = GameObject.Find("FadeManager").GetComponent<FadeManager>();
        returnButton  = GameObject.Find("/Canvas/MainMenuPanel/ReturnButton").GetComponent<Button>();
        restartButton = GameObject.Find("/Canvas/MainMenuPanel/RestartButton").GetComponent<Button>();
        titleButton   = GameObject.Find("/Canvas/MainMenuPanel/TitleButton").GetComponent<Button>();
        exitButton    = GameObject.Find("/Canvas/MainMenuPanel/ExitButton").GetComponent<Button>();
        menuPanel.SetActive(false); 
        
        returnButton.Select();
        nowScene = SceneManager.GetActiveScene();
    }

        // Update is called once per frame
    void Update()
    {
        //�S�[�����Ă��邩�V�[�����؂�ւ���Ă���Ȃ�return
        if (goal.goalFlag || !initialClick) { return; }

        if (gamepad == null)
            gamepad = Gamepad.current;

        MenuDisplay();
        SelectButtonPos();
        ItemCount();
        TimeCount();
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
            select.PlayOneShot(selectSE);
            //���݂̃{�^����ۑ�
            beforeButton = nowButton;
        }
    }

    //-----------------------------------------------------
    //�^�C���̕\���ƃJ�E���g
    //-----------------------------------------------------

    void TimeCount()
    {
        //�^�C����99�����̎��̂݌��Z
        if (nowTime > 0)
        {
            nowTime -= Time.deltaTime;
        }
        int second = (int)nowTime;
        timeText.text = "�^�C��\n" + second.ToString();
    }

    //-----------------------------------------------------
    //�A�C�e���l�����\��
    //-----------------------------------------------------

    void ItemCount()
    {
        itemText.text = itemNum.ToString() + "��";
    }

    //-----------------------------------------------------
    //���j���[��ʂ�ON�EOFF
    //-----------------------------------------------------

    void MenuDisplay() 
    {
        //�{�^�����������Ƃ�
        if (gamepad.startButton.wasPressedThisFrame)
        {
            if (!panelFlag) //���j���[��ʂ��I�t
            {
                Time.timeScale = 0f;
                playerAudio.Stop();
                bgm.volume = 0.01f;

                menuPanel.SetActive(true);
                panelFlag = true;
                pause = true;
            }
            else //���j���[��ʂ��I��
            {
                Time.timeScale = 1f;
                bgm.volume = 0.05f;

                menuPanel.SetActive(false);
                panelFlag = false;
                StartCoroutine("PauseWait");
            }
        }
    }

    //-----------------------------------------------------
    //�|�[�Y�������ɃW�����v���Ȃ����߂̏���
    //-----------------------------------------------------

    IEnumerator PauseWait()
    {
        yield return new WaitForSeconds(0.1f);
        pause = false;
    }

    //-----------------------------------------------------
    //�{�^���̏���
    //-----------------------------------------------------

    public void OnReturnButton()
    {
        panelFlag = false;
        StartCoroutine("PauseWait");
        menuPanel.SetActive(false);
        bgm.volume = 0.05f;
        Time.timeScale = 1;
    }
    public void OnRestartButton()
    {
        if (!initialClick) { return; }
        initialClick = false;
        Time.timeScale = 1;
        fadeManager.FadeOut(nowScene.name, 0, 0, 0, 0.5f);
    }
    public void OnTitleButton()
    {
        if (!initialClick) { return; }
        initialClick = false;
        Time.timeScale = 1f; 
        fadeManager.FadeOut("TitleScene", 0, 0, 0, 1f);
    }
    public void OnExitButton()
    {
        Application.Quit();
    }
}
