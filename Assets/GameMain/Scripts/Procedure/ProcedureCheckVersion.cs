/*
 *		Description: 版本检查流程
 *		
 *		这里应该涉及AB包的热更,我还没想好怎么写...
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.07.09
 *
 */
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
namespace guoShuai
{
    public class ProcedureCheckVersion : ProcedureBase
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

    }
}
