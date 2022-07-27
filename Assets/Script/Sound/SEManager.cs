using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    #region ƒVƒ“ƒOƒ‹ƒgƒ“

    private static SEManager seInstance;

    public static SEManager SEInstance
    {
        get
        {
            if (seInstance == null)
            {
                seInstance = (SEManager)FindObjectOfType(typeof(SEManager));

                if (seInstance == null)
                {
                    Debug.LogError("SEManager Instance nothing");
                }
            }

            return seInstance;
        }
    }

    private void Awake()
    {
        if (this != SEInstance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip itemSE;
    [SerializeField] private AudioClip clickSE;
    [SerializeField] private AudioClip selectSE;

    private void Start()
    {
        audioSource = audioSource.GetComponent<AudioSource>();
    }

    public void ItemSE()
    {
        audioSource.PlayOneShot(itemSE);
    }
    public void SelectSE()
    {
        audioSource.PlayOneShot(selectSE);
    }
    public void ClickSE()
    {
        audioSource.PlayOneShot(clickSE);
    }
}
