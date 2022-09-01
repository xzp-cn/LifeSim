using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapLocateTip : IUIModule
{
    public Vector3 originPos;//记录原位置
    Vector2 targetPos;//简谐运动变化的位置，计算得出
    private Transform m_tipTransform;

    public float zhenFu = 10f;//振幅
    public float HZ = 2f;//频率

    private bool isOpen = false;
    public Text m_MapLocaText;
    public void Init(Transform _tipTransform)
    {
        m_tipTransform = _tipTransform;
        originPos = _tipTransform.position;

        m_MapLocaText = m_tipTransform.GetComponentInChildren<Text>(true);
    }

    public void FreshTip(string _locateName)
    {
        string locateTag = string.Empty;
        switch (_locateName)
        {
            case "caoChang":
                locateTag = "操场";
                break;
            case "tushuGuan":
                locateTag = "图书馆";
                break;
            case "shiTang":
                locateTag = "食堂";
                break;
            case "suShe":
                locateTag = "宿舍";
                break;
            case "xiaoYuan":
                locateTag = "校园";
                break;
            case "jiaoXueLou":
                locateTag = "教学楼";
                break;
            case "daMen":
                locateTag = "大门";
                break;
            case "linyinLu":
                locateTag = "林荫路";
                break;
            default:
                break;
                
        }
        m_MapLocaText.text = locateTag;
    }

    public void OnOpen()
    {
        isOpen = true;
    }

    public void Update()
    {
        if (!isOpen)
        {
            return;
        }

        targetPos = originPos;
        targetPos.y = Mathf.Sin(Time.fixedTime * Mathf.PI * HZ) * zhenFu + originPos.y;
        m_tipTransform.position = targetPos;
    }
    public void OnClose(bool isShutdown, object userData)
    {
        isOpen = false;
    }
    public void OnRecycle()
    {

    }


}
