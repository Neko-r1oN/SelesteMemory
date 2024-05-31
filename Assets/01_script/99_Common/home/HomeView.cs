using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class HomeView : ViewBase
{
    // ボタントランジション.
    [SerializeField] UITransition buttonTransition = null;

    void Start()
    {
    }

    // -------------------------------------------------------
    // ビューオープン時コール.
    // -------------------------------------------------------
    public override async void OnViewOpened()
    {
        base.OnViewOpened();
        await Open();
    }

    // --------------------------------------------
    // ウインドウを開く.
    // --------------------------------------------
    async UniTask Open()
    {
        buttonTransition.Canvas.alpha = 0;
        buttonTransition.gameObject.SetActive(true);

        await buttonTransition.TransitionInWait();
    }

    // --------------------------------------------
    // ウインドウを閉じる.
    // --------------------------------------------
    async UniTask Close()
    {
        await buttonTransition.TransitionOutWait();
        buttonTransition.gameObject.SetActive(false);
    }

    // -------------------------------------------------------
    // ビュークローズ時コール.
    // -------------------------------------------------------
    public override void OnViewClosed()
    {
        base.OnViewClosed();
    }
}