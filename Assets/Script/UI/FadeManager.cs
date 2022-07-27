using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    #region シングルトン

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

    private Color color;         //フェードアウトの色
    private float alpha = 0f;    //透明度
    private bool isFade = false; //フェードしているかどうか

    private void OnGUI()
    {
        if(isFade)
        {
            //透明度をalphaの値に
            color.a = alpha;

            //GUIで表示
            //------------------------------------------------------------------------------------
            GUI.color = color;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            //------------------------------------------------------------------------------------
        }
    }

    //-----------------------------------------------------
    //シーンを切り替えたい時に呼び出す関数
    //引数（シーン名,RGB(色指定),フェードの間隔）
    //-----------------------------------------------------

    public void FadeOut(string scene, int r,int g, int b, float interval)
    {
        color = new Color(r, g, b);
        StartCoroutine(Fade(scene, interval));
    }

    //-----------------------------------------------------
    //関数内で呼び出される処理
    //-----------------------------------------------------
    
    private IEnumerator Fade(string scene, float interval)
    {
        isFade = true;

        //フェードのカウント
        float fadetime = 0;

        //フェードアウト
        while (fadetime <= interval)
        {
            //透明度を少しずつ上げる
            alpha = Mathf.Lerp(0f, 1f, fadetime / interval);
            fadetime += Time.deltaTime;
            yield return 0;
        }

        //フェードインが終了次第シーン切り替え
        SceneManager.LoadScene(scene);

        //カウントを0に
        fadetime = 0;

        //フェードイン
        while (fadetime <= interval)
        {
            //透明度を少しずつ下げる
            alpha = Mathf.Lerp(1f, 0f, fadetime / interval);
            fadetime += Time.deltaTime;
            yield return 0;
        }

        isFade = false;
    }
}
