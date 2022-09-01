using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class GameShowEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(GameShowEventArgs).GetHashCode();
    // Start is called before the first frame update

    public GameShowEventArgs()
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

    public static GameShowEventArgs Create(GameId args)
    {
        GameShowEventArgs gameShowEventArgs = ReferencePool.Acquire<GameShowEventArgs>();
        gameShowEventArgs.UserData = (object)args;
        return gameShowEventArgs;
    }

    public override void Clear()
    {

    }
}
