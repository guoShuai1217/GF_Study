/*
 *		Description: 游戏入口(partial类), 获取自定义的组件
 *		
 *		注 : 		
 *		自定义组件必须继承 GameFrameworkComponent
 *		
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.07.09
 *
 */
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
namespace guoShuai
{
    public partial class Game : MonoBehaviour
    {
        // 注掉的代码作为参考样式
        //public static BuiltinDataComponent BuiltinData
        //{
        //    get;
        //    private set;
        //}


        private static void InitCustomComponents()
        {
            // BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            

        }

    }
}