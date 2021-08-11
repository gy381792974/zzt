using System.Collections;
using EazyGF;
using UnityEngine;
using UnityEngine.Networking;

public class JsonDataLoaderMgr :Singleton<JsonDataLoaderMgr>
{
    public void LoadGameData()
    {
        //加载本地数据
        switch (GameMgr.Instance.LoadMode)
        {
            case AssetLoadMode.EditorMode:
                new LocalJsonDataLoader().LoadInEditor();
                break;
            case AssetLoadMode.AssetBundleMode:
                new LocalJsonDataLoader().LoadInAssetBundle();
                break;
        }
      
       //加载网络数据
       // StartCoroutine(DownLoadJsonData());
    }
    

    private IEnumerator DownLoadJsonData()
    {
        string uri = @"file:///D:";
        UnityWebRequest unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri);
       // unityWebRequest.timeout = 10;
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
        {
            // 下载出错
            Debug.LogError("下载JsonAB包出错:"+unityWebRequest.error);
        }
        else
        {
            // 下载完成
            AssetBundle assetBundle = (unityWebRequest.downloadHandler as DownloadHandlerAssetBundle)?.assetBundle;
            unityWebRequest.Dispose();
            if (assetBundle != null)
            {
                //todo 通过反射给当前的数据赋值
                TextAsset[] textAssets = assetBundle.LoadAllAssets<TextAsset>();
                var assembly = ReflectionExtension.GetAssemblyCSharp();
                for (int i = 0; i < textAssets.Length; i++)
                {
                   // var type = assembly.GetType(textAssets[i].name);
                   // var memberInfo = type.GetMember("");
                   // memberInfo.SetValue();
                }
               
            }
        }
    }
}
