using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;                   //DOTweenを使うときはこのusingを入れる
using KanKikuchi.AudioManager;       //AudioManagerを使うときはこのusingを入れる

public class TitleManager : MonoBehaviour
{

    [SerializeField] GameObject fade;
    [SerializeField] GameObject menu;
    public static bool isMenuFlag;

    

    void Start()
    {
        fade.SetActive(true);               //フェードを有効化
        menu.SetActive(false);              //メニューを非表示
        isMenuFlag = false;                 //メニューフラグを無効化

        //BGM再生
        BGMManager.Instance.Play(
           audioPath: BGMPath.NATUKAZE,     //再生したいオーディオのパス
           volumeRate: 1,                   //音量の倍率
           delay: 0.2f,                     //再生されるまでの遅延時間
           pitch: 1,                        //ピッチ
           isLoop: true,                    //ループ再生するか
           allowsDuplicate: false           //他のBGMと重複して再生させるか
);
    }

    public void OpenOptionButton()
    {
        menu.SetActive(true);
        isMenuFlag = true;
        
    }

    public void CloseOptionButton()
    {
        isMenuFlag = false;
        Invoke("CloseMenu", 0.5f);
        
    }

    void CloseMenu()
    {
        menu.SetActive(false);
    }

    public void StartGame()
    {

    }

}

