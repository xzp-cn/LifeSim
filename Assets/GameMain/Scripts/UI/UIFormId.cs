//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace StarForce
{
    /// <summary>
    /// 界面编号。
    /// </summary>
    public enum UIFormId : byte
    {
        Undefined = 0,

        /// <summary>
        /// 弹出框。
        /// </summary>
        DialogForm = 1,

        /// <summary>
        /// 首页。
        /// </summary>
        Life_HomePage = 100,

        /// <summary>
        /// 人物画像。
        /// </summary>
        Life_portraitOfMan = 101,

        /// <summary>
        /// 关于。
        /// </summary>
        AboutForm = 102,
        
        /// <summary>
        /// 测试UI
        /// </summary>
        //TestUIForm = 103,

        SettingForm=103,

        /// <summary>
        /// 正能量面板
        /// </summary>
        Life_PositiveForm=104,

        /// <summary>
        /// 职业面板
        /// </summary>
        Life_CareerForm = 105,

        /// <summary>
        /// 游戏面板
        /// </summary>
        Life_GameForm = 106,

    }
}
