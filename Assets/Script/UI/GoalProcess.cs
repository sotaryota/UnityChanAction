using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GoalProcess : MonoBehaviour
{
    Gamepad gamepad;

    public bool goalFlag = false;
    [SerializeField] private GameObject goalScorePanel;

    // Start is called before the first frame update
    void Start()
    {
        goalScorePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!goalFlag)
            return;
        if (gamepad == null)
            gamepad = Gamepad.current;
        if (gamepad.buttonNorth.wasPressedThisFrame)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
