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

    [Header("UI")]
    [SerializeField] GameObject panel;
    private bool panelFlag;
    [SerializeField] Button returnButton;   //ゲームに戻る
    [SerializeField] Button restartButton;  //再スタート
    private Scene nowScene;
    [SerializeField] Button titleButton;    //タイトルに戻る
    [SerializeField] Button exitButton;     //ゲーム終了
    private GameObject nowButton;           //現在選択中のボタン
    private GameObject beforeButton;        //ボタンを切り替えたかどうか
    private bool initial = true;            //一度だけ処理をするための変数
    [SerializeField] GameObject selectIcon; //選択アイコン
    [SerializeField] Vector3 selectIconPos; //選択アイコンの位置

    [Header("Item Poeces")]
    public int itemScore = 0;
    [SerializeField] GameObject[] itemPoeces;


    // Start is called before the first frame update
    void Start()
    {
        select        = GetComponent<AudioSource>();
        playerAudio   = GameObject.Find("player").GetComponent<AudioSource>();
        bgm           = GameObject.Find("MainCamera").GetComponent<AudioSource>();
        returnButton  = GameObject.Find("/Canvas/Panel/ReturnButton").GetComponent<Button>();
        restartButton = GameObject.Find("/Canvas/Panel/RestartButton").GetComponent<Button>();
        titleButton   = GameObject.Find("/Canvas/Panel/TitleButton").GetComponent<Button>();
        exitButton    = GameObject.Find("/Canvas/Panel/ExitButton").GetComponent<Button>();
        panel.SetActive(false); 
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
                pause = true;

                playerAudio.Stop();
                bgm.volume = 0.01f;

                panel.SetActive(true);
                panelFlag = true;
            }
            else
            {
                Time.timeScale = 1f;
                pause = false;

                bgm.volume = 0.05f;

                panel.SetActive(false);
                panelFlag = false;
            }
        }
        //セレクト中のボタンがわかる処理
        //----------------------------------------------------------------------------------
        SelectButtonPos();
    }
    void SelectButtonPos()
    {
        nowButton = EventSystem.current.currentSelectedGameObject;
        if (initial)
        {
            beforeButton = nowButton;
            selectIcon.transform.SetParent(nowButton.transform.parent);
            selectIcon.transform.position = nowButton.transform.position - selectIconPos;
            initial = false;
        }
        if(nowButton != beforeButton)
        {
            selectIcon.transform.SetParent(nowButton.transform.parent);
            selectIcon.transform.position = nowButton.transform.position - selectIconPos;
            select.PlayOneShot(selectSE);
            beforeButton = nowButton;
        }
        
    }
    //---------------------------------------------------------------------------------------
    public void OnReturnButton()
    {
        panelFlag = false;
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
