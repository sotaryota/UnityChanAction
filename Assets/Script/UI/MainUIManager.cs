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
    [SerializeField] GameObject panel;      //メニュー画面用のパネル
    private bool panelFlag;                 //パネルのオン・オフ
    [SerializeField] Button returnButton;   //ゲームに戻る
    [SerializeField] Button restartButton;  //再スタート
    private Scene nowScene;                 //現在のシーン
    [SerializeField] Button titleButton;    //タイトルに戻る
    [SerializeField] Button exitButton;     //ゲーム終了
    private GameObject nowButton;           //現在選択中のボタン
    private GameObject beforeButton;        //ボタンを切り替えたかどうか
    private bool initial = true;            //一度だけ処理をするための変数
    [SerializeField] GameObject selectIcon; //選択アイコン
    [SerializeField] Vector3 selectIconPos; //選択アイコンの位置

    [Header("Item")]
    private int befScore = 0;                  //ひとつ前の獲得アイテム数
    public int itemScore = 0;                  //現在の獲得アイテム数
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
        for(int i = 1;i <= 9;++i)
        {
            itemPoeces[i].SetActive(false);
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
    }
    //////////////////////////////////

    //セレクト中のボタンがわかる処理

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

    //アイテム獲得数

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

    //ポーズ解除時にジャンプしないための処理

    //////////////////////////////////
    IEnumerator PauseWait()
    {
        yield return new WaitForSeconds(0.1f);
        pause = false;
    }
    //////////////////////////////////

    //ボタンの処理

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
