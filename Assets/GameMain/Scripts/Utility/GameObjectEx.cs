﻿using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;

public static class GameObjectEx
{
    public static bool IsHighLighter(this GameObject go)
    {
        Highlighter highlighter = go.GetComponent<Highlighter>();
        if (highlighter==null)
        {
            go.AddComponent<Highlighter>();
        }
        bool ishilight = highlighter.constant || highlighter.tween;
        return ishilight;
    }

    /// <summary>
    /// 相机需要添加highlighterRender脚本，模型高亮
    /// </summary>
    /// <param name="go"></param>
    public static void OpenHighLighterFlash(this GameObject go)
    {
        Highlighter highlighter = go.GetComponent<Highlighter>();
        if (highlighter == null)
        {
           highlighter= go.AddComponent<Highlighter>();
        }
        GradientColorKey[] colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(){color = new Color(0,255,255),time = 0},
            new GradientColorKey(){color = new Color(129,255,0),time = 0.318f},
            new GradientColorKey(){color = new Color(209,89,13),time = 0.644f},
            new GradientColorKey(){color = new Color(17,25,243),time = 0.838f},
            new GradientColorKey(){color = new Color(226,0,255),time = 0.838f}
        };
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
        {
            new GradientAlphaKey(){alpha =0 ,time = 0},
            new GradientAlphaKey(){alpha =1 ,time = 1f},
        };


        highlighter.tween = true;
        highlighter.tweenDuration = 0.2f;
    }
    public static void OpenHighLighterConstant(this GameObject go, Color c)
    {
        Highlighter highlighter = go.GetComponent<Highlighter>();
        if (highlighter == null)
        {
            highlighter= go.AddComponent<Highlighter>();
        }
        highlighter.constantColor = c;
        highlighter.constant = true;
    }


    public static void CloseHighLighter(this GameObject go)
    {
        Highlighter highlighter = go.GetComponent<Highlighter>();
        if (highlighter == null)
        {
            go.AddComponent<Highlighter>();
        }
        highlighter.tween = false;
        highlighter.constant = false;
    }
}
