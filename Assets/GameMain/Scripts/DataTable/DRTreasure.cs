//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-01 16:10:28.688
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    /// <summary>
    /// 拾取物品表。
    /// </summary>
    public class DRTreasure : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取物品编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取物品名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取背包物品Id。
        /// </summary>
        public int BagId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物品所属背景。
        /// </summary>
        public string Model
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取消失特效编号。
        /// </summary>
        public int ClickEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取点击声音编号。
        /// </summary>
        public int SoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取类型。
        /// </summary>
        public int TypeId
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            Name = columnStrings[index++];
            BagId = int.Parse(columnStrings[index++]);
            Model = columnStrings[index++];
            ClickEffectId = int.Parse(columnStrings[index++]);
            SoundId = int.Parse(columnStrings[index++]);
            TypeId = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Name = binaryReader.ReadString();
                    BagId = binaryReader.Read7BitEncodedInt32();
                    Model = binaryReader.ReadString();
                    ClickEffectId = binaryReader.Read7BitEncodedInt32();
                    SoundId = binaryReader.Read7BitEncodedInt32();
                    TypeId = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
