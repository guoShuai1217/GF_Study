
我会把自己学习和理解的东西写到这里来 , 不管后面使用这个框架 , 或者给朋友推荐这个框架 , 作为一个互相交流和学习的记录

2020.07.10 周五 : 
GF的语言设置  : 
	
1. Launch的时候 , 会设置语言 GameEntry.Localization.Language = language; 	

2. PreLoad的时候, 会根据 GameEntry.Localization.Language 去加载对应的Default.xml文件
    2.1 Utility.Text.Format("Assets/GameMain/Localization/{0}/Dictionaries/{1}.{2}", GameEntry.Localization.Language.ToString(), assetName, fromBytes ? "bytes" : "xml");
	
3. UIForm 预制体 , 里面所有的 Text 里 , 会预先写入 Default.xml 里的key值 ; 
	
4. 在 UguiForm 基类里, OnInit() 里会获取所有的 Text 组件,然后根据里面预先写好的key值 , 换成对应的xml里的value值 : 

 Text[] texts = GetComponentsInChildren<Text>(true);
 for (int i = 0; i < texts.Length; i++)
 {
      texts[i].font = s_MainFont;
      if (!string.IsNullOrEmpty(texts[i].text))
      {
           texts[i].text = GameEntry.Localization.GetString(texts[i].text);
      }
  }



2020.07.17 周五 : 
一个页面请求多个网址的逻辑  : 

有时候一个页面里，我们会请求多个网址，在请求的回调里如何判断是哪个请求的回调呢 ? 
框架里给了 SerialId 参数 ， 用来区分是哪一个请求发送的数据

		protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("进入 " + GetType() + " 流程");

            Game.Event.Subscribe(WebRequestSuccessEventArgs.EventId, WebRequestSuccess);
            Game.Event.Subscribe(WebRequestFailureEventArgs.EventId, WebRequestFailure);

			// 发送登陆请求
            string url = ServerUtility.GetLoginURL("13817826484", "123456");
            Game.WebRequest.AddWebRequest(url, this);

			// 发送重置密码请求
            string url1 = ServerUtility.GetFindPsdURL("13817826484", "123456","631425");
            Game.WebRequest.AddWebRequest(url1, this);
        }


		private void WebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
            if (ne.UserData != this)
                return;

            switch (ne.SerialId)
            {
                case 1: // 登陆请求的回复
                   
				   
				   
                    break;

                case 2: // 重置密码请求的回复



                    break;
              
            }
           
        }



2020.07.17 周五 : 
关于如何设置语言 : 


















