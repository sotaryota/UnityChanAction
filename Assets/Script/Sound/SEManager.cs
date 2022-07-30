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
        audioSource.volume = 0.05f;
        audioSource.pitch = 1.5f;
        audioSource.PlayOneShot(itemSE);
    }
    public void SelectSE()
    {
        audioSource.volume = 0.1f;
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(selectSE);
    }
    public void ClickSE()
    {
        audioSource.volume = 0.1f;
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(clickSE);
    }
}
