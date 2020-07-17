/*
 *		Description: 初始流程,项目运行后的第一个流程
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.07.09
 *
 */
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Localization;
using GameFramework.Procedure;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace guoShuai
{
    public class ProcedureLaunch : ProcedureBase
    {

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);

            Log.Info("初始化 " + GetType());
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("进入 " + GetType() + " 流程");

            // 语言配置：设置当前使用的语言，如果不设置，则默认使用操作系统语言。
            InitLanguageSettings();

            // 变体配置：根据使用的语言，通知底层加载对应的资源变体。
            InitCurrentVariant();

            // 画质配置：根据检测到的硬件信息 Assets/Main/Configs/DeviceModelConfig 和用户配置数据，设置即将使用的画质选项。
            InitQualitySettings();


            // 声音配置：根据用户配置数据，设置即将使用的声音选项。
            InitSoundSettings();
        }

    
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);


            // ChangeState(procedureOwner, typeof(ProcedureSplash));
            ChangeState<ProcedureSplash>(procedureOwner);
        }





        private void InitLanguageSettings()
        {
            //if (Game.Base.EditorResourceMode && Game.Base.EditorLanguage != Language.Unspecified)
            //{
            //    // 编辑器资源模式直接使用 Inspector 上设置的语言
            //    return;
            //}

            Language language = Game.Localization.Language;
            if (Game.Setting.HasSetting(Constant.Setting.Language))
            {
                try
                {
                    string languageString = Game.Setting.GetString(Constant.Setting.Language);
                    language = (Language)Enum.Parse(typeof(Language), languageString);
                }
                catch
                {
                }
            }

            if (language != Language.English
                && language != Language.ChineseSimplified
                && language != Language.ChineseTraditional
                && language != Language.Korean)
            {
                // 若是暂不支持的语言，则使用英语
                language = Language.English;

                Game.Setting.SetString(Constant.Setting.Language, language.ToString());               
                Game.Setting.Save();
            }
        
            Game.Localization.Language = language;

            Log.Info("Init language settings complete, current language is '{0}'.", language.ToString());
        }

        private void InitCurrentVariant()
        {
            if (Game.Base.EditorResourceMode)
            {
                // 编辑器资源模式不使用 AssetBundle，也就没有变体了
                return;
            }

            string currentVariant = null;
            switch (Game.Localization.Language)
            {
                case Language.English:
                    currentVariant = "en-us";
                    break;

                case Language.ChineseSimplified:
                    currentVariant = "zh-cn";
                    break;

                case Language.ChineseTraditional:
                    currentVariant = "zh-tw";
                    break;

                case Language.Korean:
                    currentVariant = "ko-kr";
                    break;

                default:
                    currentVariant = "zh-cn";
                    break;
            }

            Game.Resource.SetCurrentVariant(currentVariant);

            Log.Info("Init current variant complete.");
        }

        private void InitQualitySettings()
        {
            QualityLevelType defaultQuality = QualityLevelType.Fantastic;
            int qualityLevel = Game.Setting.GetInt(Constant.Setting.QualityLevel, (int)defaultQuality);
            QualitySettings.SetQualityLevel(qualityLevel, true);

            Log.Info("Init quality settings complete.");
        }

        private void InitSoundSettings()
        {
            //Game.Sound.Mute("Music", Game.Setting.GetBool(Constant.Setting.MusicMuted, false));
            //Game.Sound.SetVolume("Music", Game.Setting.GetFloat(Constant.Setting.MusicVolume, 0.3f));
            //Game.Sound.Mute("Sound", Game.Setting.GetBool(Constant.Setting.SoundMuted, false));
            //Game.Sound.SetVolume("Sound", Game.Setting.GetFloat(Constant.Setting.SoundVolume, 1f));
            //Game.Sound.Mute("UISound", Game.Setting.GetBool(Constant.Setting.UISoundMuted, false));
            //Game.Sound.SetVolume("UISound", Game.Setting.GetFloat(Constant.Setting.UISoundVolume, 1f));

            Log.Info("Init sound settings complete.");
        }


    }
}
