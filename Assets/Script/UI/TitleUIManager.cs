using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleUIManager : MonoBehaviour
{
    Button startButton;
    Button exitButton;
    Button tutorialButton;

    private AudioSource select;
    [SerializeField] AudioClip selectSE;

    [SerializeField] GameObject selectIcon; //選択アイコン
    [SerializeField] Vector3 selectIconPos; //選択アイコンの位置
    private GameObject nowButton;
    private GameObject beforeButton;
    int i = 0;

    private void Start()
    {
        select         = GetComponent<AudioSource>();
        startButton    = GameObject.Find("/Canvas/StartButton").GetComponent<Button>();
        exitButton     = GameObject.Find("/Canvas/ExitButton").GetComponent<Button>();
        tutorialButton = GameObject.Find("/Canvas/TutorialButton").GetComponent<Button>();
        startButton.Select();
    }
    private void Update()
    {
        SelectButtonPos();
    }
    void SelectButtonPos()
    {
        nowButton = EventSystem.current.currentSelectedGameObject;
        if (i == 0)
        {
            beforeButton = nowButton;
            selectIcon.transform.position = nowButton.transform.position - selectIconPos;
            ++i;
        }
        if (nowButton != beforeButton)
        {
            selectIcon.transform.position = nowButton.transform.position - selectIconPos;
            select.PlayOneShot(selectSE);
            beforeButton = nowButton;
        }

    }
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
}
