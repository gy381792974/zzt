using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Switch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject hideBg;
    [SerializeField] GameObject showBg;
    public Image targetImg;
    private bool isOn = false;
    public SwitchGroup group;
    public bool IsOn
    {
        get { return isOn; }

        set
        {
            isOn = value;
            showBg.SetActive(isOn);
            onValueChanged?.Invoke(isOn);  //执行两次
            switchState?.Invoke(this);
        }
    }


    public OnValueChange onValueChanged;
    SwitchState switchState = new SwitchState();


    private void Awake()
    {
        hideBg?.SetActive(true);
        SetCloseState();
    }

    private void Start()
    {
        if (group != null)
        {
            group.AddSwitch(this);
            switchState.AddListener(group.SwitchesController);
            group.SetLastSwitch(this);
        }
        //IsOn = isOn;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsOn)
            return;
        IsOn = !isOn;
    }
    public void SetCloseState()
    {
        this.isOn = false;
        showBg.SetActive(IsOn);
    }


    //private void SwitchState()
    //{
    //    List<Switch> switches = group.GetSwitches();
    //    for (int i = 0; i < switches.Count; i++)
    //    {
    //        if (switches[i].isOn)
    //        {
    //            switches[i].isOn = false;
    //        }
    //    }
    //    this.isOn = true;
    //}
}
[System.Serializable]
public class OnValueChange : UnityEvent<bool> { }
public class SwitchState : UnityEvent<Switch> { }