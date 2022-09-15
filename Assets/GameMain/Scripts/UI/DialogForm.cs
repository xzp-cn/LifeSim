//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class DialogForm : UGuiForm
    {
        [SerializeField]
        private Text m_TitleText = null;

        [SerializeField]
        private Text m_MessageText = null;

        [SerializeField] 
        private Image m_MessageBg;

        [SerializeField]
        private GameObject[] m_ModeObjects = null;

        [SerializeField]
        private Text[] m_ConfirmTexts = null;

        [SerializeField]
        private Text[] m_CancelTexts = null;

        [SerializeField]
        private Text[] m_OtherTexts = null;

        [SerializeField]
        private Image m_MessageBG = null;

        private int m_DialogMode = 1;
        private bool m_PauseGame = false;
        private object m_UserData = null;
        private GameFrameworkAction<object> m_OnClickConfirm = null;
        private GameFrameworkAction<object> m_OnClickCancel = null;
        private GameFrameworkAction<object> m_OnClickOther = null;

        public int DialogMode
        {
            get
            {
                return m_DialogMode;
            }
        }

        public bool PauseGame
        {
            get
            {
                return m_PauseGame;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public void OnConfirmButtonClick()
        {
            Close();
            m_MessageBG.gameObject.SetActive(false);
            if (m_OnClickConfirm != null)
            {
                m_OnClickConfirm(m_UserData);
            }
        }

        public void OnCancelButtonClick()
        {
            Close();

            if (m_OnClickCancel != null)
            {
                m_OnClickCancel(m_UserData);
            }
        }

        public void OnOtherButtonClick()
        {
            Close();

            if (m_OnClickOther != null)
            {
                m_OnClickOther(m_UserData);
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            DialogParams dialogParams = (DialogParams)userData;
            if (dialogParams == null)
            {
                Log.Warning("DialogParams is invalid.");
                return;
            }

            m_DialogMode = dialogParams.Mode;
            RefreshDialogMode();

            m_TitleText.text = dialogParams.Title;
            m_MessageText.text = dialogParams.Message;

            m_PauseGame = dialogParams.PauseGame;
            RefreshPauseGame();

            m_UserData = dialogParams.UserData;
            RefreshMessageImage();

            RefreshConfirmText(dialogParams.ConfirmText);
            m_OnClickConfirm = dialogParams.OnClickConfirm;

            RefreshCancelText(dialogParams.CancelText);
            m_OnClickCancel = dialogParams.OnClickCancel;

            RefreshOtherText(dialogParams.OtherText);
            m_OnClickOther = dialogParams.OnClickOther;
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            if (m_PauseGame)
            {
                GameEntry.Base.ResumeGame();
            }

            m_DialogMode = 1;
            m_TitleText.text = string.Empty;
            m_MessageText.text = string.Empty;
            m_PauseGame = false;
            m_UserData = null;

            RefreshConfirmText(string.Empty);
            m_OnClickConfirm = null;

            RefreshCancelText(string.Empty);
            m_OnClickCancel = null;

            RefreshOtherText(string.Empty);
            m_OnClickOther = null;

            base.OnClose(isShutdown, userData);
        }

        private void RefreshDialogMode()
        {
            for (int i = 1; i <= m_ModeObjects.Length; i++)
            {
                m_ModeObjects[i - 1].SetActive(i == m_DialogMode);
            }
        }

        private void RefreshPauseGame()
        {
            if (m_PauseGame)
            {
                GameEntry.Base.PauseGame();
            }
        }

        private void RefreshConfirmText(string confirmText)
        {
            if (string.IsNullOrEmpty(confirmText))
            {
                confirmText = GameEntry.Localization.GetString("Dialog.ConfirmButton");
            }

            for (int i = 0; i < m_ConfirmTexts.Length; i++)
            {
                m_ConfirmTexts[i].text = confirmText;
            }
        }

        private void RefreshCancelText(string cancelText)
        {
            if (string.IsNullOrEmpty(cancelText))
            {
                cancelText = GameEntry.Localization.GetString("Dialog.CancelButton");
            }

            for (int i = 0; i < m_CancelTexts.Length; i++)
            {
                m_CancelTexts[i].text = cancelText;
            }
        }

        private void RefreshOtherText(string otherText)
        {
            if (string.IsNullOrEmpty(otherText))
            {
                otherText = GameEntry.Localization.GetString("Dialog.OtherButton");
            }

            for (int i = 0; i < m_OtherTexts.Length; i++)
            {
                m_OtherTexts[i].text = otherText;
            }
        }

        private void RefreshMessageImage()
        {
            if (m_UserData==null)
            {
                m_MessageBG.gameObject.SetActive(false);
            }
            else
            {
                Sprite sp= GetUISprite((string)m_UserData);
                m_MessageBG.sprite = sp;
                m_MessageBG.gameObject.SetActive(false);
            }
        }


        private UIPool UiPool;
        private GameObject uiPoolObject;

        Sprite GetUISprite(string _imageName)
        {
            Sprite sp = null;
            Action action = () =>
            {
                UIPool uiPool = uiPoolObject.GetComponent<UIPool>();
                UIStruct uiStruct = uiPool.m_UiStructs.Find((_uiStruct) => { return _uiStruct.uiSprite.name.Equals(_imageName); });
                sp = uiStruct.uiSprite;
            };

            if (uiPoolObject == null)
            {
                GameEntry.Resource.LoadAsset(AssetUtility.GetUIFormAsset("UIPrefab"), Constant.AssetPriority.UIFormAsset, new LoadAssetCallbacks(
                    (assetName, asset, duration, userData) =>
                    {
                        uiPoolObject = (GameObject)asset;
                        Log.Info("Load 资源 '{0}' OK.", "UIPrefab");
                        action?.Invoke();
                    },

                    (assetName, status, errorMessage, userData) =>
                    {
                        Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", "UIPrefab", assetName, errorMessage);
                    }));
            }
            else
            {
                action?.Invoke();
            }

            return sp;
        }
    }
}
