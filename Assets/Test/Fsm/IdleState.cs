using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;


public class IdleState : FsmState<ActorOwner>
{
    // Start is called before the first frame update
    protected override void OnInit(IFsm<ActorOwner> fsm)
    {
        Log.Debug("IdleState::OnInit");
    }

    /// <summary>
    /// 有限状态机状态进入时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnEnter(IFsm<ActorOwner> fsm)
    {
        Log.Debug("IdleState::OnEnter进入" + fsm.CurrentStateTime);
    }

    protected override void OnUpdate(IFsm<ActorOwner> fsm, float elapseSeconds, float realElapseSeconds)
    {
        if (Input.GetKeyDown(KeyCode.M))//跳转
        {
            ChangeState<MoveState>(fsm);
            Log.Debug("更新时间  IdleState::OnUpdate");
        }
    }
}
