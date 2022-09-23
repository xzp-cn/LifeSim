using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class TipBar : MonoBehaviour
{
    private RectTransform rt;
    private Image img;
    private Text m_Text;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponentInChildren<Image>();
        m_Text = GetComponentInChildren<Text>();
    }

    public void Show(string content,float _duration)
    {
        if (rt==null)
        {
            Start();
        }
        ResetVal();
        m_Text.text =content;

        //回收
        rt.anchoredPosition=new Vector2(0,0);
        float m = 0;

       var tw= DOTween.To(
            () => { return m; },
            (t) => { },
            0,
            1.5f
        );
       tw.onComplete = () =>
        {
             DOTween.To(
                () =>
                {
                    return m;
                },
                (t) =>
                {
                    rt.anchoredPosition = new Vector2(0, t);
                },
                500,
                _duration
            ).onComplete = () =>
            {
               gameObject.SetActive(false);
            };
            img.DoAlpha(0, _duration);
            m_Text.DoAlpha(0, _duration);
        };
       tw.onKill = () =>
       {
           gameObject.SetActive(false);
       };

    }

    public void ResetVal()
    {
        img.color=new Color(1,1,1, 0.3882353f);
        m_Text.color=new Color(1,1,1,1);
    }

    void Recycle()
    {
        IObjectPool<TipBarItemObject> tipBarItemObject = GameEntry.ObjectPool.GetObjectPool<TipBarItemObject>("tipBar");//
        tipBarItemObject.Unspawn(this);
        
    }
    private void OnDisable()
    {
       Recycle();
    }
}
