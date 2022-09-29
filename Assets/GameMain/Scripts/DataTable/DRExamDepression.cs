﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-29 16:54:38.767
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
    /// 暴力倾向试题。
    /// </summary>
    public class DRExamDepression : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取题目编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取题干。
        /// </summary>
        public string QuestionStem
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取选项A。
        /// </summary>
        public string OptionA
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取选项B。
        /// </summary>
        public string OptionB
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取1。
        /// </summary>
        public int OptionScoreA
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取0。
        /// </summary>
        public int OptionScoreB
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
            QuestionStem = columnStrings[index++];
            OptionA = columnStrings[index++];
            OptionB = columnStrings[index++];
            OptionScoreA = int.Parse(columnStrings[index++]);
            OptionScoreB = int.Parse(columnStrings[index++]);

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
                    QuestionStem = binaryReader.ReadString();
                    OptionA = binaryReader.ReadString();
                    OptionB = binaryReader.ReadString();
                    OptionScoreA = binaryReader.Read7BitEncodedInt32();
                    OptionScoreB = binaryReader.Read7BitEncodedInt32();
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
