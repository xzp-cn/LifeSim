using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class QuesPanelDrag : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    private Vector2 offset;
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos= eventData.position;
        pos.z = 0;
        transform.position = pos+(Vector3)offset;

        DragRangeLimit(transform);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset =(Vector2)transform.position - eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
            
    }

    public void DragRangeLimit(Transform tra)
    {
        var pos = tra.GetComponent<RectTransform>();
        float x = Mathf.Clamp(pos.position.x, pos.rect.width * 0.5f, Screen.width - (pos.rect.width * 0.5f));
        float y = Mathf.Clamp(pos.position.y, pos.rect.height * 0.5f, Screen.height - (pos.rect.height * 0.5f));
        pos.position = new Vector2(x, y);
    }
}
