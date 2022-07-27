using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    #region �V���O���g��

    private static FadeManager instance;

    public static FadeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (FadeManager)FindObjectOfType(typeof(FadeManager));

                if (instance == null)
                {
                    Debug.LogError("FadeManager Instance nothing");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    private Color color;         //�t�F�[�h�A�E�g�̐F
    private float alpha = 0f;    //�����x
    private bool isFade = false; //�t�F�[�h���Ă��邩�ǂ���

    private void OnGUI()
    {
        if(isFade)
        {
            //�����x��alpha�̒l��
            color.a = alpha;

            //GUI�ŕ\��
            //------------------------------------------------------------------------------------
            GUI.color = color;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            //------------------------------------------------------------------------------------
        }
    }

    //-----------------------------------------------------
    //�V�[����؂�ւ��������ɌĂяo���֐�
    //�����i�V�[����,RGB(�F�w��),�t�F�[�h�̊Ԋu�j
    //-----------------------------------------------------

    public void FadeOut(string scene, int r,int g, int b, float interval)
    {
        color = new Color(r, g, b);
        StartCoroutine(Fade(scene, interval));
    }

    //-----------------------------------------------------
    //�֐����ŌĂяo����鏈��
    //-----------------------------------------------------
    
    private IEnumerator Fade(string scene, float interval)
    {
        isFade = true;

        //�t�F�[�h�̃J�E���g
        float fadetime = 0;

        //�t�F�[�h�A�E�g
        while (fadetime <= interval)
        {
            //�����x���������グ��
            alpha = Mathf.Lerp(0f, 1f, fadetime / interval);
            fadetime += Time.deltaTime;
            yield return 0;
        }

        //�t�F�[�h�C�����I������V�[���؂�ւ�
        SceneManager.LoadScene(scene);

        //�J�E���g��0��
        fadetime = 0;

        //�t�F�[�h�C��
        while (fadetime <= interval)
        {
            //�����x��������������
            alpha = Mathf.Lerp(1f, 0f, fadetime / interval);
            fadetime += Time.deltaTime;
            yield return 0;
        }

        isFade = false;
    }
}
