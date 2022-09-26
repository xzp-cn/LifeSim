using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class PlayerMoveOnEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(PlayerMoveOnEventArgs).GetHashCode();
    // Start is called before the first frame update

    public PlayerMoveOnEventArgs()
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

    public static GameEventArgs Create(VarBoolean args)
    {
        PlayerMoveOnEventArgs playerMoveOnEventArgs = ReferencePool.Acquire<PlayerMoveOnEventArgs>();
        playerMoveOnEventArgs.UserData = (object)args;
        return playerMoveOnEventArgs;
    }

    public override void Clear()
    {
        UserData = null;
    }
}
