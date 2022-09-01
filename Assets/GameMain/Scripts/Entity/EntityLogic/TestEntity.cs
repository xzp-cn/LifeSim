using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using  UnityGameFramework.Runtime;

namespace StarForce
{
    public class TestEntity : Entity
    {
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            CachedTransform.Rotate(Vector3.forward,Time.deltaTime*10);
            
        }
    }
}

