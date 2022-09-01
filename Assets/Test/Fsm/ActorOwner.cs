using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

public class ActorOwner 
{
    //private IFsm<ActorOwner> m_Fsm = null;

    public ActorOwner()
    {
        FsmComponent fsmComponent = GameEntry.GetComponent<FsmComponent>();

        // 参数传入有限状态机名称、拥有者和所有需要的状态
       // m_Fsm = fsmComponent.CreateFsm("ActorFsm", this, new IdleState(), new MoveState());
    }
}

