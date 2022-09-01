using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class StoryEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(StoryEventArgs).GetHashCode();
    // Start is called before the first frame update

    public StoryEventArgs()
    {
        UserData = null;
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

    public static GameEventArgs Create(VarInt32 args)
    {
        StoryEventArgs storyEventArgs = ReferencePool.Acquire<StoryEventArgs>();
        storyEventArgs.UserData = args;
        return storyEventArgs;
    }

    public override void Clear()
    {
        UserData = null;
    }
}
