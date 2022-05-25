using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalProcess : MonoBehaviour
{
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
    }
}
