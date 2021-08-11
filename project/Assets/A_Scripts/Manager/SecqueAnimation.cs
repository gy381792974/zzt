using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecqueAnimation : MonoBehaviour
{
    [SerializeField] public int rol=0;
    [SerializeField] public int col=0;
    [SerializeField] public int speed=10;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField]
    private SpriteRenderer _SpriteRenderer
    {
        get
        {
            if (spriteRenderer == null)
            {
                spriteRenderer= GetComponent<SpriteRenderer>();
            }

            return spriteRenderer;
        }
    }
    private void Start()
    {
        ShaderHelp.PlayUIAnimation(_SpriteRenderer, rol, col, speed);
    }

    public void PauseAnimation()
    {
        ShaderHelp.PauseUIAnimation(_SpriteRenderer);
    }

    public void ResumeAnimation()
    {
        ShaderHelp.ResumeUIAnimation(_SpriteRenderer);
    }

}
