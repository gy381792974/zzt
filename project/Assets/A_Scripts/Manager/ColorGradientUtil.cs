using UnityEngine;
using UnityEditor;
using System;

namespace EazyGF
{
    public class ColorGradientUtil : SingleClass<ColorGradientUtil>
    {
        string bCGradSName = "Unlit/TextureColor";

        Material mMaterial; //渐变材质

        MeshRenderer curMeshRenderer;
        Material ogrMat; //原本的材质

        private bool isPlayerCGS = false;

        private float mbMax = 0.33f;

        public override void Init()
        {
             mMaterial = new Material(Shader.Find(bCGradSName));
             mMaterial.SetFloat("_Mengbai", mbMax);
             mMaterial.name = "CGGriad";
        }

        public void PlayerCGradientEff(MeshRenderer meshRenderer)
        {
            if (meshRenderer == null)
            {
                Debug.LogError("传过来的网格组件渲染值是null值");
                return;
            }

            if (this.curMeshRenderer != null)
            {
                this.curMeshRenderer.material = ogrMat;
            }

            this.curMeshRenderer = meshRenderer;
            ogrMat = meshRenderer.material;

            mMaterial.SetTexture("_MainTex", ogrMat.GetTexture("_MainTex"));
            meshRenderer.material = mMaterial;

            isPlayerCGS = true;
        }

        public void CanelCGradientEff()
        {
            if (curMeshRenderer != null && ogrMat != null)
            {
                curMeshRenderer.material = ogrMat;

                isPlayerCGS = false;

                curMeshRenderer = null;
            }
        }

        public void UpdateSEffect()
        {
            if (isPlayerCGS)
            {
               float value = Mathf.Abs(Mathf.Sin(Time.time * 3) * mbMax);

                //Debug.Log("vale +" + value);

               mMaterial.SetFloat("_MengBai", value);
            }
        }
    }
}
