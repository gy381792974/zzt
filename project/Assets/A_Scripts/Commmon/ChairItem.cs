using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EazyGF;
using System;

public class ChairItem : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] GameObject[] imgObj;
    Text buildLevel;
    Image img;
    ChairPanel panel;
    int index;
    public Image Img { get => img; private set => img = value; }
    public int Index { get => index; private set => index = value; }
    public BuildDataModel MBuildDataModel { get => buildDataModel; set => buildDataModel = value; }

    private void Awake()
    {
        panel = UIMgr.GetUI<ChairPanel>();
        buildLevel = imgObj[0].GetComponentInChildren<Text>();
        Img = toggle.targetGraphic.GetComponent<Image>();
        toggle.group = this.GetComponentInParent<ToggleGroup>();
        toggle.onValueChanged?.AddListener((isOn) => { panel.ClickToggleChangeImage(this, isOn); });
    }

    public void SetChairData(Chair_Property chair, int index)
    {
        Img.sprite = AssetMgr.Instance.LoadTexture("BuildIcon", chair.icon);
        //Img.SetNativeSize();
        SetItemLevelState(chair);
        Index = index;
    }

    public void SetItemLevelState(Chair_Property chair)
    {
        imgObj[0].SetActive(chair.level < 4);
        buildLevel.text = $"{chair.level}";
        imgObj[1].SetActive(chair.level >= 4);
    }



    BuildDataModel buildDataModel;

    internal void BindData(BuildDataModel buildDataModel)
    {
        this.buildDataModel = buildDataModel;
    }

}
