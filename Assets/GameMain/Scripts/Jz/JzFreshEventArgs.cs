using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class JzFreshEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(JzFreshEventArgs).GetHashCode();
    // Start is called before the first frame update

    public JzFreshEventArgs()
    {
        
    }
    /// <summary>
    /// 获取用户自定义数据。
    /// </summary>
    public object UserData
    {
        get;
        private set;
    }

    public override int Id
    {
        get
        {
            return EventId;

        }
    }

    public static JzFreshEventArgs Create(int args)
    {
        JzFreshEventArgs jzFreshEventArgs = ReferencePool.Acquire<JzFreshEventArgs>();
        jzFreshEventArgs.UserData = (object)args;
        return jzFreshEventArgs;
    }


    public override void Clear()
    {

    }
}
