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
    [SerializeField] GameObject menuPanel;     //メニュー画面用のパネル
    [SerializeField] Button returnButton;      //ゲームに戻る
    [SerializeField] Button restartButton;     //再スタート
    [SerializeField] Button titleButton;       //タイトルに戻る
    [SerializeField] Button exitButton;        //ゲーム終了
    [SerializeField] GameObject selectIcon;    //選択アイコン
    [SerializeField] Vector3 selectIconPos;    //選択アイコンの位置

    private Scene nowScene;                    //現在のシーン
    private bool panelFlag;                    //パネルのオン・オフ
    private GameObject nowButton;              //現在選択中のボタン
    private GameObject beforeButton;           //ボタンを切り替えたかどうか
    private bool initial = true;               //一度だけ処理をするための変数
    private bool initialClick = true;          //ボタン処理を重複させないためのフラグ
    public bool pause = false;                 //メニュー画面が開かれているか

    [Header("Item")]
    public int itemNum = 0;                    //現在の獲得アイテム数
    public Text itemText;                      //獲得数の表示テキスト

    [Header("Time")]
    public float nowTime = 99f;                //現在のタイム
    public Text timeText;                      //タイムの表示テキスト

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
        //ゴールしているかシーンが切り替わっているならreturn
        if (goal.goalFlag || !initialClick) { return; }

        if (gamepad == null)
            gamepad = Gamepad.current;

        MenuDisplay();
        SelectButtonPos();
        ItemCount();
        TimeCount();
    }

    //-----------------------------------------------------
    //セレクト中のボタンがわかる処理
    //-----------------------------------------------------

    void SelectButtonPos()
    {
        //現在選択中のボタン
        nowButton = EventSystem.current.currentSelectedGameObject;
        //一度だけ行う処理
        if (initial)
        {
            //とりあえず現在のボタンを保存
            beforeButton = nowButton;
            //アイコンの初期位置を決める
            selectIcon.transform.position = nowButton.transform.position - selectIconPos;
            initial = false;
        }
        //現在選択しているボタンが保存したボタンと違うなら
        if (nowButton != beforeButton)
        {
            //アイコンの位置を変更
            selectIcon.transform.position = nowButton.transform.position - selectIconPos;
            select.PlayOneShot(selectSE);
            //現在のボタンを保存
            beforeButton = nowButton;
        }
    }

    //-----------------------------------------------------
    //タイムの表示とカウント
    //-----------------------------------------------------

    void TimeCount()
    {
        //タイムが99未満の時のみ減算
        if (nowTime > 0)
        {
            nowTime -= Time.deltaTime;
        }
        int second = (int)nowTime;
        timeText.text = "タイム\n" + second.ToString();
    }

    //-----------------------------------------------------
    //アイテム獲得数表示
    //-----------------------------------------------------

    void ItemCount()
    {
        itemText.text = itemNum.ToString() + "個";
    }

    //-----------------------------------------------------
    //メニュー画面のON・OFF
    //-----------------------------------------------------

    void MenuDisplay() 
    {
        //ボタンを押したとき
        if (gamepad.startButton.wasPressedThisFrame)
        {
            if (!panelFlag) //メニュー画面がオフ
            {
                Time.timeScale = 0f;
                playerAudio.Stop();
                bgm.volume = 0.01f;

                menuPanel.SetActive(true);
                panelFlag = true;
                pause = true;
            }
            else //メニュー画面がオン
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
    //ポーズ解除時にジャンプしないための処理
    //-----------------------------------------------------

    IEnumerator PauseWait()
    {
        yield return new WaitForSeconds(0.1f);
        pause = false;
    }

    //-----------------------------------------------------
    //ボタンの処理
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
