//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-30 09:39:49.985
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
    public class DRSceneContent : DataRowBase
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
        /// 获取剧情概述。
        /// </summary>
        public string StorySummary
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

        /// <summary>
        /// 获取抑郁症Id。
        /// </summary>
        public string ExamDepressArr
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取职业Id。
        /// </summary>
        public string ExamCareerArr
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取暴力Id。
        /// </summary>
        public string ExamVoilenceArr
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取任务提示。
        /// </summary>
        public string TaskTip
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取位置。
        /// </summary>
        public string PosArr
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
            StorySummary = columnStrings[index++];
            SpeakAsideIdArray = columnStrings[index++];
            SceneBG = columnStrings[index++];
            LocateName = columnStrings[index++];
            ExamDepressArr = columnStrings[index++];
            ExamCareerArr = columnStrings[index++];
            ExamVoilenceArr = columnStrings[index++];
            TaskTip = columnStrings[index++];
            PosArr = columnStrings[index++];

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
                    StorySummary = binaryReader.ReadString();
                    SpeakAsideIdArray = binaryReader.ReadString();
                    SceneBG = binaryReader.ReadString();
                    LocateName = binaryReader.ReadString();
                    ExamDepressArr = binaryReader.ReadString();
                    ExamCareerArr = binaryReader.ReadString();
                    ExamVoilenceArr = binaryReader.ReadString();
                    TaskTip = binaryReader.ReadString();
                    PosArr = binaryReader.ReadString();
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
