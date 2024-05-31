using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
public class UITransition : MonoBehaviour
{
    // レクトトランスフォーム取得用.
    public RectTransform Rect
    {
        get
        {
            if (rect == null) rect = GetComponent<RectTransform>();
            return rect;
        }
    }
    // レクトトランスフォーム保管用.
    RectTransform rect = null;

    // 設定値.
    [System.Serializable]
    public class TransitionParam
    {
        // 実行フラグ.
        public bool IsActive = true;
        // インの値.
        public Vector2 In = new Vector2(0, 1f);
        // アウトの値.
        public Vector2 Out = new Vector2(1f, 0);
    }

    // フェード設定値.
    [SerializeField] TransitionParam fade = new TransitionParam();
    // スケール設定値.
    [SerializeField] TransitionParam scale = new TransitionParam() { IsActive = false, In = Vector2.zero, Out = Vector2.zero };
    // 遷移時間.
    [SerializeField] float duration = 1f;

    //! インのシークエンス.
    Sequence inSequence = null;
    //! アウトのシークエンス.
    Sequence outSequence = null;

    //! インのキャンセルトークン.
    CancellationTokenSource inCancellation = null;
    //! アウトのキャンセルトークン.
    CancellationTokenSource outCancellation = null;

    // キャンバスグループ取得用.
    public CanvasGroup Canvas
    {
        get
        {
            if (canvas == null) canvas = GetComponent<CanvasGroup>();
            return canvas;
        }
    }

    // キャンバスグループ保管用.
    CanvasGroup canvas = null;

    void Start()
    {
    }

    // ----------------------------------------------------
    // トランジションイン.
    // ----------------------------------------------------
    public void TransitionIn(UnityAction onCompleted = null)
    {
        if (inSequence != null)
        {
            inSequence.Kill();
            inSequence = null;
        }
        inSequence = DOTween.Sequence();

        if (fade.IsActive == true && Canvas != null)
        {
            Canvas.alpha = fade.In.x;

            inSequence.Join
            (
                Canvas.DOFade(fade.In.y, duration)
                .SetLink(gameObject)
            );
        }
        if (scale.IsActive == true)
        {
            var current = Rect.transform.localScale;
            Rect.transform.localScale = new Vector3(scale.In.x, scale.In.y, current.z);

            inSequence.Join
            (
                Rect.DOScale(current, duration)
                .SetLink(gameObject)
            );
        }

        inSequence
        .SetLink(gameObject)
        .OnComplete(() => onCompleted?.Invoke());
    }

    // ----------------------------------------------------
    // トランジションアウト.
    // ----------------------------------------------------
    public void TransitionOut(UnityAction onCompleted = null)
    {
        if (outSequence != null)
        {
            outSequence.Kill();
            outSequence = null;
        }
        outSequence = DOTween.Sequence();

        if (fade.IsActive == true && Canvas != null)
        {
            Canvas.alpha = fade.Out.x;

            outSequence.Join
            (
                Canvas.DOFade(fade.Out.y, duration)
                .SetLink(gameObject)
            );
        }

        if (scale.IsActive == true)
        {
            var current = Rect.transform.localScale;
            outSequence.Join
            (
                Rect.DOScale(new Vector3(scale.Out.x, scale.Out.y, current.z), duration)
                .SetLink(gameObject)
                .OnComplete(() => Rect.transform.localScale = current)
            );
        }

        outSequence
        .SetLink(gameObject)
       .OnComplete(() => onCompleted?.Invoke());
    }

    // ----------------------------------------------------
    // トランジションイン終了待機.
    // ----------------------------------------------------
    public async UniTask TransitionInWait()
    {
        bool isDone = false;
        if (inCancellation != null)
        {
            inCancellation.Cancel();
        }
        inCancellation = new CancellationTokenSource();

        TransitionIn(() => { isDone = true; });

        try
        {
            await UniTask.WaitUntil(() => isDone == true, PlayerLoopTiming.Update, inCancellation.Token);
        }
        catch (System.OperationCanceledException e)
        {
            Debug.Log("キャンセルされました。" + e);
        }

    }

    // ----------------------------------------------------
    // トランジションアウト終了待機.
    // ----------------------------------------------------
    public async UniTask TransitionOutWait()
    {
        bool isDone = false;
        if (outCancellation != null)
        {
            outCancellation.Cancel();
        }
        outCancellation = new CancellationTokenSource();

        TransitionOut(() => { isDone = true; });

        try
        {
            await UniTask.WaitUntil(() => isDone == true, PlayerLoopTiming.Update, outCancellation.Token);
        }
        catch (System.OperationCanceledException e)
        {
            Debug.Log("キャンセルされました。" + e);
        }

    }

    // ----------------------------------------------------
    // 破棄された時のコールバック.
    // ----------------------------------------------------
    void OnDestroy()
    {
        if (inCancellation != null)
        {
            inCancellation.Cancel();
        }
        if (outCancellation != null)
        {
            outCancellation.Cancel();
        }
    }

}