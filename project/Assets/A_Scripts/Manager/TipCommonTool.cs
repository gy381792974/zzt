using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EazyGF
{
    public class TipCommonTool
    {
        TipPanel tip;
        private static TipCommonTool instance;
        public static TipCommonTool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TipCommonTool();
                    instance.Init();
                }
                return instance;
            }
        }

        public void Init()
        {

        }

        public void ShowTip(string str)
        {
            if (tip == null)
            {
                UIMgr.ShowPanel<TipPanel>();
                tip = UIMgr.GetUI<TipPanel>();
            }
            tip.PlayAnimation(str);
        }

        public void ShowTip(int[] arr, params object[] objs)
        {
            if (tip == null)
            {
                UIMgr.ShowPanel<TipPanel>();
                tip = UIMgr.GetUI<TipPanel>();
            }
            tip.PlayAnimation(arr, objs);
        }

    }
}


