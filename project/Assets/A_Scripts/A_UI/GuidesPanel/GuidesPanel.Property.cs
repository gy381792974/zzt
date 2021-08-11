using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class GuidesPanel
	{
		private GuidesPanelData mPanelData = null;

		[SerializeField] private Image GuidesPanel_img;
		[SerializeField] private Image GuideTitlePanel_img;
		[SerializeField] private Image GuideTitle_img;
		[SerializeField] private Image LeftKuohao_img;
		[SerializeField] private Image MinRoleNum_img;
		[SerializeField] private Image MinRoleNum1_img;
		[SerializeField] private Image XieGang_img;
		[SerializeField] private Image MaxRoleNum_img;
		[SerializeField] private Image MaxRoleNum1_img;
		[SerializeField] private Image RightKuohao_img;
		[SerializeField] private Button GuideClose_btn;
		[SerializeField] private Image Guides_img;
		[SerializeField] private ScrollRect Guides_sRect;
		[SerializeField] private GameObject Content_obj;
	}
}
