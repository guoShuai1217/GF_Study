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
        DialogForm,

        /// <summary>
        /// 登陆界面
        /// </summary>
        LoginForm,

        /// <summary>
        /// 菜单
        /// </summary>
        MenuForm,

        /// <summary>
        /// 新建案件
        /// </summary>
        NewCaseForm,

        /// <summary>
        /// 打开已有案件
        /// </summary>
        ExistCaseForm,

        /// <summary>
        /// 绘制界面
        /// </summary>
        DrawForm,

        /// <summary>
        /// 主界面
        /// </summary>
        MainForm,

        /// <summary>
        /// 勘察报告
        /// </summary>
        ReportForm,

    }
}
