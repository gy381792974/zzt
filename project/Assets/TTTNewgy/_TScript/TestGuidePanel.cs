using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EazyGF;

public class TestGuidePanel : MonoBehaviour
{
    public List<Button> buttons;
    public Transform btnsGrid;
    public CircleGuideCtrl circleGuideCtrl;
    //public MGuideCtrl mGuideCtrl;

    [ContextMenu("InitBtns")]
    public void InitBtns()
    {
        buttons.Clear();

        for (int i = 0; i < btnsGrid.childCount; i++)
        {
            buttons.Add(btnsGrid.GetChild(i).GetComponent<Button>());
            btnsGrid.GetChild(i).GetComponent<Button>().GetComponentInChildren<Text>().text = $"{i}";
        }
    }

    private void Awake()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() =>
            {
                BtnClick(index);
            });
        }
    }

    private void Start()
    {
        StartCoroutine( UICommonUtil.Instance.DelayedHandle((obj) => {
            circleGuideCtrl.Play(buttons[0].GetComponent<Image>());

            //mGuideCtrl.Play(buttons[0].GetComponent<RectTransform>());

        }, 0.5f));


    }

    private void BtnClick(int index)
    {
        index++;
        Debug.LogWarning("index" + index);
        int nextTargetIndex = index % buttons.Count;

        circleGuideCtrl.Play(buttons[nextTargetIndex].GetComponent<Image>());

         //mGuideCtrl.Play(buttons[nextTargetIndex].GetComponent<RectTransform>());
    }
}
