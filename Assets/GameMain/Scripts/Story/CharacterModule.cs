using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Resource;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class CharacterModule
{
    private Transform m_LeftTransform;

    private Transform m_RightTransform;

    GameObject uiPoolGameObject;

    // Start is called before the first frame update
    public void Init(Transform _dialogTransform)
    {
        m_LeftTransform = _dialogTransform.Find("Image_leftHead");
        m_RightTransform = _dialogTransform.Find("Image_rightHead");
    }

    public DRRole GetRoleTable(int _roleId)
    {
        IDataTable<DRRole> drdialogTable = GameEntry.DataTable.GetDataTable<DRRole>();
        DRRole drRole = drdialogTable.GetDataRow(_roleId);
        return drRole;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_roleId">角色id</param>
    /// <param name="_name">角色名字</param>
    /// <param name="_pos">角色位置</param>
    public void Refresh(int _roleId,int _pos)
    {
        DRRole drRole=GetRoleTable(_roleId);
        System.Action action = () =>
        {
            UIPool uiPool = uiPoolGameObject.GetComponent<UIPool>();
            UIStruct uiStruct = uiPool.m_UiStructs.Find((_uiStruct) => { return _uiStruct.uiSprite.name.Equals(drRole.ResName); });
            Transform parTransform = _pos < 0 ? m_LeftTransform : m_RightTransform;
            Image img_head = parTransform.Find("Image_head").GetComponent<Image>();
            img_head.sprite = uiStruct.uiSprite;
            img_head.SetNativeSize();
            parTransform.Find("Text").GetComponent<Text>().text = drRole.Name;

            //名字信息展示
            Text txtName_left= parTransform.parent.Find("dialog_center/Image_left/Image_name/Text").GetComponent<Text>();
            Text txtName_right = parTransform.parent.Find("dialog_center/Image_right/Image_name/Text").GetComponent<Text>();
            Text txtName = _pos < 0 ? txtName_left : txtName_right;
            txtName.text = drRole.Name;
        };
        //drRole.
        string Asset = "UIPrefab";

        if (uiPoolGameObject==null)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetUIFormAsset(Asset), Constant.AssetPriority.UIFormAsset, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    uiPoolGameObject = (GameObject)asset;
                    Log.Info("Load 资源 '{0}' OK.", Asset);
                   
                    action.Invoke();    
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", Asset, assetName, errorMessage);
                }));
        }
        else
        {
            action.Invoke();
        }


    }

    /// <summary>
    /// 状态切换
    /// </summary>
    public void OnLeave()
    {

    }

    public void OnDestroy()
    {
        uiPoolGameObject = null;
    }
}
