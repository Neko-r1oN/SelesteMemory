using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBase : MonoBehaviour
{
    // 初期ビューインデックス.
    [SerializeField] protected int initialViewIndex = 0;
    // 初期ビュートランジションフラグ.
    [SerializeField] protected bool isInitialTransition = true;
    // Viewリスト.
    [SerializeField] protected List<ViewBase> viewList = new List<ViewBase>();
    // 現在のビュー.
    protected ViewBase currentView = null;

    protected virtual void Start()
    {
    }
}