using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickGainGold : MonoBehaviour, IPointerClickHandler
{
    private int goldNum;
    private int id;
    public int Id { get { return id; } set { id = value; } }
    public int GoldNum
    {
        get { return goldNum; }
        set
        {
            goldNum = value;

            StaffController.SaveStaffData(Id, goldNum);
        }
    }

    public void GainGold()
    {
        EazyGF.ItemPropsManager.Intance.AddItem((int)EazyGF.CurrencyType.Coin, GoldNum);
        GoldNum = 0;
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GainGold();
    }


}
