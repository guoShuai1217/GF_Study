/*
 *		Description: 预加载资源流程
 *		
 *		并不是真正的去加载资源,而是读取配置表里的数据,存到内存里
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
    public class ProcedurePreload : ProcedureBase
    {

        public static readonly string[] DataTableName = new string[]
        {
             "Test", // 这是个测试资源，并没有使用

             "UIForm",// 界面

            //"Entity",
            //"Music",
            //"Scene",
            //"Sound",
            //"UISound",
        };

        // 资源预加载完成后,value = true 
        private Dictionary<string, bool> m_LoadedDic = new Dictionary<string, bool>();


        #region 重写父类的方法

    
        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);

            Log.Info("初始化 " + GetType());
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);         
            Log.Debug("进入 " + GetType() + " 流程");

           // LoadingForm.Instance.OnShow();
           
            Game.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, LoadDataTableSuccess);
            Game.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, LoadDataTabelFailure);
           
            m_LoadedDic.Clear();
            PreLoadResource();
         
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            foreach (bool item in m_LoadedDic.Values)
            {
                if (!item) return;
            }

            Log.Debug("所有资源都加载完成");

            // LoadingForm.Instance.OnHide();
            ChangeState<ProcedureMenu>(procedureOwner);

        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {         
            Game.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, LoadDataTableSuccess);
            Game.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, LoadDataTabelFailure);





            base.OnLeave(procedureOwner, isShutdown);
        }



        #endregion



        // 预加载资源
        private void PreLoadResource()
        {

            foreach (string item in DataTableName)
            {
                LoadDataTable(item);
            }

        }


        // 加载配置表
        private void LoadDataTable(string item)
        {
            string dataTableName = Utility.Text.Format("DataTable.{0}", item);
            m_LoadedDic.Add(dataTableName, false);

            // 开始加载配置文件
            Game.DataTable.LoadDataTable(item, false,this);
        }



        #region 注册事件的回调

        // 加载配置文件成功的回调
        private void LoadDataTableSuccess(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
            if (ne.UserData != this) return;

            m_LoadedDic[Utility.Text.Format("DataTable.{0}", ne.DataTableName)] = true;
            Log.Info("Load data table '{0}' OK.", ne.DataTableName);
        }


        // 加载配置文件失败的回调
        private void LoadDataTabelFailure(object sender, GameEventArgs e)
        {

            LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
            if (ne.UserData != this) return;

            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableName, ne.DataTableAssetName, ne.ErrorMessage);
        }


        #endregion




    }
}
