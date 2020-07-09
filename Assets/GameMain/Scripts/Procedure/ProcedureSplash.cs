/*
 *		Description: 动画流程
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
    public class ProcedureSplash : ProcedureBase
    {

        private float m_timer = 0f;

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);

            Log.Info("初始化 " + GetType());
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_timer = 0f;
            Log.Debug("进入 " + GetType() + " 流程");
          
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            // 这里模拟动画播放3s,之后切换流程
            m_timer += elapseSeconds;
            if(m_timer >= 3f)
            {
                // 如果是编辑器模式,就切换到预加载资源流程;
                ChangeState(procedureOwner, Game.Base.EditorResourceMode ? typeof(ProcedurePreload) : typeof(ProcedureCheckVersion));
            }

        }
    }
}
