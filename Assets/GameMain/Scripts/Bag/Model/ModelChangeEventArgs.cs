using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class ModelChangeEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(ModelChangeEventArgs).GetHashCode();
    // Start is called before the first frame update

    public ModelChangeEventArgs()
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

    public static ModelChangeEventArgs Create(string args)
    {
        ModelChangeEventArgs modelChangeEventArgs = ReferencePool.Acquire<ModelChangeEventArgs>();
        modelChangeEventArgs.UserData = (object)args;
        return modelChangeEventArgs;
    }


    public override void Clear()
    {

    }
}
