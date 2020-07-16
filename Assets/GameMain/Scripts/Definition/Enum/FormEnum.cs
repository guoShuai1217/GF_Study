/*
 *		Description: UI界面的名称枚举
 *		
 *		注:
 *		枚举值要和 UIForm.txt 里的id对应起来
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.07.09
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace guoShuai
{
    public enum FormEnum : byte
    {
        Undefined = 0,

        /// <summary>
        /// 弹框
        /// </summary>
        DialogForm = 1,

        /// <summary>
        /// 加载界面
        /// </summary>
        LoadingForm = 2,

        /// <summary>
        /// 登陆界面
        /// </summary>
        LoginForm = 10,

        /// <summary>
        /// 注册
        /// </summary>
        RegistForm = 11 ,

        /// <summary>
        /// 忘记密码
        /// </summary>
        FindPsdForm = 12 ,

        /// <summary>
        /// 菜单
        /// </summary>
        MenuForm = 20,

        /// <summary>
        /// 新建案件
        /// </summary>
        NewCaseForm =21,

        /// <summary>
        /// 打开已有案件
        /// </summary>
        ExistCaseForm = 22,

        /// <summary>
        /// 绘制界面
        /// </summary>
        DrawForm = 50,

        /// <summary>
        /// 主界面
        /// </summary>
        MainForm = 51,

        /// <summary>
        /// 勘察报告
        /// </summary>
        ReportForm = 100,

    }
}
