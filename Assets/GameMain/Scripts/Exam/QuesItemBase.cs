using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuesItemBase : IUIModule
{
    /// <summary>
    /// 题干
    /// </summary>
    public string stemContent
    {
        get { return m_LabelText.text; }
        set { m_LabelText.text = value; }
    }

    public Toggle m_Toggle;
    public Text m_LabelText;

    public bool m_IsSelected;

    public virtual void OnInit(Toggle _toggle,string _stemContent)
    {
        m_Toggle = _toggle;
        m_LabelText=m_Toggle.GetComponentInChildren<Text>();
        stemContent = _stemContent;
        _toggle.onValueChanged.RemoveAllListeners();
        m_IsSelected = false;
        _toggle.onValueChanged.AddListener((isOn) => { m_IsSelected = isOn; });
    }

    public virtual void OnOpen()
    {
    }

    public virtual void Update()
    {

    }

    public virtual void OnClose(bool isShutdown, object userData)
    {

    }

    public virtual void OnRecycle()
    {

    }

 
}
