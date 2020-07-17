using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using GameFramework.Event;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace guoShuai
{
    public class RegistForm : UGuiForm
    {

        public InputField input_Account;
        public InputField input_Password;
        public InputField input_PasswordAgain;

        private RegistLogic logic;
        private RegistModel model;
      
    
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Log.Info("实例化 " + this.GetType());     
        }

        public override void OnClick(string str)
        {
            base.OnClick(str);
            switch (str)
            {
                case "btn_Sure":
                    OnClickSure();
                    break;

                default:
                    Log.Warning("按钮不存在 " + str);
                    break;
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            model = new RegistModel();
            logic = new RegistLogic(this, model);

            logic.AddListenerWebRequest();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            logic.RemoveListenerWebRequest();
            model = null;
            logic = null;

            base.OnClose(isShutdown, userData);
        }


        private void OnClickSure()
        {
            if (!checkAccount())
                return;

            model.Account = input_Account.text;
            model.Password = input_Password.text;
            // 生日性别年龄...

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

            if (string.IsNullOrEmpty(input_Password.text) || input_Password.text.Length < 4 || input_Password.text.Length > 16)
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

            if (input_Password.text != input_PasswordAgain.text)
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

    }

    public class RegistLogic
    {
        private RegistForm form;
        private RegistModel model;

        public RegistLogic(RegistForm form, RegistModel model)
        {
            this.form = form;
            this.model = model;
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

        internal void OnClickSure()
        {
            string json = Utility.Json.ToJson(model);
            byte[] data = Encoding.UTF8.GetBytes(json);
            
            Game.WebRequest.AddWebRequest(ServerUtility.GetRegistURL(), data, this);
        }

        private void WebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
            if (ne.UserData != this) return;

            Log.Error("注册请求出错,网址为{0};错误信息为{1}" ,ne.WebRequestUri, ne.ErrorMessage);
        }

        private void WebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
            if (ne.UserData != this) return;

            // TODO解析返回回来的数据
            // Utility.Json.ToObject<>(ne.GetWebResponseBytes());
            Log.Info("注册成功");

            form.Close();
        }




    }

    [Serializable]
    public class RegistModel
    {
        public string Account { get; set; }

        public string Password { get; set; }

        public string Mail { get; set; }

        public string Phone { get; set; }

        public string Sex { get; set; }

        public string Birthday { get; set; }
    }
}
