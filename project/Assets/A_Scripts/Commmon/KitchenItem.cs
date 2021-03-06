using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EazyGF;

public class KitchenItem : MonoBehaviour
{
    [SerializeField] Switch sw;
    Image img;
    CookPanel panel;
    int id;
    int index;
    int curLevel = 1;
    [SerializeField] GameObject[] imgObj;
    Text levelText;
    public Image Img { get => img; private set => img = value; }
    public int Index { get => index; private set => index = value; }
    public int Id { get => id; set => id = value; }
    public int CurLevel
    {
        get => curLevel;
        private set
        {
            curLevel = value;
        }
    }

    private void Awake()
    {
        panel = UIMgr.GetUI<CookPanel>();
        Img = sw.targetImg;
        levelText = imgObj[0].GetComponentInChildren<Text>();
        sw.group = this.GetComponentInParent<SwitchGroup>();
        sw.onValueChanged?.AddListener((isOn) => { panel.ClickSwitch(this, isOn); });
    }

    public void SetKitchenData(KitchenLevel_Property kitchen, int index, int curLevel, int id)
    {
        Img.sprite = AssetMgr.Instance.LoadTexture("BuildIcon", kitchen.Icon);
        //Img.SetNativeSize();
        SetItemLevelState(kitchen);
        Index = index;
        CurLevel = curLevel;
        Id = id;
    }

    public void SetSwitch(bool isOn)
    {
        sw.IsOn = isOn;
    }

    public void UpdateData(int curLevel)
    {
        CurLevel = curLevel;
    }

    public void SetItemLevelState(KitchenLevel_Property kitchen)
    {
        imgObj[0].SetActive(kitchen.level < 4);
        levelText.text = $"{kitchen.level}";
        imgObj[1].SetActive(kitchen.level >= 4);
    }

}
