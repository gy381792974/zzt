using UnityEngine;

using System.Collections;

using Spine.Unity;

public class TTGame : MonoBehaviour
{
    // Start is called before the first frame update

    public SkeletonAnimation sa;

    void Start()
    {
        //Sequence sequence = DOTween.Sequence();

        //Tween tween4 = obj.transform.DOLocalMoveX(-200, 0.5f).SetEase(Ease.Linear);
        //sequence.Join(tween4);

        //Tween tween5 = obj.transform.DOLocalMoveX(-70, 0.5f).SetDelay(0.5f).SetEase(Ease.Linear);
        //sequence.Join(tween5);

        ////Tween tween6 = obj.transform.DOLocalMoveX(-200, 0.5f).SetEase(Ease.Linear);
        ////tween6.SetLoops(2, LoopType.Yoyo);
        ////sequence.Join(tween6);

        //sequence.OnComplete(() =>
        //{
        //   
        //   
        //    GameObject.Destroy(obj);
        //});

        //Vector3[] path = new Vector3[3];
        //path[0] = obj.transform.position;
        //path[1] = obj.transform.position + new Vector3(-1, 1, 0);
        //path[2] = flyTargetPos;

        //var tweenPath = obj.transform.DOPath(path, 0.6f, PathType.CatmullRom);
        //tweenPath.SetEase(Ease.Linear);
        //tweenPath.OnComplete(() =>
        //{
        //}


        StartCoroutine(DelHandl());
    }

    // Update is called once per frame

    public IEnumerator DelHandl()
    {
        yield return new WaitForSeconds(1);

        //Debug.LogWarning("Del");

        //UIMgr.ShowPanel<TTTGyPanel>();

        //UIMgr.ShowPanel<MainPanel>();

        //AssetMgr.Instance.LoadGameobj("TTModel");

        //Stall_PropertyBase[] datas = Stall_DataBase.GetArray(0);

        //for (int j = 0; j < datas.Length; j++)
        //{
        //    Debug.LogError(string.Format("{0},{1},{2}", datas[j].ID, datas[j].CapacityNum, datas[j].NeedStar));
        //}

        //for (int i = 1; i < 2; i++)
        //{                
        //}
        //LoadSkeletonDataAssetInAssetBundle("role_1001_2_4_5_SkeletonData", "role_1001_2_4_5_SkeletonData");


#if UNITY_EDITOR
        //LoadSkeletonDataAssetInEditor("CusRole", "role_1001_2_4_5_SkeletonData");
        LoadSkeletonDataAssetInAssetBundle("role_1001_2_4_5_skeletondata", "role_1001_2_4_5_skeletondata");
#else
          LoadSkeletonDataAssetInAssetBundle("role_1001_2_4_5_skeletondata", "role_1001_2_4_5_skeletondata");
#endif

        sa.skeleton.SetSkin("role_1002");
        sa.AnimationState.SetAnimation(0, "move", true);
    }

    private void LoadSkeletonDataAssetInAssetBundle(string spineAssetBundleName, string spineAssetName)
    {
        sa.skeletonDataAsset = AssetMgr.Instance.LoadAsset<SkeletonDataAsset>(spineAssetBundleName, $"{spineAssetName}");
       
        sa.Initialize(true);
    }

    // 加载本地资源赋值
    private void LoadSkeletonDataAssetInEditor(string spineAssetBundleName, string spineAssetName)
    {

        //string basePath = $"{AB_ResFilePath.abAllSpinesRootDir}/{spineAssetBundleName}/{spineAssetName}";
        //sa.skeletonDataAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>($"{basePath}.asset");

        ////skeletonAnimation.material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>($"{basePath}_Material.mat");
        //sa.Initialize(true);
    }
}
