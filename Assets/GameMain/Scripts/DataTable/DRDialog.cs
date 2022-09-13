//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2022-09-13 14:10:45.201
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
    /// 对话。
    /// </summary>
    public class DRDialog : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取id编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取对话内容。
        /// </summary>
        public string Content
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取说话人。
        /// </summary>
        public int RoleId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取角色位置。
        /// </summary>
        public int Pos
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否手机聊天。
        /// </summary>
        public bool WeChat
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
            Content = columnStrings[index++];
            RoleId = int.Parse(columnStrings[index++]);
            Pos = int.Parse(columnStrings[index++]);
            WeChat = bool.Parse(columnStrings[index++]);

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
                    Content = binaryReader.ReadString();
                    RoleId = binaryReader.Read7BitEncodedInt32();
                    Pos = binaryReader.Read7BitEncodedInt32();
                    WeChat = binaryReader.ReadBoolean();
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
