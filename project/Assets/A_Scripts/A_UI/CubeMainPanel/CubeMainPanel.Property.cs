using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;

namespace EazyGF
{
	public partial class CubeMainPanel
	{
		private CubeMainPanelData mPanelData = null;

		[SerializeField] private Button HelpBtn_btn;
		[SerializeField] private Image GoldIcon_img;
		[SerializeField] private Text GoldTxt_text;
		[SerializeField] private Image StarIcon_img;
		[SerializeField] private Text StarTxt_text;
		[SerializeField] private Button PauseBtn_btn;
		[SerializeField] private Image FillSlide_img;
		[SerializeField] private Image StarLeft_img;
		[SerializeField] private Image HideStarLeft_img;
		[SerializeField] private Image StarMiddle_img;
		[SerializeField] private Image HideStarMiddle_img;
		[SerializeField] private Image starRight_img;
		[SerializeField] private Image HidestarRight_img;
		[SerializeField] private Text LevelText_text;
		[SerializeField] private Image UpUI_img;

		[SerializeField] private List<SkeletonGraphic> sgs;
		[SerializeField] private List<GameObject> lockStartsObj;

		[SerializeField] private List<CubeProps> cubeProps;
    }
}
