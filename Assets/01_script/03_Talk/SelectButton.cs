
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

// ------------------------------------------------------------
/// 選択肢のボタン.
// ------------------------------------------------------------
public class SelectButton : MonoBehaviour
{
    // テキスト.
    [SerializeField] Text buttonText = null;
    // トランジション.
    [SerializeField] UITransition transition = null;
    // クリックイベント定義.
    public class ClickEvent : UnityEvent<int> { };
    // クリックイベント.
    public ClickEvent OnClicked = new ClickEvent();
    // ボタンインデックス.
    public int buttonIndex = 0;

    void Start()
    {
    }

    // ------------------------------------------------------------
    // 作成時コール.
    // ------------------------------------------------------------
    public async UniTask OnCreated(string txt, int index, UnityAction<int> onClick)
    {
        transition.Canvas.alpha = 0;
        buttonText.text = txt;
        buttonIndex = index;
        OnClicked.AddListener(onClick);

        await transition.TransitionInWait();
    }

    // ------------------------------------------------------------
    // 閉じる.
    // ------------------------------------------------------------
    public async UniTask Close()
    {
        await transition.TransitionOutWait();
        Destroy(gameObject);
    }

    // ------------------------------------------------------------
    // ボタンクリックコールバック.
    // ------------------------------------------------------------
    public void OnButtonClicked()
    {
        OnClicked.Invoke(buttonIndex);
    }
}