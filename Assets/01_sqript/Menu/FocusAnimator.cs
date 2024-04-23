using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FocusAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float extRate;      //ägèkÉTÉCÉY
    [SerializeField] float time;         //ägèkéûä‘

    private Tweener _tweener;
    private RectTransform _rect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(extRate, extRate, 1), "time", time, "easeType", iTween.EaseType.easeOutBack));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(1, 1, 1), "time", time, "easeType", iTween.EaseType.easeOutBack));
    }
}