using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EazyGF
{
    public class AwardData
    {
        public int id; // 3调味瓶
        public int num;

        public AwardData(int id, int num)
        {
            this.id = id;
            this.num = num;
        }
    }


    class UICommonUtil : SingleClass<UICommonUtil>
    {

        public IEnumerator DelayedHandle(Action<object> listener, float time, object obj = null)
        {
            yield return new WaitForSeconds(time);

            if (listener != null)
            {
                listener(obj);
            }
        }

        public List<int> UpsetData(List<int> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                int index = UnityEngine.Random.Range(i, data.Count);

                if (i != index)
                {
                    int a = data[i];
                    data[i] = data[index];
                    data[index] = a;
                }
            }

            return data;
        }

        private RectTransform rootUiCanvas;
        private RectTransform RootUICanvas
        {
            get
            {
                if (rootUiCanvas == null)
                {
                    rootUiCanvas = UIPanelManager.Instance.GetComponent<RectTransform>();
                }

                return rootUiCanvas;
            }
        }

        public Vector2 GetUIPosBySceenPos(RectTransform uICanvas)
        {
            float cWidth = uICanvas.rect.width;

            float cHeight = uICanvas.rect.height;

            float screenWidth = Screen.width;

            float screenHeight = Screen.height;

            float X = Input.mousePosition.x - screenWidth / 2f;

            float Y = Input.mousePosition.y - screenHeight / 2f;

            Vector2 tranPos = new Vector2(X * (cWidth / screenWidth), Y * (cHeight / screenHeight));

            return tranPos;
        }

        public Vector2 GetUIPosByWorldPos(Vector3 worldPos)
        {
            Vector3 scrPos = Camera.main.WorldToScreenPoint(worldPos);


            float canvasHeight = RootUICanvas.rect.height;
            float canvasWidth = RootUICanvas.rect.width;

            float scrWidth = UnityEngine.Screen.width;
            float scrHeight = UnityEngine.Screen.height;

            scrPos.x = scrPos.x - scrWidth / 2;
            scrPos.y = scrPos.y - scrHeight / 2;

            float offx = (scrPos.x / scrWidth) * canvasWidth;
            float offy = (scrPos.y / scrHeight) * canvasHeight;

            Vector2 starPos = new Vector2(offx, offy);

            return starPos;

            //return World2CanvasPos(RootUICanvas.transform.GetComponent<Canvas>(), worldPos);
        }

        /// <summary>
        /// 世界坐标向画布坐标转换
        /// </summary>
        /// <param name="canvas">画布</param>
        /// <param name="world">世界坐标</param>
        /// <returns>返回画布上的二维坐标</returns>
        protected Vector2 World2CanvasPos(Canvas canvas, Vector3 worldPos)
        {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                worldPos, canvas.GetComponent<Camera>(), out position);
            return position;
        }


        //读取数据
        public static string GetSerSavePath(string savaDataName)
        {
            string savePath = "";

#if !UNITY_EDITOR
                savePath = Path.Combine(Application.persistentDataPath, savaDataName);
#else
            savePath = Path.Combine(Directory.GetCurrentDirectory(), $"OtherAssets/PlayerData/{savaDataName}");
#endif
            return savePath;
        }


        public static string GetTranstion(int[] tran, int[] param)
        {
            switch (param.Length)
            {
                case 1:
                    return LanguageMgr.GetTranstion(tran, param[0]);
                    
                case 2:

                    return LanguageMgr.GetTranstion(tran, param[0], param[1]);

                case 3:
                    return LanguageMgr.GetTranstion(tran, param[0], param[1], param[2]);

                default:
                    break;
            }

            return LanguageMgr.GetTranstion(tran);
        }

        public Dictionary<string, SkeletonDataAsset> SkeleDir = new Dictionary<string, SkeletonDataAsset>();

        public SkeletonDataAsset GetSkeletonDataAssetByName(string bundleName, string spineAssetName)
        {
            if (!SkeleDir.ContainsKey(bundleName))
            {
                SkeletonDataAsset sda = LoadSkeletonDataAsset(bundleName, spineAssetName);

                SkeleDir.Add(bundleName, sda);

                return sda;
            }

            return SkeleDir[bundleName];
        }

        public SkeletonDataAsset LoadSkeletonDataAsset(string spineAssetBundleName, string spineAssetName)
        {
#if UNITY_EDITOR
            return LoadSkeletonDataAssetInEditor(spineAssetBundleName, spineAssetName);
            //return LoadSkeletonDataAssetInAssetBundle(spineAssetBundleName, spineAssetName);
#else
           return LoadSkeletonDataAssetInAssetBundle(spineAssetBundleName, spineAssetName);
#endif

        }

        private SkeletonDataAsset LoadSkeletonDataAssetInAssetBundle(string spineAssetBundleName, string spineAssetName)
        {
            return AssetMgr.Instance.LoadAsset<SkeletonDataAsset>(spineAssetBundleName, $"{spineAssetName}_SkeletonData.asset");
        }

        // 加载本地资源赋值
        private SkeletonDataAsset LoadSkeletonDataAssetInEditor(string spineAssetBundleName, string spineAssetName)
        {
#if UNITY_EDITOR

            string basePath = $"{AB_ResFilePath.abAllSpinesRootDir}/{spineAssetBundleName}/{spineAssetName}";
            return UnityEditor.AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>($"{basePath}_SkeletonData.asset");
#endif

            return LoadSkeletonDataAssetInAssetBundle(spineAssetBundleName, spineAssetName);
        }

        //private void FormetColor(string color)
        //{
        //    return Color.
        //}
        
        //是否满足顾客条件
        public bool IsFillConditByRotia(int rotia)
        {
            int value = UnityEngine.Random.Range(0, 101);

            return rotia <= value;
        }
    }
}
