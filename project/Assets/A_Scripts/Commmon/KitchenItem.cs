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
    int index;
    int curLevel = 1;
    public Image Img { get => img; private set => img = value; }
    public int Index { get => index; private set => index = value; }
    public int CurLevel { get => curLevel; private set => curLevel = value; }

    private void Awake()
    {
        panel = UIMgr.GetUI<CookPanel>();
        Img = sw.targetImg;
        sw.group = this.GetComponentInParent<SwitchGroup>();
        sw.onValueChanged?.AddListener((isOn) => { panel.ClickSwitch(this, isOn); });
    }

    public void SetKitchenData(Kitchen_Property kitchen, int index, int curLevel)
    {
        Img.sprite = AssetMgr.Instance.LoadTexture("BuildIcon", kitchen.Icon);
        Img.SetNativeSize();
        Index = index;
        CurLevel = curLevel;
    }

    public void SetSwitch(bool isOn)
    {
        sw.IsOn = isOn;
    }
    public void UpdateData(int curLevel)
    {
        this.curLevel = curLevel;
    }
}
