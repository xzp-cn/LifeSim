using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using UnityEngine;

public class TestEventsArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(TestEventsArgs).GetHashCode();
    // Start is called before the first frame update

    public TestEventsArgs()
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

    public static TestEventsArgs Create(string args)
    {
        TestEventsArgs testEventsArgs = ReferencePool.Acquire<TestEventsArgs>();
        testEventsArgs.UserData = (object)args;
        return testEventsArgs;
    }

    public override void Clear()
    {

    }
}
