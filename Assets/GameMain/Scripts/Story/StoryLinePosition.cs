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
    public Sprite leftSprite, RightSprite;
    private Image img;
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
        //RectTransform rt= transform.parent as RectTransform;
        //isToRight = !isToRight;
        //float startValue = isToRight?43:1300f;
        //float endValue=isToRight? 1300f:43;
        //img.sprite = isToRight?leftSprite:RightSprite;
        //DOTween.To(
        //        () => { return startValue; },
        //        (t) => { rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, t); },
        //        endValue,
        //        1f
        //    );

        //return;
        delta = isToRight?0.1f:-0.1f;
        tarPos += delta;
        tarPos=Mathf.Clamp(tarPos, 0, 1);
        isClicked = true;
    }
    //Update is called once per frame
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
