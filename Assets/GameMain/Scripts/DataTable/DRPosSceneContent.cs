//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-29 16:54:38.771
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
    /// 场景表。
    /// </summary>
    public class DRPosSceneContent : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取场景编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取剧情。
        /// </summary>
        public string StoryName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取旁白id数组。
        /// </summary>
        public string SpeakAsideIdArray
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取场景对话背景。
        /// </summary>
        public string SceneBG
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取校园位置。
        /// </summary>
        public string LocateName
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
            StoryName = columnStrings[index++];
            SpeakAsideIdArray = columnStrings[index++];
            SceneBG = columnStrings[index++];
            LocateName = columnStrings[index++];

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
                    StoryName = binaryReader.ReadString();
                    SpeakAsideIdArray = binaryReader.ReadString();
                    SceneBG = binaryReader.ReadString();
                    LocateName = binaryReader.ReadString();
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
