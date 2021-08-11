using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class ChairPanel
	{
		private ChairPanelData mPanelData = null;

		[SerializeField] private Button left_btn;
		[SerializeField] private Button right_btn;
		[SerializeField] private Text name_text;
		[SerializeField] private Text desc_text;
		[SerializeField] private Image Icon_img;
		[SerializeField] private Text curLevel_text;
		[SerializeField] private GameObject chair_obj;
		[SerializeField] private Text chairLock_text;
		[SerializeField] private GameObject chairUnLock_obj;
		[SerializeField] private Image chairFill_img;
		[SerializeField] private Text chairMaxLevel_text;
		[SerializeField] private Text curStage_text;
		[SerializeField] private Text nextStage_text;
		[SerializeField] private Button build_btn;
		[SerializeField] private Text build_text;
		[SerializeField] private Text Coin_text;
		[SerializeField] private Transform Grid_trans;
	}
}
