using UnityEditor;
using UnityEngine;

public class AddShaderToLoadList
{

 //[MenuItem("Assets/添加Shader为预加载项", false, 888)]
    public static void AddShader()
    {
        string[] myShaders = new string[]{
            "Legacy Shaders/Diffuse",
            "Hidden/CubeBlur",
            "Hidden/CubeCopy",
            "Hidden/CubeBlend",
            "Sprites/Default",
            "UI/Default",
            "UI/DefaultETC1",//以上为默认系统添加的Shader
            "UI/Default Gray"//图片灰色材质
        };

        SerializedObject graphicsSettings = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0]);
        SerializedProperty it = graphicsSettings.GetIterator();
        SerializedProperty dataPoint;
        while (it.NextVisible(true))
        {
            if (it.name == "m_AlwaysIncludedShaders")
            {
                it.ClearArray();
                for (int i = 0; i < myShaders.Length; i++)
                {
                    it.InsertArrayElementAtIndex(i);
                    dataPoint = it.GetArrayElementAtIndex(i);
                    dataPoint.objectReferenceValue = Shader.Find(myShaders[i]);
                }
            }
            graphicsSettings.ApplyModifiedProperties();
        }
    }
}
