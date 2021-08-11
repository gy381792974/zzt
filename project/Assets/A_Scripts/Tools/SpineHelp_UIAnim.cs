
using Spine.Unity;
using UnityEngine;

#pragma warning disable 649
[RequireComponent(typeof(SkeletonGraphic))]
public class SpineHelp_UIAnim : MonoBehaviour
{
    //ab包名
    [SerializeField] private string spineAssetBundleName;
    //资源名
    [SerializeField] private string spineAssetName;

    //播放动画
    [SerializeField] private string aniName;
    [SerializeField] private bool loop = true;

    private SkeletonGraphic skeletonGraphic;
    bool isInit = false;
    void Awake()
    {
        if (isInit)
        {
            return;
        }
        skeletonGraphic = GetComponent<SkeletonGraphic>();
        if (skeletonGraphic == null)
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
        isInit = true;
    }


    // 加载本地资源赋值
    private void LoadSkeletonDataAssetInEditor()
    {
#if UNITY_EDITOR
        string basePath = $"{AB_ResFilePath.abAllSpinesRootDir}/{spineAssetBundleName}/{spineAssetName}";
        skeletonGraphic.skeletonDataAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>($"{basePath}_SkeletonData.asset");

        skeletonGraphic.material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>($"{basePath}_Material.mat");
        skeletonGraphic.Initialize(true);


        PlayAnimation();
#endif
    }

    private void LoadSkeletonDataAssetInAssetBundle()
    {
        skeletonGraphic.skeletonDataAsset = AssetMgr.Instance.LoadAsset<SkeletonDataAsset>(spineAssetBundleName, $"{spineAssetName}_SkeletonData.asset");
        skeletonGraphic.material = AssetMgr.Instance.LoadAsset<Material>(spineAssetBundleName, $"{spineAssetName}_Material.mat");
        skeletonGraphic.Initialize(true);

        PlayAnimation();
    }
    public void LoadSkeletonDataAssetInAssetBundle(string ABname, string spineName)
    {
        skeletonGraphic.skeletonDataAsset = AssetMgr.Instance.LoadAsset<SkeletonDataAsset>(ABname, $"{spineName}_SkeletonData.asset");
        skeletonGraphic.material = AssetMgr.Instance.LoadAsset<Material>(ABname, $"{spineName}_Material.mat");
        skeletonGraphic.Initialize(true);

        PlayAnimation();
    }
    public void LoadSkeletonDataAssetInEditor(string ABname, string spineName)
    {
#if UNITY_EDITOR
        string basePath = $"{AB_ResFilePath.abAllSpinesRootDir}/{ABname}/{spineName}";
        skeletonGraphic.skeletonDataAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>($"{basePath}_SkeletonData.asset");

        skeletonGraphic.material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>($"{basePath}_Material.mat");
        skeletonGraphic.Initialize(true);

        PlayAnimation();
#endif
    }
    public void Init()
    {
        if (!isInit)
        {
            Awake();
        }
    }

    private void PlayAnimation()
    {
        if (!string.IsNullOrEmpty(aniName))
        {
            //skeletonGraphic.Skeleton.SetSkin(aniName);

            skeletonGraphic.AnimationState.SetAnimation(0, aniName, loop);
        }
    }

}
#pragma warning restore 649

