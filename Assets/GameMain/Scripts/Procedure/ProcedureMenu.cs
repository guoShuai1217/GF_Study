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

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);

            Log.Info("初始化 " + GetType());
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("进入 " + GetType() + " 流程");

            Game.UI.OpenUIForm(FormEnum.LoginForm,this);
 
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }
    }
}
