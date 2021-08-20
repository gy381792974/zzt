using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchOnOffItem : MonoBehaviour
{

    public Text ItemDes;
    public InputField Input;
    public Text btnTxt;

    private void Start()
    {

    }

    SwitchOnoffKey mKey;
    public void Refresh(string text, int value, SwitchOnoffKey key)
    {
        mKey = key;
        ItemDes.text = text;

        switch (key)
        {
            case SwitchOnoffKey.TEST:
                
                break;

            case SwitchOnoffKey.DISPLAYLOG:
                
                break;

            case SwitchOnoffKey.TestValue1:
                
                break;

            case SwitchOnoffKey.TestValue2:
                
                break;
            case SwitchOnoffKey.ChangeLanguage:

                ItemDes.text = $"多语言0en1cn";

                break;

            case SwitchOnoffKey.AddItemNum:
                
                ItemDes.text = $"增加所有道具";

                break;

            case SwitchOnoffKey.DeleteAllPlayerPrefs:
                
                break;

            case SwitchOnoffKey.AddCoinStar:

                ItemDes.text = $"增加金币星星";

                break;
            case SwitchOnoffKey.timeScale:
                
                ItemDes.text = $"修改时间缩放";
                
                break;
            case SwitchOnoffKey.DebugPanel:

                ItemDes.text = $"打开调试界面";

                break;

            case SwitchOnoffKey.KeepNum:

                ItemDes.text = $"顾客最少人数";

                break;
            case SwitchOnoffKey.TargetCus:

                ItemDes.text = $"创建特定客户";

                break;
            case SwitchOnoffKey.UnlockAllBuild:

                ItemDes.text = $"解锁所有建筑 并设置解锁的等级";

                break;
            default:
                break;
        }

        Input.text = value.ToString();

        Input.textComponent.fontSize = 25;
    }

    public void NotifySubmit()
    {
        int result;

        if (!int.TryParse(Input.text, out result))
        {
            Input.text = "0";
        }

        if (int.TryParse(Input.text, out result))
        {
            SwitchOnOffPanel panel = transform.GetComponentInParent<SwitchOnOffPanel>();
            if (panel != null)
            {
                SwitchOnoffKey key = mKey;

                if (key == SwitchOnoffKey.NUM)
                {
                    OnClickCallBack(null);
                }
                else
                {
                    panel.setSwitchOnoffValue(key, result);
                }

            }
        }
    }

    private void OnClickCallBack(object obj)
    {
        switch (mKey)
        {
            case SwitchOnoffKey.NUM:

                Input.textComponent.fontSize = 20;

                Input.text = "Model Num: " + ModelDebug.Instance.GetModelCount().ToString();

                break;

            default:
                break;
        }
    }

}
