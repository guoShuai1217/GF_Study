/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */

using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace guoShuai
{
    public class DialogForm : UGuiForm
    {
        [SerializeField]
        private Text m_TitleText = null;

        [SerializeField]
        private Text m_MessageText = null;

        [SerializeField]
        private GameObject[] m_BtnArr = null;
       
        [SerializeField]
        private Text m_ConfirmTexts = null;

        [SerializeField]
        private Text m_CancelTexts = null;

        [SerializeField]
        private Text m_OtherTexts = null;

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
                Game.Base.ResumeGame();
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
            //for (int i = 1; i <= m_BtnArr.Length; i++)
            //{
            //    m_ModeObjects[i - 1].SetActive(i == m_DialogMode);
            //}
            switch (m_DialogMode)
            {
                case 1:
                    m_BtnArr[0].SetActive(false);
                    m_BtnArr[1].SetActive(true);
                    m_BtnArr[2].SetActive(false);
                    break;
                case 2:
                    m_BtnArr[0].SetActive(true);
                    m_BtnArr[1].SetActive(false);
                    m_BtnArr[2].SetActive(true);
                    break;
                case 3:
                    m_BtnArr[0].SetActive(true);
                    m_BtnArr[1].SetActive(true);
                    m_BtnArr[2].SetActive(true);
                    break;
                default:
                    break;
            }

        }

        private void RefreshPauseGame()
        {
            if (m_PauseGame)
            {
                Game.Base.PauseGame();
            }
        }

        private void RefreshConfirmText(string confirmText)
        {
            if (string.IsNullOrEmpty(confirmText))
            {
                confirmText = Game.Localization.GetString("Dialog.ConfirmButton");
            }

            m_ConfirmTexts.text = confirmText;
        }

        private void RefreshCancelText(string cancelText)
        {
            if (string.IsNullOrEmpty(cancelText))
            {
                cancelText = Game.Localization.GetString("Dialog.CancelButton");
            }

            m_CancelTexts.text = cancelText;
        }

        private void RefreshOtherText(string otherText)
        {
            if (string.IsNullOrEmpty(otherText))
            {
                otherText = Game.Localization.GetString("Dialog.OtherButton");
            }

            m_OtherTexts.text = otherText;
        }

    }
}
