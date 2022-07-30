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

    [SerializeField] GameObject controllerPanel; //パネル
    [SerializeField] GameObject selectIcon;      //選択アイコン
    [SerializeField] Vector3 selectIconPos;      //選択アイコンの位置
    private GameObject nowButton;                //現在選択中のボタン
    private GameObject beforeButton;             //ひとつ前に選択していたボタン
    private bool ctrlFlag = false;               //操作方法が表示されているかどうか
    private bool initial = true;                 //一度だけ処理をするための変数
    private bool initialClick = true;            //ボタン処理を重複させないためのフラグ

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
        //シーンが切り替わっているならreturn
        if (!initialClick) { return; }

        if (gamepad == null)
            gamepad = Gamepad.current;

        ControllerTutorial();
        SelectButtonPos();
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
            seManager.SelectSE();
            //現在のボタンを保存
            beforeButton = nowButton;
        }
    }

    //-----------------------------------------------------
    //操作方法の表示処理
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
    //ボタン毎の処理
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
    //ボタン判定が重複しないための処理
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
