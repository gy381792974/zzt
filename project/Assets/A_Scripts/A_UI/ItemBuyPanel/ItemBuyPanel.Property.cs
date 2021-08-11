using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class ItemBuyPanel
	{
		private ItemBuyPanelData mPanelData = null;

		[SerializeField] private Image Icon_img;
		[SerializeField] private Button Close_btn;
		[SerializeField] private Button left_btn;
		[SerializeField] private Button right_btn;
		[SerializeField] private Button Buy_btn;
		[SerializeField] private Text BuyItemNum_text;
		[SerializeField] private Text ItemDesc_text;
		[SerializeField] private Text ItemCostNum_text;
		[SerializeField] private Text ItemNum_text;
	}
}
