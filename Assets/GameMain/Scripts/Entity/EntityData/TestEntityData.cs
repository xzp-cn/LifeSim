using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    public class TestEntityData : AccessoryObjectData
    {
        [SerializeField]
        private float m_Speed = 0f;

        public TestEntityData(int entityId, int typeId, int ownerId, CampType ownerCamp)
            : base(entityId, typeId, ownerId, ownerCamp)
        {
            IDataTable<DRTest> dtThruster = GameEntry.DataTable.GetDataTable<DRTest>();
            DRTest drThruster = dtThruster.GetDataRow(TypeId);
            if (drThruster == null)
            {
                return;
            }
        }

        /// <summary>
        /// 速度。
        /// </summary>
        public float Speed
        {
            get
            {
                return m_Speed;
            }
        }
    }
}

