using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace guoShuai
{
    public class FindPsdForm : UGuiForm
    {
        public InputField input_Account;
        public InputField input_Code;
        public InputField input_Psd;
        public InputField input_PsdAgain;

        
        public GameObject clockParent;
        public Text txt_Time;

        float m_Timer = 0f;

        private FindPsdLogic logic;
        private FindPsdModel model;

      
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            init();

            model = new FindPsdModel();
            logic = new FindPsdLogic(this, model);
            logic.AddListenerWebRequest();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            logic.RemoveListenerWebRequest();
            model = null;
            logic = null;
           
            base.OnClose(isShutdown, userData);
        }

 
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (clockParent.activeSelf)
            {
                m_Timer += elapseSeconds;
                txt_Time.text = (60 - m_Timer).ToString();
                if(m_Timer >= 60)
                {
                    Log.Info("验证码超时");
                    m_Timer = 0f;
                    clockParent.SetActive(false);
                }
            }
        }

        void init()
        {
            input_Account.text = string.Empty;
            input_Code.text = string.Empty;
            input_Psd.text = string.Empty;
            input_PsdAgain.text = string.Empty;
            clockParent.SetActive(false);
            txt_Time.text = string.Empty;
            m_Timer = 0f;
        }


        #region 按钮和点击事件

        public override void OnClick(string str)
        {
            base.OnClick(str);
            switch (str)
            {
                case "btn_Code":
                    OnClickCode();
                    break;

                case "btn_Sure":
                    OnClickSure();
                    break;
                default:
                    Log.Error("按钮不存在 " + str);
                    break;
            }
        }

        // 获取验证码
        private void OnClickCode()
        {
            logic.GetCode();

            clockParent.SetActive(true);
        }

        // 点击确定
        private void OnClickSure()
        {
            if (!checkAccount())
                return;

            model.Account = input_Account.text;
            model.Password = input_Psd.text;

            logic.OnClickSure();
        }


        bool checkAccount()
        {
            if (string.IsNullOrEmpty(input_Account.text) || input_Account.text.Length < 4 || input_Account.text.Length > 16)
            {
                Game.UI.PushDialog(new DialogParams
                {
                    Mode = 1,
                    Title = "提示",
                    Message = "账号不合法,请重新输入",
                    ConfirmText = "确定",
                    //OnClickConfirm = delegate (object userData) {  },
                    //CancelText = "取消",
                    //OnClickCancel = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
                });
                return false;
            }

            if (string.IsNullOrEmpty(input_Psd.text) || input_Psd.text.Length < 4 || input_Psd.text.Length > 16)
            {
                Game.UI.PushDialog(new DialogParams
                {
                    Mode = 1,
                    Title = "提示",
                    Message = "密码不合法,请重新输入",
                    ConfirmText = "确定",
                    //OnClickConfirm = delegate (object userData) {  },
                    //CancelText = "取消",
                    //OnClickCancel = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
                });

                return false;
            }

            if (input_Psd.text != input_PsdAgain.text)
            {
                Game.UI.PushDialog(new DialogParams
                {
                    Mode = 1,
                    Title = "提示",
                    Message = "两次密码输入不一致,请重新输入",
                    ConfirmText = "确定",
                    //OnClickConfirm = delegate (object userData) {  },
                    //CancelText = "取消",
                    //OnClickCancel = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
                });

                return false;
            }

            return true;
        }

        #endregion


    }


    public class FindPsdLogic
    {
        private FindPsdModel model;
        private FindPsdForm form;

        public FindPsdLogic(FindPsdForm form,FindPsdModel model)
        {
            this.model = model;
            this.form = form;
        }

        // 添加web请求监听事件
        public void AddListenerWebRequest()
        {
            Game.Event.Subscribe(WebRequestSuccessEventArgs.EventId, WebRequestSuccess);
            Game.Event.Subscribe(WebRequestFailureEventArgs.EventId, WebRequestFailure);
        }

        // 移除web请求监听事件
        public void RemoveListenerWebRequest()
        {
            Game.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, WebRequestSuccess);
            Game.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, WebRequestFailure);
        }


        public void GetCode()
        {
            Game.WebRequest.AddWebRequest(ServerUtility.GetCodeURL(), this);
        }


        private void WebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
            if (ne.UserData != this)
                return;

            Log.Error("请求验证码出错 , URL = {0} , ErrorMessage = {1}", ne.WebRequestUri, ne.ErrorMessage);
        }

        private void WebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
            if (ne.UserData != this)
                return;

            switch (ne.SerialId)
            {
                case 1: // 获取验证码请求
                    Log.Info("ne.SerialId" + ne.SerialId);
                    // Utility.Json.ToObject<>(ne.GetWebResponseBytes());

                    // 获取验证码,存到model里
                    // this.model = 
                    break;

                case 2: // 发送重置密码请求



                    break;
              
            }
           
        }

        internal void OnClickSure()
        {
            string json = Utility.Json.ToJson(model);
            byte[] data = Encoding.UTF8.GetBytes(json);
            //Game.WebRequest.AddWebRequest(ServerUtility.GetFindPsdURL(), data, this);
        }
    }


    [Serializable]
    public class FindPsdModel
    {
        public string Account { get; set; }

        public string Password { get; set; }

        public string Code { get; set; }
    }
}
