using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;  //DOTween‚ðŽg‚¤‚Æ‚«‚Í‚±‚Ìusing‚ð“ü‚ê‚é

public class MenuManager : MonoBehaviour
{

    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OpenOptionAnim()
    {
        //menu.SetActive(true);
        animator.SetBool("isManu", true);
    }

    public void CloseOptionAnim()
    {
       
        animator.SetBool("isManu", false);
    }

}

