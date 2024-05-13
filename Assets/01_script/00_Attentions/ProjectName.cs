using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class ProjectName : MonoBehaviour
{
    private Text nameTxt;

    private float timer;    //繰り返す間隔

    [Header("スタートカラー")]
    [SerializeField]
    Color32 startColor = new Color32(255, 255, 255, 0);
   

    void Start()
    {
        nameTxt = GetComponent<Text>();
        nameTxt.color = startColor;

        timer = 0.0f;
    }

    void Update()
    {
        nameTxt.color = Color.Lerp(nameTxt.color, new Color(1, 1, 1, 1), 2.0f * Time.deltaTime);

        timer += Time.deltaTime;     //時間をカウントする


        if (timer >= 3.0f)
        {
            nameTxt.color = Color.Lerp(nameTxt.color, new Color(0, 0, 0, -3), 2.0f * Time.deltaTime);
            
        }
    }

  

}