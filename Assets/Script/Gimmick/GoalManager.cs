using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour
{
    Gamepad gamepad;
    [SerializeField] MainUIManager mainUI;
    [SerializeField] AudioSource bgm;
    FadeManager fadeManager;

    public bool goalFlag = false;       //ゴールしているか
    private bool initial = true;        //ゴール時に一度だけ処理をするための変数
    private bool initialClick = true;        //ボタン処理を重複させないためのフラグ
    private bool exitGameFlag = false;  //trueならゲームを終了できる
    public Text resultText;             //リザルトの表示テキスト

    private int time;                   //残りタイム
    private int timeScore = 10;         //タイムの基本スコア
    private int timeTotalScore;         //タイムの合計スコア
    private int item;                   //アイテム獲得数
    private int itemScore = 100;        //アイテムの基本スコア
    private int itemTotalScore;         //アイテムの合計スコア
    private int totalScore;             //タイムとアイテムの合計スコア

    [SerializeField] private GameObject gameUIPanel; //ゲームのメインUI
    [SerializeField] private GameObject goalPanel;   //ゴール時に表示するUI
    [SerializeField] private float waitTime;         //テキストの表示間隔

    void Start()
    {
        mainUI      = mainUI.GetComponent<MainUIManager>();
        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>(); 
        goalPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //ゴールしていないならreturn
        if (!goalFlag) { return; }

        if (gamepad == null)
            gamepad = Gamepad.current;

        //ゴールした際に一度だけ処理
        if (initial)
        {
            //表示したい値の計算と用意
            //--------------------------------------------
            time = (int)mainUI.nowTime;
            item = mainUI.itemNum;
            timeTotalScore = time * timeScore;
            itemTotalScore = item * itemScore;
            totalScore = timeTotalScore + itemTotalScore;
            //--------------------------------------------

            StartCoroutine("ResultScore");
            initial = false;
        }

        ExitGame();
    }

    private void ExitGame()
    {
        if (exitGameFlag)
        {
            Debug.Log("ゲームを終了できます");
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                if (!initialClick) { return; }
                initialClick = false;
                fadeManager.FadeOut("TitleScene", 1, 1, 1, 1f);
            }
        }
    }

    //-----------------------------------------------------
    //テキストの表示とゲーム終了に移行する処理
    //-----------------------------------------------------
    IEnumerator ResultScore()
    {
        while(bgm.volume > 0)
        {
            bgm.volume -= 0.0001f;
            yield return 0;
        }

        gameUIPanel.SetActive(false);
        goalPanel.SetActive(true);

        yield return new WaitForSeconds(waitTime);


        resultText.text = "残りタイム\n" + time.ToString() + " × " + timeScore.ToString() +  " = " + timeTotalScore.ToString() + "\n\n";

         yield return new WaitForSeconds(waitTime);

        resultText.text += "アイテム獲得数\n" + item.ToString() + " × " + itemScore.ToString() + " = " + itemTotalScore.ToString() + "\n\n";


        yield return new WaitForSeconds(waitTime);

        resultText.text += "合計スコア\n" + timeTotalScore.ToString() + " + " + itemTotalScore.ToString() + " = " + totalScore.ToString() + "\n\n";

        yield return new WaitForSeconds(waitTime);

        exitGameFlag = true;
    }
}
