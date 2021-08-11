using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EazyGF;
using System;

public class ChairItem : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    Image img;
    ChairPanel panel;
    int index;
    public Image Img { get => img; private set => img = value; }
    public int Index { get => index; private set => index = value; }
    public BuildDataModel MBuildDataModel { get => buildDataModel; set => buildDataModel = value; }

    private void Awake()
    {
        panel = UIMgr.GetUI<ChairPanel>();
        Img = toggle.targetGraphic.GetComponent<Image>();
        toggle.group = this.GetComponentInParent<ToggleGroup>();
        toggle.onValueChanged?.AddListener((isOn) => { panel.ClickToggleChangeImage(this, isOn); });
    }

    public void SetChairData(Chair_Property chair, int index)
    {
        Img.sprite = AssetMgr.Instance.LoadTexture("BuildTex", chair.icon);
        Index = index;
    }


    BuildDataModel buildDataModel;

    internal void BindData(BuildDataModel buildDataModel)
    {
        this.buildDataModel = buildDataModel;
    }

    //public void SetCookData(Kitchen_Property kitchen, int index)
    //{
    //    Img.sprite = AssetMgr.Instance.LoadTexture("BuildTex", kitchen.Icon);
    //    Index = index;
    //}

}
