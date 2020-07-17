/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using GameFramework.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace guoShuai
{
    public class SettingForm : UGuiForm
    {

        public Toggle tog_SimpleChinese;

        public Toggle tog_TraditionChinese;

        public Toggle tog_English;

        public CanvasGroup tip;

        // 当前选择的语言
        private Language curLanguage = Language.Unspecified;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            tog_SimpleChinese.onValueChanged.AddListener(SelectSimpleChinese);
            tog_TraditionChinese.onValueChanged.AddListener(SelectTraditionChinese);
            tog_English.onValueChanged.AddListener(SelectEnglish);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            curLanguage = Game.Localization.Language;
            switch (curLanguage)
            {
                case Language.English:
                    tog_English.isOn = true;
                    break;

                case Language.ChineseSimplified:
                    tog_SimpleChinese.isOn = true;
                    break;

                case Language.ChineseTraditional:
                    tog_TraditionChinese.isOn = true;
                    break;
            
                default:
                    break;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (tip.gameObject.activeSelf)
            {
                tip.alpha = 0.5f + 0.5f * Mathf.Sin(Mathf.PI * Time.time);
            }
        }


        public override void OnClick(string str)
        {
            base.OnClick(str);
            switch (str)
            {
                case "btn_Sure":
                    OnClickSure();
                    break;
                case "btn_Cancel":
                    Close();
                    break;
                default:
                    Log.Error("按钮不存在 " + str);
                    break;
            }
        }

  
        private void SelectSimpleChinese(bool value)
        {
            if (value)
            {
                curLanguage = Language.ChineseSimplified;
                RefreshLanguageTips();
            }
        }

        private void SelectTraditionChinese(bool value)
        {
            if (value)
            {
                curLanguage = Language.ChineseTraditional;
                RefreshLanguageTips();
            }
        }

        private void SelectEnglish(bool value)
        {
            if (value)
            {
                curLanguage = Language.English;
                RefreshLanguageTips();
            }
        }


        private void OnClickSure()
        {

            if(curLanguage == Game.Localization.Language)
            {
                Close();
                return;
            }

         
            Game.Setting.SetString(Constant.Setting.Language, curLanguage.ToString());
            Game.Setting.Save();

            // Game.Sound.StopMusic();
            GameEntry.Shutdown(ShutdownType.Restart);
        }


        private void RefreshLanguageTips()
        {
            tip.gameObject.SetActive(curLanguage != Game.Localization.Language);
        }


    }
}
