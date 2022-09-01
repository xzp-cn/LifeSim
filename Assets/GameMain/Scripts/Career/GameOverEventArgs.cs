using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class GameOverEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(GameOverEventArgs).GetHashCode();

    public GameOverEventArgs()
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

    public static GameOverEventArgs Create(object args)
    {
        GameOverEventArgs gameOverEventArgs = ReferencePool.Acquire<GameOverEventArgs>();
        gameOverEventArgs.UserData = args;
        return gameOverEventArgs;
    }


    public override void Clear()
    {

    }
}
