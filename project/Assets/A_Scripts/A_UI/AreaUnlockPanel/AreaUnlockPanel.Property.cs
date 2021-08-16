using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class AreaUnlockPanel
	{
		private AreaUnlockPanelData mPanelData = null;

		[SerializeField] private Button build_btn;
		[SerializeField] private Text build_text;
		[SerializeField] private Text coin_text;
		[SerializeField] private Image title_img;
		[SerializeField] private Text areaDes_text;
		[SerializeField] private Image area_img;
		[SerializeField] private Button close_btn;
	}
}
