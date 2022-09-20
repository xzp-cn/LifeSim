//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-19 10:58:49.679
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
    /// ʰȡ��Ʒ��。
    /// </summary>
    public class DRTreasure : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取��Ʒ���。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取��Ʒ����。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取������ƷId。
        /// </summary>
        public int BagId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取��Ʒ�������。
        /// </summary>
        public string Model
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取��ʧ��Ч���。
        /// </summary>
        public int ClickEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取���������。
        /// </summary>
        public int SoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取����。
        /// </summary>
        public int TypeId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取����ֵ。
        /// </summary>
        public int Power
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取����id。
        /// </summary>
        public int StroyID
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
            Power = int.Parse(columnStrings[index++]);
            StroyID = int.Parse(columnStrings[index++]);

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
                    Power = binaryReader.Read7BitEncodedInt32();
                    StroyID = binaryReader.Read7BitEncodedInt32();
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
