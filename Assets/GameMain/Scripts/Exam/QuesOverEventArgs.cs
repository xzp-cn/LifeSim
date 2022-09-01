using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class QuesOverEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(QuesOverEventArgs).GetHashCode();

    public QuesOverEventArgs()
    {

    }

    /// <summary>
    /// 获取用户自定义数据。
    /// </summary>
    public object UserData { get; private set; }

    public override int Id
    {
        get { return EventId; }
    }

    public static QuesOverEventArgs Create(VarInt32 args)
    {
        QuesOverEventArgs overEventArgs = ReferencePool.Acquire<QuesOverEventArgs>();
        overEventArgs.UserData = (object)args;
        return overEventArgs;
    }


    public override void Clear()
    {

    }
}