//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-30 09:39:49.981
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
    /// 角色表。
    /// </summary>
    public class DRRole : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取角色编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取角色名。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string ResName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取tts语音id。
        /// </summary>
        public int SpeakerId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取头像资源名称。
        /// </summary>
        public string HeadImg
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取关系描述。
        /// </summary>
        public string DesRelation
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取性格介绍。
        /// </summary>
        public string Personality
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
            ResName = columnStrings[index++];
            SpeakerId = int.Parse(columnStrings[index++]);
            HeadImg = columnStrings[index++];
            DesRelation = columnStrings[index++];
            Personality = columnStrings[index++];

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
                    ResName = binaryReader.ReadString();
                    SpeakerId = binaryReader.Read7BitEncodedInt32();
                    HeadImg = binaryReader.ReadString();
                    DesRelation = binaryReader.ReadString();
                    Personality = binaryReader.ReadString();
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
