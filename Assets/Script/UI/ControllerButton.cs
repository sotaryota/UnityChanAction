using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerButton : MonoBehaviour
{
    Gamepad gamepad;
    [SerializeField] GameObject controllerImage;
    public bool ctrlFlag = false;
    private void Start()
    {
        controllerImage.SetActive(false);
    }
    private void Update()
    {
        if (gamepad == null)
            gamepad = Gamepad.current;

        if (!ctrlFlag) return;
        ControllerTutorial();
    }
    void ControllerTutorial()
    {
        controllerImage.SetActive(true);
        if(gamepad.buttonSouth.wasPressedThisFrame)
        {
            controllerImage.SetActive(false);
            ctrlFlag = false;
        }
    }
}
