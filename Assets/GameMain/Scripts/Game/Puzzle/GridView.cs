using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using GameFramework.Event;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class GridView 
{
    public Transform par;
    public GameObject item;
    private IObjectPool<GridItemObject> m_ObjectPool;
    public List<GameObject> m_GridUiItems;
    public void OnInit(Transform _par, GameObject _item)
    {
        par = _par;
        item = _item;
        bool exist= GameEntry.ObjectPool.HasObjectPool<GridItemObject>("Grid");
        if (!exist)
        {
           m_ObjectPool= GameEntry.ObjectPool.CreateSingleSpawnObjectPool<GridItemObject>("Grid");
        }
        m_GridUiItems=new List<GameObject>();
    }

    public void OnOpen(GridItem[,] gridArr)
    {
        for (int i = 0; i < gridArr.GetLength(0); i++)
        {
            for (int j = 0; j < gridArr.GetLength(1); j++)
            {
                GameObject go =GetItemObj();
                RectTransform rt = go.GetComponent<RectTransform>();
                Vector3 pos = new Vector3(i * rt.rect.width + i * 2f, -j * rt.rect.height - j * 2f, 0);
                rt.anchoredPosition3D = pos;
                go.name = string.Format("{0},{1}", i, j);
                m_GridUiItems.Add(go);
            }
        }
        //初始化资源
        RectTransform _rt = item.GetComponent<RectTransform>();
        float offsetX = gridArr.GetLength(0) / 2 * _rt.rect.size.x;
        float offsetY = gridArr.GetLength(1) / 2 * _rt.rect.size.y;
        _rt = par as RectTransform;
        _rt.anchoredPosition += new Vector2(-offsetX, offsetY);

        GameEntry.Event.Subscribe(GridUILocateEventArgs.EventId,GetOffset);
    }

    GameObject GetItemObj()
    {
        GridItemObject gridItemObject= m_ObjectPool.Spawn();
        if (gridItemObject!=null)
        {
            return (GameObject)gridItemObject.Target;
        }
        else
        {
            GameObject uiItem= GameObject.Instantiate(item);
            uiItem.transform.SetParent(par);
            m_ObjectPool.Register(GridItemObject.Create(uiItem), true);
            return uiItem;
        }
    }

    public void OnClose()
    {
        GameEntry.Event.Unsubscribe(GridUILocateEventArgs.EventId, GetOffset);
        foreach (GameObject _gridItem in m_GridUiItems)
        {
            m_ObjectPool.Unspawn(_gridItem);
        }
        m_GridUiItems.Clear();
    }

    /// <summary>
    /// 吸附，重叠
    /// </summary>
    /// <returns></returns>
    public void GetOffset(object sender,GameEventArgs args)
    {
        GridArgs gridArgs = (GridArgs)((GridUILocateEventArgs) args).UserData;

        RectTransform hitRectTransform = null;
        foreach (Transform _transform in par)
        {
            RectTransform rt = _transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, gridArgs.data.position))
            {
                hitRectTransform = rt;
            }
        }

        if (hitRectTransform==null)
        {
            Log.Debug("没有格子");
            return;
        }
        Vector3 offset = hitRectTransform.position - gridArgs.item.transform.position;
        gridArgs.item.rt.position += offset;

        foreach (RectTransform _iteRectTransform in gridArgs.item.rt)
        {
            if (_iteRectTransform== gridArgs.item.rt)
            {
                continue;
            }
            Vector2 screenPos=RectTransformUtility.WorldToScreenPoint(null, _iteRectTransform.position);
            Ray ray = RectTransformUtility.ScreenPointToRay(null, screenPos);
            RaycastHit[] raycastHits=Physics.RaycastAll(ray);
            if (raycastHits!=null&&raycastHits.Length>=2)
            {
                Debug.Log("有重叠");
                gridArgs.item.ResetPos();
                break;
            }
        }
       
    }

    /// <summary>
    /// 结算
    /// </summary>
    public bool Settle()
    {
        bool isFinish=true;
        foreach (RectTransform _itemTransform in par)
        {
            if (_itemTransform==par)
            {
                continue;
            }

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, _itemTransform.position);
            Ray ray = RectTransformUtility.ScreenPointToRay(null, screenPos);
            Debug.DrawRay(ray.origin,ray.direction*100,Color.red,100);
            RaycastHit[] raycastHits = Physics.RaycastAll(ray);
            if (raycastHits != null && raycastHits.Length < 1)
            {
                Debug.Log("空格");
                isFinish = false;
                break;
            }
        }

        return isFinish;
    }
}
