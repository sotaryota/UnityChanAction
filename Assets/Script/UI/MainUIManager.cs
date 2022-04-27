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
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioSource bgm;

    [Header("UI")]
    [SerializeField] GameObject panel;
    private bool panelFlag;
    [SerializeField] Button returnButton;   //ゲームに戻る
    [SerializeField] Button restartButton;  //再スタート
    [SerializeField] Button titleButton;    //タイトルに戻る
    [SerializeField] Button exitButton;     //ゲーム終了
    private GameObject button;
    [SerializeField] GameObject selectIcon; //選択アイコン
    [SerializeField] Vector3 selectIconPos; //選択アイコンの位置

    // Start is called before the first frame update
    void Start()
    {
        playerAudio   = GameObject.Find("player").GetComponent<AudioSource>();
        bgm           = GameObject.Find("MainCamera").GetComponent<AudioSource>();
        returnButton  = GameObject.Find("/Canvas/Panel/ReturnButton").GetComponent<Button>();
        restartButton = GameObject.Find("/Canvas/Panel/RestartButton").GetComponent<Button>();
        titleButton   = GameObject.Find("/Canvas/Panel/TitleButton").GetComponent<Button>();
        exitButton    = GameObject.Find("/Canvas/Panel/ExitButton").GetComponent<Button>();
        panel.SetActive(false); 
        returnButton.Select();
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
        //button = EventSystem.current.currentSelectedGameObject;
        //selectIconPos = button.transform.position - new Vector3(150, 0, 0);
        //SelectButtonPos();
        //Debug.Log(selectIcon.transform.position);
    }
    //void SelectButtonPos()
    //{
    //    if (button == restartButton)
    //    {
    //        selectIcon.transform.position = selectIconPos;
    //    }
    //    else if (button == titleButton)
    //    {
    //        selectIcon.transform.position = selectIconPos;
    //    }
    //    else if (button == returnButton)
    //    {
    //        selectIcon.transform.position = selectIconPos;
    //    }
    //    else if(button == exitButton)
    //    {
    //        selectIcon.transform.position = selectIconPos;
    //    }
    //}
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
        SceneManager.LoadScene("MainGameScene");
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
