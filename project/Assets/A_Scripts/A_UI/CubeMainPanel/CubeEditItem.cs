using UnityEngine;
using UnityEngine.UI;

public class CubeEditItem : MonoBehaviour
{
    public Button selBtn;

    public Image bgImg;

    public GameObject iconObj;

    int index;

    public int Index { get => index; set => index = value; }


    private void Start()
    {
        selBtn.onClick.AddListener(() =>
        {

            SetSelect(!iconObj.gameObject.activeSelf);
        });
    }

    private void SetImgBg()
    {
        //bgImg.color = iconObj.activeSelf ? Color.white : Color.gray;
    }

    public void ClearData()
    {
        SetSelect(false);
    }

    public void SetSelect(bool isSelect)
    {
        iconObj.SetActive(isSelect);
        SetImgBg();
    }
}
