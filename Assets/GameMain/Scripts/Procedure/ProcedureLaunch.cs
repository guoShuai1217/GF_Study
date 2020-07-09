/*
 *		Description: 初始流程,项目运行后的第一个流程
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.07.09
 *
 */
using GameFramework.Fsm;
using GameFramework.Procedure;
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




        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);


            // ChangeState(procedureOwner, typeof(ProcedureSplash));
            ChangeState<ProcedureSplash>(procedureOwner);
        }
    }
}
