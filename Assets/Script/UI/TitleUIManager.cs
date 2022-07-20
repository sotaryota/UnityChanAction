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
    [SerializeField] GameObject controllerPanel; //パネル
    [SerializeField] GameObject selectIcon;      //選択アイコン
    [SerializeField] Vector3 selectIconPos;      //選択アイコンの位置
    private GameObject nowButton;                //現在選択中のボタン
    private GameObject beforeButton;             //ひとつ前に選択していたボタン
    public bool ctrlFlag = false;
    private bool initial = true;                 //一度だけ処理をするための変数

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

    //セレクト中のボタンがわかる処理

    //////////////////////////////////
    
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

    //////////////////////////////////

    //操作方法の表示処理

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

    //ボタンの処理

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

    //ボタン判定が重複しないためのコルーチン

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
