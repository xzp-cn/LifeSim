using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StoryLinePosition : MonoBehaviour
{

    public ScrollRect m_ScrollRect;
    private float delta=0.06f;
    private float tarPos = 0;
    private const float timeDelay = 1;
    private  float timeCurrent = 0;
    private bool isClicked = false;
    public bool isToRight = true;
    private Image img;
    public Sprite normalSprite, graySprite;
    private void Start()
    {
        tarPos = m_ScrollRect.horizontalNormalizedPosition;
        img=GetComponent<Image>();
    }

    /// <summary>
    /// 按下
    /// </summary>
    public void OnClickDown()
    {
        delta = isToRight?0.1f:-0.1f;
        tarPos += delta;
        tarPos=Mathf.Clamp(tarPos, 0, 1);
        isClicked = true;
    }

    /// <summary>
    /// 改变状态
    /// </summary>
    public void ChangeState()
    {
        bool isEnd = false;
        if (isToRight)
        {
            isEnd = Mathf.Abs(m_ScrollRect.horizontalNormalizedPosition-1)<0.01f;
        }
        else
        {
            isEnd = Mathf.Abs(m_ScrollRect.horizontalNormalizedPosition)<0.01f;
        }
        img.sprite = isEnd ? graySprite : normalSprite;
    }
    void Update()
    {
        if (!isClicked)
        {
            return;
        }

        if (timeCurrent < timeDelay)
        {
            timeCurrent += Time.deltaTime;
            float normalPos = m_ScrollRect.horizontalNormalizedPosition;
            m_ScrollRect.horizontalNormalizedPosition = Mathf.Lerp(normalPos, tarPos, timeCurrent);
            tarPos = m_ScrollRect.horizontalNormalizedPosition;
        }
        else
        {
            timeCurrent = 0;
            isClicked = false;
        }
    }
}
