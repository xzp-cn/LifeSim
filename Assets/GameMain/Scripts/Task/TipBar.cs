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
        StopAllCoroutines();
        StartCoroutine(AnimBar(_duration));
    }

    IEnumerator AnimBar(float _duration)
    {
        yield return new WaitForSeconds(1f);
        float t = 0;
        float h = 0;
        //_duration = 6;
        //img.DoAlpha(0, 2);
        //m_Text.DoAlpha(0, 2);
        Color cImg = img.color;
        Color cText = m_Text.color;
        WaitForEndOfFrame wf=new WaitForEndOfFrame();
        while (t<=_duration)
        {
            h=Mathf.Lerp(h, 500,Mathf.Sin((float)(Math.PI/2*(t/_duration))));
            rt.anchoredPosition = new Vector2(0, h);
            //
            float alpha = Mathf.Lerp(cImg.a, 0, t / _duration);
            cImg.a = alpha;
            img.color = cImg;
            //
            cText.a = alpha;
            m_Text.color = cText;
            //
            t += Time.deltaTime;
            yield return wf;
        }
        Recycle();
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
}
