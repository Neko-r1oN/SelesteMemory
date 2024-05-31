using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase : MonoBehaviour
{
    // ビューのトランジション.
    UITransition transition = null;
    // ビューのトランジション.
    public UITransition Transition
    {
        get
        {
            if (transition == null) transition = GetComponent<UITransition>();
            return transition;
        }
    }
    // シーンベースクラス.
    public SceneBase Scene = null;


    // -------------------------------------------------------
    // ビューオープン時コール.
    // -------------------------------------------------------
    public virtual void OnViewOpened()
    {
        // Debug.Log( "View Open" );
    }

    // -------------------------------------------------------
    // ビュークローズ時コール.
    // -------------------------------------------------------
    public virtual void OnViewClosed()
    {
        // Debug.Log( "View Close" );
    }
}