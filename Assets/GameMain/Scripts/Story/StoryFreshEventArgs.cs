using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class StoryFreshEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(StoryFreshEventArgs).GetHashCode();
    // Start is called before the first frame update

    public StoryFreshEventArgs()
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

    public static StoryFreshEventArgs Create(string args)
    {
        StoryFreshEventArgs storyFreshEventArgs = ReferencePool.Acquire<StoryFreshEventArgs>();
        storyFreshEventArgs.UserData = (object)args;
        return storyFreshEventArgs;
    }

    public override void Clear()
    {

    }
}

