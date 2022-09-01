using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class MoveState : FsmState<ActorOwner>
{
    protected override void OnInit(IFsm<ActorOwner> fsm)
    {
        Log.Debug("MoveState::OnInit初始化 " + fsm.Name);
    }

    /// <summary>
    /// 有限状态机状态进入时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnEnter(IFsm<ActorOwner> fsm)
    {
        Log.Debug("MoveState::OnEnter进入 " + fsm.CurrentState);
    }

    protected override void OnUpdate(IFsm<ActorOwner> fsm, float elapseSeconds, float realElapseSeconds)
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeState<IdleState>(fsm);
        }
    }
}
