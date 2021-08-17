using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class CookPanel
	{
		private CookPanelData mPanelData = null;

		[SerializeField] private Image title_img;
		[SerializeField] private Button left_btn;
		[SerializeField] private Button right_btn;
		[SerializeField] private Text name_text;
		[SerializeField] private Text desc_text;
		[SerializeField] private Image Icon_img;
		[SerializeField] private Text curLevel_text;
		[SerializeField] private GameObject cook_obj;
		[SerializeField] private Text cookLock_text;
		[SerializeField] private GameObject unlockCook_obj;
		[SerializeField] private Image cookFill_img;
		[SerializeField] private Text cookMaxLevel_text;
		[SerializeField] private Text unlockCook_text;
		[SerializeField] private Button build_btn;
		[SerializeField] private Text build_text;
		[SerializeField] private Text Coin_text;
	}
}
