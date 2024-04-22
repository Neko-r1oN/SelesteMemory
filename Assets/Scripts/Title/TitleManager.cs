using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;  //DOTween‚ðŽg‚¤‚Æ‚«‚Í‚±‚Ìusing‚ð“ü‚ê‚é

public class TitleManager : MonoBehaviour
{
    

    [SerializeField] GameObject fade;

    [SerializeField] GameObject menu;
    public static bool isMenuFlag;

    void Start()
    {
        fade.SetActive(true);
        menu.SetActive(false);

        isMenuFlag = false;
        
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
}

