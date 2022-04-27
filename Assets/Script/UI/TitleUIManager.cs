using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    Button startButton;
    Button exitButton;

    private void Start()
    {
        startButton = GameObject.Find("/Canvas/StartButton").GetComponent<Button>();
        exitButton = GameObject.Find("/Canvas/ExitButton").GetComponent<Button>();
        startButton.Select();
    }
    public void OnStartButton()
    {
        Debug.Log("ボタンを押した");
        SceneManager.LoadScene("MainGameScene");
    }
    public void OnExitButton()
    {
        Debug.Log("ボタンを押した");
        Application.Quit();
    }
}
