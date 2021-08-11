using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderHelp
{
    private static Shader grayShader;
    private static Shader GrayShader
    {
        get
        {
            if (grayShader == null)
            {
                 grayShader = Shader.Find("UI/ImageGreyShader");
            }
            return grayShader;
        }
    }

    private static Shader uiAnimationShader;

    private static Shader UIAnimationShader
    {
        get
        {
            if (uiAnimationShader == null)
            {
                uiAnimationShader = Shader.Find("UI/SequenceFrameAnimation");
            }
            return uiAnimationShader;
        }
    }

    /// <summary>
    /// 设置图片为灰色
    /// </summary>
    /// <param name="image"></param>
    public static void SetSpriteGray(Image image)
    {
        if (image == null)
        {
            Debug.LogError("图片丢失！");
            return;
        }
        if (GrayShader == null)
        {
            Debug.LogError("找不到置灰材质");
            return;
        }
        if (image.material.shader!= GrayShader)
        {
            image.material = new Material(GrayShader);
        }

        if (image.material.GetInt("_IsGray") != -1)
        {
            image.material.SetInt("_IsGray", -1);
            image.RecalculateMasking();
        }
    }
    /// <summary>
    /// 设置图片为正常颜色
    /// </summary>
    /// <param name="image"></param>
    public static void SetSpriteNormal(Image image)
    {
        if (image == null)
        {
            Debug.LogError("图片丢失！");
            return;
        }

        if (GrayShader == null)
        {
            Debug.LogError("找不到置灰材质");
            return;
        }
        if (image.material.shader != GrayShader)
        {
            image.material = new Material(GrayShader);
        }
        if (image.material.GetInt("_IsGray") != 1)
        {
            image.material.SetInt("_IsGray", 1);
            image.RecalculateMasking();
        }
    }
    /// <summary>
    /// 设置图片材质为null
    /// </summary>
    /// <param name="image"></param>
    public static void SetNullMaterial(Image image)
    {
        if (image == null)
        {
            Debug.LogError("图片丢失！");
            return;
        }

        if (image.material != null)
        {
            image.material = null;
        } 
    }
    public static void PlayUIAnimation(Image image, int rowCount, int colCount, float speed)
    {
        if (UIAnimationShader == null)
        {
            Debug.LogError("找不到UI动画材质");
            return;
        }
        
        if (image.material.shader != UIAnimationShader)
        {
            image.material = new Material(GrayShader);
        }
        image.material.SetFloat("_RowCount", rowCount);
        image.material.SetFloat("_ColCount", rowCount);
        image.material.SetFloat("_Speed", speed);
    }

    public static void PlayUIAnimation(SpriteRenderer spriteRenderer, int rowCount, int colCount, float speed)
    {
        if (UIAnimationShader == null)
        {
            Debug.LogError("找不到UI动画材质");
            return;
        }
        
        if (spriteRenderer.material.shader != UIAnimationShader)
        {
            spriteRenderer.material = new Material(UIAnimationShader);
        }
        spriteRenderer.material.SetFloat("_RowCount", rowCount);
        spriteRenderer.material.SetFloat("_ColCount", rowCount);
        spriteRenderer.material.SetFloat("_Speed", speed);
    }

    public static void PlayUIAnimation(Material material, int rowCount, float speed)
    {
        if (UIAnimationShader == null)
        {
            Debug.LogError("找不到UI动画材质");
            return;
        }
        material.SetFloat("_RowCount", rowCount);
        material.SetFloat("_ColCount", rowCount);
        material.SetFloat("_Speed", speed);
    }


    /// <summary>
    /// 从第一帧开始播放动画
    /// </summary>
    /// <param name="spriteRenderer"></param>

    //public static void RestartUIAnimation(SpriteRenderer spriteRenderer)
    //{
    //    if (UIAnimationShader == null)
    //    {
    //        Debug.LogError("找不到UI动画材质");
    //        return;
    //    }

    //    if (spriteRenderer.material.shader != UIAnimationShader)
    //    {
    //        spriteRenderer.material = new Material(UIAnimationShader);
    //    }
    //    //spriteRenderer.material.SetFloat("_Stop", 0);
    //    //spriteRenderer.material.SetFloat("_Pause", 0);
    //    //spriteRenderer.material.SetFloat("_Time.y", 0);
    //}

    //public static void RestartUIAnimation(Material material)
    //{
    //    material.SetFloat("_Time.y", 0);
    //}

    /// <summary>
    /// 暂停序列帧动画，永远只播放第一帧
    /// </summary>
    /// <param name="spriteRenderer"></param>
    public static void PauseUIAnimation(SpriteRenderer spriteRenderer)
    {
        if (UIAnimationShader == null)
        {
            Debug.LogError("找不到UI动画材质");
            return;
        }

        if (spriteRenderer.material.shader != UIAnimationShader)
        {
            spriteRenderer.material = new Material(UIAnimationShader);
        }
        spriteRenderer.material.SetFloat("_Pause", 1);
    }
    public static void PauseUIAnimation(Material material)
    {
        if (UIAnimationShader == null)
        {
            Debug.LogError("找不到UI动画材质");
            return;
        }

        material.SetFloat("_Pause", 1);
    }

    public static void ResumeUIAnimation(SpriteRenderer spriteRenderer)
    {
        if (UIAnimationShader == null)
        {
            Debug.LogError("找不到UI动画材质");
            return;
        }

        if (spriteRenderer.material.shader != UIAnimationShader)
        {
            spriteRenderer.material = new Material(UIAnimationShader);
        }
        spriteRenderer.material.SetFloat("_Pause", 0);
    }
    public static void ResumeUIAnimation(Material material)
    {
        if (UIAnimationShader == null)
        {
            Debug.LogError("找不到UI动画材质");
            return;
        }
        material.SetFloat("_Stop", 0);
        material.SetFloat("_Pause", 0);
    }


    /// <summary>
    /// 停止序列帧动画，该动画会隐藏
    /// </summary>
    /// <param name="spriteRenderer"></param>
    public static void StopUIAnimation(SpriteRenderer spriteRenderer)
    {
        if (UIAnimationShader == null)
        {
            Debug.LogError("找不到UI动画材质");
            return;
        }

        if (spriteRenderer.material.shader != UIAnimationShader)
        {
            spriteRenderer.material = new Material(UIAnimationShader);
        }
        spriteRenderer.material.SetFloat("_Stop", 1);
    }
    /// <summary>
    /// 停止序列帧动画，该动画会隐藏
    /// </summary>
    /// <param name="spriteRenderer"></param>
    public static void StopUIAnimation(Material material)
    {
        if (UIAnimationShader == null)
        {
            Debug.LogError("找不到UI动画材质");
            return;
        }
        material.SetFloat("_Pause", 1);
        material.SetFloat("_Stop", 1);
    }
}
