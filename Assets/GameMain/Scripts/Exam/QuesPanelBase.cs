using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using StarForce;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuesPanelBase: IUIModule
{
    protected int m_quesId;//题目id
    protected Transform m_panel;
    protected Text m_TitleText;
    protected List<QuesItemBase> m_QuesItemList=new List<QuesItemBase>();

    protected Toggle[] m_Toggles;

    private ToggleGroup group;
    //获取数据表数据
    protected virtual T GetDataRow<T>(int _rowId) where  T:IDataRow
    {
        IDataTable<T>  dataTable= GameEntry.DataTable.GetDataTable<T>();
        T t=dataTable.GetDataRow(_rowId);
        return t;
    }
    

    public virtual void OnInit(Transform _panel,int _quesId)
    {
        m_panel = _panel;
        m_panel.name = _quesId.ToString();
        m_quesId = _quesId;
        m_TitleText = m_panel.Find("Text_title").GetComponent<Text>();
        m_Toggles = m_panel.GetComponentsInChildren<Toggle>(true);

        @group= m_panel.GetComponent<ToggleGroup>();
        foreach (Toggle _toggle in m_Toggles)
        {
            group.RegisterToggle(_toggle);
        }
        group.SetAllTogglesOff();
    }
    public virtual void OnOpen()
    {
        m_panel.gameObject.SetActive(true);
    }
    public virtual void Update()
    {

    }

    public virtual void OnClose(bool isShutdown, object userData)
    {
        m_panel.gameObject.SetActive(false);
        group.SetAllTogglesOff();
        
    }

    public virtual void OnRecycle()
    {
        m_QuesItemList.Clear();
    }

 




}
