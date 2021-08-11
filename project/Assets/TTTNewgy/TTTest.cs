using EazyGF;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TTTest : MonoBehaviour
{
    public SkeletonAnimation sa;
    public SkeletonGraphic sg;

    private List<SkeletonDataAsset> dat;

    CustomerMgr cMgr;

    CustomerSpeMgr csMgr;

    public Text debugTxt;

    void Start()
    {
        cMgr = GameObject.Find("Battle/CustomerMgr").GetComponent<CustomerMgr>();

        //csMgr = GameObject.Find("Battle/CustomerSpcialMgr").GetComponent<CustomerSpeMgr>();

        StartCoroutine(Handle());

        EventManager.Instance.RegisterEvent(EventKey.Null, TestEvent);
    }

    private IEnumerator Handle()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);



            debugTxt.text = "Num: " + (cMgr.GetNCNum());
        }

        //sg.Skeleton.SetSkin("role_1001");
        //sg.AnimationState.SetAnimation(0, "move", true);
    }

    private void TestEvent(object arg0)
    {
        SwitchOnOffPanel.Instance.OnClosePanle();
        int value = (int)arg0;

        //Debug.LogError("logError : " + value);

        switch (value)
        {
            case 1:

                //PlayAnim("breath");
                //PlayAnim("breath");

                //Debug.LogError(Directory.GetCurrentDirectory());

                //UIMgr.ShowPanel<ClownGamePanel>(new ClownGamePanelData(1));

                //UIMgr.ShowPanel<ItemBuyPanel>(new ItemBuyPanelData(10));

                //UIMgr.ShowPanel<VictoryPanel>(new VictoryPanelData(2, 2));

                //UIMgr.ShowPanel<PausePanel>();

                //UIMgr.ShowPanel<DancingGirlGamePanel>(new DancingGirlGamePanelData(1));

                //UIMgr.ShowPanel<MagicGamePanel>(new MagicGamePanelData(1));


                UIMgr.ShowPanel<CubeMainPanel>();
                //UIMgr.ShowPanel<CubeGridPanel>();

                break;

            case 2:

                UIMgr.ShowPanel<MaskPanel>();

                break;

            case 3:

                PlayAnim("move");

                break;

            case 4:

                PlayAnim("sit");

                break;

            case 5:

                PlayAnim("move_meal");

                break;


            case 6:

                PlayAnim2("role_1001");

                break;

            case 7:

                PlayAnim2("role_1002");

                break;


            case 8:

                Debug.LogError(Directory.GetCurrentDirectory());

                cMgr = GameObject.Find("Battle/CustomerSpace").GetComponent<CustomerMgr>();

                //csMgr = GameObject.Find("Battle/CustomerSpcialMgr").GetComponent<CustomerSpeMgr>();

                StopAllCoroutines();

                StartCoroutine(Handle());

                break;
            case 9:

                PlayAnim2("role_1005");

                break;
            default:
                break;
        }
    }

    public void PlayAnim(string animName)
    {
        //sa..SetAnimation(0, animName, false);

        //sa.sta.SetAnimation(0, animName, true);

        //sa.st

        //sa.AnimationState.SetAnimation(0, animName, true);
    }

    public void PlayAnim2(string name)
    {
        Debug.LogWarning(name + "_0");

        sa.Skeleton.SetSkin(name + "_0");
        //sa.Skeleton.SetSkin(name);
    }


    // Update is called once per frame

    float timer;
    float timeInterval = 1;

   
}
