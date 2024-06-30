using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SelectButtonDialog : MonoBehaviour
{
    // 背景のトランジション.
    [SerializeField] UITransition bgTransition = null;
    // ボタンの親.
    [SerializeField] Transform buttonParent = null;
    // ボタンのプレハブ.
    [SerializeField] SelectButton buttonPrefab = null;
    // レスポンス.
    int response = -1;
    // ボタンリスト.
    List<SelectButton> buttons = new List<SelectButton>();

    void Start()
    {
    }

    // ------------------------------------------------------------
    // ボタンの生成.
    // ------------------------------------------------------------
    public async UniTask<int> CreateButtons(bool bgOpen, string[] selectParams)
    {
        if (selectParams == null || selectParams.Length == 0) return -1;

        var tasks = new List<UniTask>();
        int index = 0;

        // 背景の設定.
        if (bgOpen == true)
        {
            bgTransition.gameObject.SetActive(true);
            tasks.Add(bgTransition.TransitionInWait());
        }
        else
        {
            bgTransition.gameObject.SetActive(false);
        }

        // ボタンの生成.
        foreach (var param in selectParams)
        {
            var button = Instantiate(buttonPrefab, buttonParent);
            buttons.Add(button);
            tasks.Add(button.OnCreated(param, index, OnAnyButtonClicked));
            index++;
        }

        // レイアウトグループの確実な反映のためにキャンバスを更新.
        Canvas.ForceUpdateCanvases();

        await UniTask.WhenAll(tasks);

        // ここで何かしらのボタンが押されるまで待機.
        await UniTask.WaitUntil(() => response != -1, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
        var res = response;
        // 閉じる.
        await Close();

        return res;
    }

    // ------------------------------------------------------------
    // ダイアログを閉じる.
    // ------------------------------------------------------------
    public async UniTask Close()
    {
        // ボタンを閉じる.
        var tasks = new List<UniTask>();
        foreach (var b in buttons)
        {
            tasks.Add(b.Close());
        }
        // 背景を閉じる.
        if (bgTransition.gameObject.activeSelf == true)
        {
            tasks.Add(bgTransition.TransitionOutWait());
        }

        await UniTask.WhenAll(tasks);
        bgTransition.gameObject.SetActive(false);
        response = -1;
    }

    // ------------------------------------------------------------
    // どれかのボタンを押した時の処理.
    // ------------------------------------------------------------
    void OnAnyButtonClicked(int index)
    {
        // レスポンスを決定.
        Debug.Log(index + "をクリック");
        response = index;
    }
}
