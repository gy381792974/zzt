using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class MaGicPanel
	{
		private MaGicPanelData mPanelData = null;

		[SerializeField] private Image DaojuPanelBg_img;
		[SerializeField] private Image DaoJuTitle_img;
		[SerializeField] private Image DaojuBg_img;
		[SerializeField] private Image Daoju_img;
		[SerializeField] private Text DaojuTxt_text;
		[SerializeField] private Button NumMinusShow_btn;
		[SerializeField] private Button NumMinusHide_btn;
		[SerializeField] private Image BuyNum_img;
		[SerializeField] private Text BuyNumTxt_text;
		[SerializeField] private Button NumADDShow_btn;
		[SerializeField] private Button NumADDHide_btn;
		[SerializeField] private Text GongNengTxt_text;
		[SerializeField] private Button DaojuBuy_btn;
		[SerializeField] private Image GoldIcon_img;
		[SerializeField] private Text GoldTxt_text;
		[SerializeField] private Button DaojuCloseBtn_btn;
	}
}
