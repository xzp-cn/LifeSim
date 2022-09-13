﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.ObjectPool;
using StarForce;
using UnityEngine;
using UnityEngine.UI;

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
        ).onComplete= () =>
        {
           IObjectPool<TipBarItemObject> tipBarItemObject=  GameEntry.ObjectPool.GetObjectPool<TipBarItemObject>("tipBar");//
           tipBarItemObject.Unspawn(this);
        };

        img.DoAlpha(0,_duration);
        m_Text.DoAlpha(0,_duration);
    }

    public void ResetVal()
    {
        img.color=new Color(1,1,1, 0.3882353f);
        m_Text.color=new Color(1,1,1,1);
    }
}
