using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる




public class AttentionText : MonoBehaviour
{
    private Text AttentionTxt;

    private float timer;    //繰り返す間隔

    [Header("スタートカラー")]
    [SerializeField]
    Color32 startColor = new Color32(255, 255, 255, 0);
    //ループ終了(折り返し)時の色を0～255までの整数で指定。
    [Header("エンドカラー")]
    [SerializeField]
    Color32 endColor = new Color32(255, 255, 255, 255);

    void Start()
    {
        AttentionTxt = GetComponent<Text>();
        AttentionTxt.color = startColor;

        timer = 0.0f;
    }

    void Update()
    {
        timer += Time.deltaTime;     //時間をカウントする
        if (timer >= 4.0f)
        {
            AttentionTxt.color = Color.Lerp(AttentionTxt.color, new Color(1, 1, 1, 1), 2.0f * Time.deltaTime);




            if (timer >= 7.0f)
            {
                AttentionTxt.color = Color.Lerp(AttentionTxt.color, new Color(0, 0, 0, -3), 2.0f * Time.deltaTime);
                Invoke("StartTitleScene", 1.0f);
            }
        }
    }

    public void StartTitleScene()
    {
        // シーン遷移
        Initiate.Fade("TitleScene", new Color(0,0, 0, 0), 2.0f);
    }

}