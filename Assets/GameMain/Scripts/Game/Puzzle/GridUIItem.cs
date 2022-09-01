using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StarForce;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 offset;
    public RectTransform rt;

    private Vector3 originPos;
    private void Start()
    {
        rt = transform.parent as RectTransform;
        originPos=rt.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = eventData.position - rt.anchoredPosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rt.SetAsLastSibling();
        rt.anchoredPosition = eventData.position - offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameEntry.Event.Fire(GridUILocateEventArgs.EventId,GridUILocateEventArgs.Create(new GridArgs()
        {
            item = this,
            data = eventData
        }));
        //RectTransform _rt = FindObjectOfType<GridView>().GetOffset(eventData.position);
        //if (_rt == null)
        //{
        //    Debug.LogError("没有格子");
        //    return;

        //}
        //Vector3 offset = _rt.position - transform.position;
        //rt.position += offset;
    }

    public void ResetPos()
    {
        rt.DOLocalMove(originPos, 0.5f);//
    }

}

public class GridArgs
{
    public GridUIItem item;
    public PointerEventData data;
}
