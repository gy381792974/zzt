using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class StartPanel2
	{
		private StartPanel2Data mPanelData = null;

		[SerializeField] private Image BackPanel_img;
		[SerializeField] private Image Title_img;
		[SerializeField] private Transform progressBG_trans;
		[SerializeField] private Image progressBG_img;
		[SerializeField] private Transform progress_trans;
		[SerializeField] private Image progress_img;
		[SerializeField] private Text progress_text;
		[SerializeField] private Button Start_btn;
	}
}
