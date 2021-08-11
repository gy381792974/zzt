
using Spine.Unity;
using UnityEngine;

#pragma warning disable 649
[RequireComponent(typeof(SkeletonAnimation))]
public class SpineHelp_Animtion : MonoBehaviour
{
    //ab包名
    [SerializeField] private string spineAssetBundleName;
    //资源名
    [SerializeField] private string spineAssetName;
    private SkeletonAnimation skeletonAnimation;

    //骨骼名
    [SerializeField] private string skeletonName;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (skeletonAnimation == null)
        {
            return;
        }
        if (string.IsNullOrEmpty(spineAssetBundleName))
        {
            return;
        }
        if (!string.IsNullOrEmpty(spineAssetBundleName))
        {
            spineAssetBundleName = spineAssetBundleName.ToLower();
        }
        else
        {
            Debug.LogError(gameObject.name + "：spine 的ab包名不能为空！");
            return;
        }

        if (string.IsNullOrEmpty(spineAssetName))
        {
            spineAssetName = spineAssetBundleName;
        }
#if UNITY_EDITOR
        //目前发现，在编辑器模式下打成ab包后无法正确显示Spine动画
        //因此在编辑器模式下直接加载本地资源进行赋值操作
        LoadSkeletonDataAssetInEditor();
#else
//在移动端直接加载ab包就可以了
        LoadSkeletonDataAssetInAssetBundle();
#endif
    }


    // 加载本地资源赋值
    private void LoadSkeletonDataAssetInEditor()
    {
#if UNITY_EDITOR
        string basePath = $"{AB_ResFilePath.abAllSpinesRootDir}/{spineAssetBundleName}/{spineAssetName}";
        skeletonAnimation.skeletonDataAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>($"{basePath}_SkeletonData.asset");

        //skeletonAnimation.material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>($"{basePath}_Material.mat");
        skeletonAnimation.Initialize(true);

        SetOtherAtt();
#endif
    }

    private void LoadSkeletonDataAssetInAssetBundle()
    {
        skeletonAnimation.skeletonDataAsset = AssetMgr.Instance.LoadAsset<SkeletonDataAsset>(spineAssetBundleName, $"{spineAssetName}_SkeletonData.asset");
        //skeletonAnimation.material = AssetMgr.Instance.LoadAsset<Material>(spineAssetBundleName, $"{spineAssetBundleName}_Material.mat");
        skeletonAnimation.Initialize(true);

        SetOtherAtt();
    }

    private void SetOtherAtt()
    {
        if (!string.IsNullOrEmpty(skeletonName))
        {
            skeletonAnimation.Skeleton.SetSkin(skeletonName);
        }
    }
}
#pragma warning restore 649

