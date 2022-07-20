using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour
{
    Gamepad gamepad;
    [SerializeField] MainUIManager mainUI;

    public bool goalFlag = false;
    private bool initial = true;
    private bool exitGameFlag = false;
    public Text resultText;

    private int time;
    private int timeScore = 10;
    private int timeTotalScore;
    private int item;
    private int itemScore = 100;
    private int itemTotalScore;
    private int totalScore;

    [SerializeField] private GameObject gameUIPanel;
    [SerializeField] private GameObject goalPanel;
    [SerializeField] private float waitTime;

    void Start()
    {
        mainUI = mainUI.GetComponent<MainUIManager>();
        goalPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!goalFlag)
            return;
        if (gamepad == null)
            gamepad = Gamepad.current;

        if (initial)
        {
            time = (int)mainUI.nowTime;
            item = mainUI.itemNum;
            timeTotalScore = time * timeScore;
            itemTotalScore = item * itemScore;
            totalScore = timeTotalScore + itemTotalScore;
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
                SceneManager.LoadScene("TitleScene");
            }
        }
    }

    IEnumerator ResultScore()
    {
        yield return new WaitForSeconds(waitTime);

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
