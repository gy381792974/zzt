using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TCGShader : MonoBehaviour
{
    // Start is called before the first frame update
    
    Material mMaterial ;
    private float mbMax = 0.5f;

    string bCGradSName = "Unlit/TextureColor";

     public MeshRenderer meshRenderer;

    Material orgM;

    void Start()
    {

        mMaterial = new Material(Shader.Find(bCGradSName));
        mMaterial.SetFloat("_Mengbai", mbMax);
        mMaterial.name = "CGGriad";

        Debug.LogError("org");
    }

    bool isPlay = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            isPlay = !isPlay;

            if (isPlay)
            {
                orgM = meshRenderer.material;
                mMaterial.SetTexture("_MainTex", orgM.GetTexture("_MainTex"));
                meshRenderer.material = mMaterial;
            }
            else
            {
                meshRenderer.material = orgM;
            }
        }

        if (isPlay)
        {
            float value = Mathf.Abs(Mathf.Sin(mbMax * Time.time * 10) * 0.5f);

            Debug.LogError(value);

            mMaterial.SetFloat("_MengBai", value);
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.LogError("触摸到UI");
        }
        else
        {
            Debug.LogError("没有");
        }
    }
}
