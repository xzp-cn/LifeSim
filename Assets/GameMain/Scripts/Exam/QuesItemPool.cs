using System.Collections;
using System.Collections.Generic;
using GameFramework.ObjectPool;
using StarForce;
using UnityEngine;

public class QuesItemPool 
{
    public GameObject m_twoItemTemplate;
    public GameObject m_threeItemTemplate;
    private IObjectPool<QuesItemPoolObject> twoObjectPool;
    private IObjectPool<QuesItemPoolObject> threetObjectPool;
    List<GameObject> twoObjectPoolList = new List<GameObject>();
    List<GameObject> threeObjectPoolList = new List<GameObject>();

    private Transform par;
    public QuesItemPool(Transform _par,GameObject _twoItemTemplate,GameObject _threetItemTemplate)
    {
        par = _par;
        m_twoItemTemplate= _twoItemTemplate;
        m_threeItemTemplate = _threetItemTemplate; 
        
        //
        int m_InstancePoolCapacity = 10;
        twoObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<QuesItemPoolObject>("2", m_InstancePoolCapacity);
        threetObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<QuesItemPoolObject>("3", m_InstancePoolCapacity);
        twoObjectPoolList.Clear();
        threeObjectPoolList.Clear();
    }

    public GameObject GetTwoPoolObj()
    {
        GameObject twoItem = null;
        QuesItemPoolObject twoItemObject = twoObjectPool.Spawn();
        if (twoItemObject != null)
        {
            twoItem = (GameObject)twoItemObject.Target;
        }
        else
        {
            twoItem = Object.Instantiate(m_twoItemTemplate);
            Transform _transform = twoItem.GetComponent<Transform>();
            _transform.SetParent(par);
            _transform.localScale = Vector3.one;
            twoObjectPool.Register(QuesItemPoolObject.Create(twoItem), true);
        }
        twoObjectPoolList.Add(twoItem);
        return twoItem;
    }
    public void RecycleTwoPoolObj()
    {
        for (int i = twoObjectPoolList.Count-1; i >=0; i--)
        {
            twoObjectPool.Unspawn(twoObjectPoolList[i]);
        }
        twoObjectPoolList.Clear();
    }

    public GameObject GetThreePoolObj()
    {
        GameObject threeItem = null;
        QuesItemPoolObject threeItemObject = threetObjectPool.Spawn();
        if (threeItemObject != null)
        {
            threeItem = (GameObject)threeItemObject.Target;
        }
        else
        {
            threeItem = Object.Instantiate(m_threeItemTemplate);
            Transform _transform = threeItem.GetComponent<Transform>();
            _transform.SetParent(par);
            _transform.localScale = Vector3.one;
            threetObjectPool.Register(QuesItemPoolObject.Create(threeItem), true);
        }
        threeObjectPoolList.Add(threeItem);
        return threeItem;
    }
    public void RecycleThreePoolObj()
    {
        for (int i = threeObjectPoolList.Count - 1; i >= 0; i--)
        {
            threetObjectPool.Unspawn(threeObjectPoolList[i]);
            
        }
        threeObjectPoolList.Clear();
    }

    public void UnspawnAllObj()
    {
        RecycleTwoPoolObj();
        RecycleThreePoolObj();
    }

    public void ReleaseAll()
    {
        //threetObjectPool.Release();
        //twoObjectPool.Release();
    }
}
