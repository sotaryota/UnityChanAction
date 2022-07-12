using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GoalProcess : MonoBehaviour
{
    Gamepad gamepad;

    public bool goalFlag = false;
    private bool initial = true;
    private bool exitGameFlag = false;

    // [SerializeField] private GameObject goalScorePanel;

    // Start is called before the first frame update
    void Start()
    {
        //goalScorePanel.SetActive(false);
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
            StartCoroutine("TimeScore");
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

    IEnumerator TimeScore()
    {
        yield return new WaitForSeconds(1.5f);

        Debug.LogWarning("タイムのスコア表示");

        StartCoroutine("ItemScore");
    }
    IEnumerator ItemScore()
    {
        yield return new WaitForSeconds(1.5f);

        Debug.LogWarning("アイテムのスコア表示");

        StartCoroutine("TotalScore");
    }
    IEnumerator TotalScore()
    {
        yield return new WaitForSeconds(1.5f);

        Debug.LogWarning("トータルスコア表示");

        yield return new WaitForSeconds(1.5f);

        exitGameFlag = true;
    }
}
