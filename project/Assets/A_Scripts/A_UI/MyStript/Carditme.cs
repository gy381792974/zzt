using EazyGF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carditme : UIBase
{
    [SerializeField] private Image Cardkuang_img; 
    [SerializeField] private Image CardRolekuangShow_img;
    [SerializeField] private Text CardRolekuang_text;
    [SerializeField] private Image CardRoleIcon_img;
    [SerializeField] private Button CardClick_btn;

    public Sprite[] CardIconArr;
    public int cardId;
    private void Start()
    {
        InitCardPanelMessage(cardId);
        CardClick_btn.onClick.AddListener(() =>
        {
            UIMgr.ShowPanel<RoleCardPanel>();
            UIMgr.GetUI<RoleCardPanel>().RoleIcon_img.sprite = CardRoleIcon_img.sprite;
            UIMgr.GetUI<RoleCardPanel>().RoleCardFunctionLanguage(cardId);
        });
    }


    /// <summary>
    /// 读取配置卡片信息
    /// </summary>
    /// <param name="card"></param>
    public void InitCardPanelMessage(int card)
    {
        switch (CARD_Data.GetCARD_DataByID(card).ID)
        {
            case 1:
                Cardkuang_img.sprite = CardRolekuangShow_img.sprite;
                CardRoleIcon_img.sprite = CardIconArr[0];
                CardRolekuang_text.text = LanguageMgr.GetTranstion(7, 1);
                break;
            case 2:
                Cardkuang_img.sprite = CardRolekuangShow_img.sprite;
                CardRoleIcon_img.sprite = CardIconArr[1];
                CardRolekuang_text.text = LanguageMgr.GetTranstion(8, 1);
                break;
            case 3:
                Cardkuang_img.sprite = CardRolekuangShow_img.sprite;
                CardRoleIcon_img.sprite = CardIconArr[2];
                CardRolekuang_text.text = LanguageMgr.GetTranstion(9, 1);
                break;
            case 4:
                Cardkuang_img.sprite = CardRolekuangShow_img.sprite;
                CardRoleIcon_img.sprite = CardIconArr[3];
                CardRolekuang_text.text = LanguageMgr.GetTranstion(10, 1);
                break;
            case 5:
                Cardkuang_img.sprite = CardRolekuangShow_img.sprite;
                CardRoleIcon_img.sprite = CardIconArr[4];
                CardRolekuang_text.text = LanguageMgr.GetTranstion(8, 1);
                break;
            case 6:
                Cardkuang_img.sprite = CardRolekuangShow_img.sprite;
                CardRoleIcon_img.sprite = CardIconArr[5];
                CardRolekuang_text.text = LanguageMgr.GetTranstion(10, 1);
                break;
            case 7:
                Cardkuang_img.sprite = CardRolekuangShow_img.sprite;
                CardRoleIcon_img.sprite = CardIconArr[6];
                CardRolekuang_text.text = LanguageMgr.GetTranstion(7, 1);
                break;
            case 8:
                Cardkuang_img.sprite = CardRolekuangShow_img.sprite;
                CardRoleIcon_img.sprite = CardIconArr[7];
                CardRolekuang_text.text = LanguageMgr.GetTranstion(8, 1);
                break;
            case 9:
                Cardkuang_img.sprite = CardRolekuangShow_img.sprite;
                CardRoleIcon_img.sprite = CardIconArr[8];
                CardRolekuang_text.text = LanguageMgr.GetTranstion(8, 1);
                break;
            case 10:
                Cardkuang_img.sprite = CardRolekuangShow_img.sprite;
                CardRoleIcon_img.sprite = CardIconArr[9];
                CardRolekuang_text.text = LanguageMgr.GetTranstion(9, 1);
                break;
            default:
                CardClick_btn.interactable = false;
                break;
        }
    }


}
