using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class FangKuangIngPanel
	{
		private FangKuangIngPanelData mPanelData = null;

		[SerializeField] private Button HelpBtn_btn;
		[SerializeField] private Image GoldKuang_img;
		[SerializeField] private Image GoldIcon_img;
		[SerializeField] private Text GoldTxt_text;
		[SerializeField] private Image StarKuang_img;
		[SerializeField] private Image StarIcon_img;
		[SerializeField] private Text StarTxt_text;
		[SerializeField] private Button PauseBtn_btn;
		[SerializeField] private Image SlideDown_img;
		[SerializeField] private Image KongSlide_img;
		[SerializeField] private Image FillSlide_img;
		[SerializeField] private Image StarLeft_img;
		[SerializeField] private Image HideStarLeft_img;
		[SerializeField] private Image StarMiddle_img;
		[SerializeField] private Image HideStarMiddle_img;
		[SerializeField] private Image starRight_img;
		[SerializeField] private Image HidestarRight_img;
		[SerializeField] private Text LevelText_text;
		[SerializeField] private Image UpUI_img;
		[SerializeField] private GameObject Daoju1Pos_obj;
		[SerializeField] private Button HuiTuiButton_btn;
		[SerializeField] private Image HuiTuiNum_img;
		[SerializeField] private Text HuiTuiNumTxt_text;
		[SerializeField] private Button HuituiBuyBtn_btn;
		[SerializeField] private GameObject Daoju2Pos_obj;
		[SerializeField] private Button MoFaButton_btn;
		[SerializeField] private Image MoFaNum_img;
		[SerializeField] private Text MoFaNumTxt_text;
		[SerializeField] private Button MofaBuyBtn_btn;
		[SerializeField] private GameObject Daoju3Pos_obj;
		[SerializeField] private Button DaLuanButton_btn;
		[SerializeField] private Image DaLuanNum_img;
		[SerializeField] private Text DaLuanNumTxt_text;
		[SerializeField] private Button DaLuanBuyBtn_btn;
	}
}
