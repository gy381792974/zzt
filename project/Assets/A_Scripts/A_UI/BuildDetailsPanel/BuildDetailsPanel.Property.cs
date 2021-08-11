using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace EazyGF
{
	public partial class BuildDetailsPanel
	{
		private BuildDetailsPanelData mPanelData = null;

		[SerializeField] private Image Facilityphoto_img;
		[SerializeField] private Text FacilityTitle_text;
		[SerializeField] private Text FacilityJieshao_text;

		[SerializeField] private List<Text> infoTexts;

		[SerializeField] private Button GoldBuy_btn;
		[SerializeField] private Text GoldBuy_text;
		[SerializeField] private Button FacilityClose_btn;
	}
}
