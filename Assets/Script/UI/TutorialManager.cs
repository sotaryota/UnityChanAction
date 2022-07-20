using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    Gamepad gamepad;
    
    [Header("Audio")]
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioSource bgm;

    [Header("Tutorial")]
    [SerializeField] GameObject tutorialPanel;   //チュートリアル用のパネル
    [SerializeField] GameObject[] tutorialImage; //表示する画像
    public int imageNum = 0;                     //表示したい画像の番号
    public bool tutorialFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = playerAudio.GetComponent<AudioSource>();

        if(tutorialPanel!= null)
            tutorialPanel.SetActive(false);
        
        for (int i = 1; i < tutorialImage.Length; ++i)
        {
            tutorialImage[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        if (!tutorialFlag) return;

        TutorialDisplay();
    }
    //////////////////////////////////

    //チュートリアル用のパネル

    //////////////////////////////////
    void TutorialDisplay()
    {
        if (tutorialFlag)
        {
            if (imageNum < tutorialImage.Length)
            {
                Time.timeScale = 0f;
                playerAudio.Stop();
                bgm.volume = 0.01f;
                tutorialPanel.SetActive(true);

                if (gamepad.buttonSouth.wasPressedThisFrame)
                {
                    if (imageNum + 1 == tutorialImage.Length)
                    {
                        tutorialFlag = false;
                        Time.timeScale = 1f;
                        bgm.volume = 0.05f;

                        tutorialPanel.SetActive(false);
                        StartCoroutine("PauseWaiting");
                    }
                    else
                    {
                        tutorialImage[imageNum].SetActive(false);
                        imageNum++;
                        tutorialImage[imageNum].SetActive(true);
                    }
                }
            }
        }
    }
    IEnumerator PauseWaiting()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
