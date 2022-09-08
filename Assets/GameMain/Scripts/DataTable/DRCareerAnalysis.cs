﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-08 11:17:10.177
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
    /// 职场分析表。
    /// </summary>
    public class DRCareerAnalysis : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取职业类型。
        /// </summary>
        public string CareerType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取职业描述。
        /// </summary>
        public string CareerDes
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取共同特点。
        /// </summary>
        public string Common
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取性格特点。
        /// </summary>
        public string Personality
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取职业建议。
        /// </summary>
        public string Suggestion
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
            CareerType = columnStrings[index++];
            CareerDes = columnStrings[index++];
            Common = columnStrings[index++];
            Personality = columnStrings[index++];
            Suggestion = columnStrings[index++];

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
                    CareerType = binaryReader.ReadString();
                    CareerDes = binaryReader.ReadString();
                    Common = binaryReader.ReadString();
                    Personality = binaryReader.ReadString();
                    Suggestion = binaryReader.ReadString();
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
