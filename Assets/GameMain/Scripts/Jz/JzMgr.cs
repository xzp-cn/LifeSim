using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.DataTable;
using GameFramework.Event;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
public class JzMgr : IUIModule
{
    private Transform m_JzTransform;
    private IDataTable<DRMessageCollege> messageTable;
    public void Init(Transform _jzTransform)    
    {
        m_JzTransform = _jzTransform;
    }

    string GetContent(int id)
    {
        messageTable = GameEntry.DataTable.GetDataTable<DRMessageCollege>();
        DRMessageCollege drMessage=messageTable.GetDataRow(id);
        string content= drMessage.Content;
        return content;
    }

    void Show(object sender,GameEventArgs args)
    {
        RectTransform mask=m_JzTransform.Find("Message/mask") as RectTransform;
        mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,85);
        mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400);
        Text text=mask.GetComponentInChildren<Text>();
        int id = (int)((JzFreshEventArgs) args).UserData;
        text.text = GetContent(id);

        m_JzTransform.Find("button_close").gameObject.SetActive(false);
        float step = mask.rect.size.x;
        DOTween.To(
            () => { return step; },
            (x) => {mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,x); },
            1685,
            2
        ).onComplete= () =>
        {
            m_JzTransform.Find("button_close").gameObject.SetActive(true);
        };
        m_JzTransform.gameObject.SetActive(true);
    }
    public void OnOpen()
    {   
        GameEntry.Event.Subscribe(JzFreshEventArgs.EventId,Show);
    }

    public void Update()
    {

    }
    public void OnRecycle()
    {

    }

    public void Close()
    {
        RectTransform mask = m_JzTransform.Find("Message/mask") as RectTransform;
        mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400);
        m_JzTransform.Find("button_close").gameObject.SetActive(false);

        float step = mask.rect.size.x;
        DOTween.To(
            () => { return step; }, 
            (x) => { mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x); },
            85,
            2
        ).onComplete = () =>
        {
            m_JzTransform.gameObject.SetActive(false);
            m_JzTransform.Find("button_close").gameObject.SetActive(false);
            GameEntry.Event.Fire(this, StoryFreshEventArgs.Create(null));
        };
    }

    public void OnClose(bool isShutdown, object userData)
    {
        GameEntry.Event.Unsubscribe(JzFreshEventArgs.EventId,Show);
    }
 
}
