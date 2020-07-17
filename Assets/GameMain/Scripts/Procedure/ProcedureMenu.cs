/*
 *		Description: 界面流程
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.07.09
 *
 */
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using System.Collections.Generic;
using GameFramework.Event;
using System;
using GameFramework;

namespace guoShuai
{
    class ProcedureMenu : ProcedureBase
    {
        // 0 : 无状态
        // 1 : 新建案件
        // 2 : 打开已有案件
        private CaseEnum curCase = CaseEnum.None;

        /// <summary>
        /// 设置案件类型
        /// </summary>
        /// <param name="curCase"></param>
        public void SetCurCase(CaseEnum curCase)
        {
            this.curCase = curCase;
        }

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);

            Log.Info("初始化 " + GetType());
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("进入 " + GetType() + " 流程");
            curCase = CaseEnum.None;

            Game.UI.OpenUIForm(FormEnum.LoginForm,this);
 
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (curCase == CaseEnum.None)
                return;

            //   如果是新案件 : 进入ProcedureDraw流程
            // 如果是已有案件 : 进入ProcedureEditor流程
            ChangeState(procedureOwner, curCase == CaseEnum.NewCase ? typeof(ProcedureDraw):typeof(ProcedureEditor));

        }
    }


    public enum CaseEnum : byte
    {
        None,
        NewCase,
        OpenCase
    }
}
