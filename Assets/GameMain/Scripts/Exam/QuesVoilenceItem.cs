using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class QuesVoilenceItem : QuesItemBase
{
    public int m_nextQuesId;

    public override void OnInit(Toggle m_toggle,string _stem)
    {
        base.OnInit(m_toggle,_stem);
    }
    public override void OnOpen()
    {
        base.OnOpen();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown,userData);
    }

    public override void OnRecycle()
    {
        base.OnRecycle();
    }

}

public enum QuesType
{
    /// <summary>
    /// 抑郁症
    /// </summary>
    Depression=1,
    /// <summary>
    /// 暴力测试
    /// </summary>
    Violent=2,
    /// <summary>
    /// 职业分析
    /// </summary>
    Career=3,
};
