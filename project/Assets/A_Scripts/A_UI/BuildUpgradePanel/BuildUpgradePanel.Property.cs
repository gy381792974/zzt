using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class BuildUpgradePanel
	{
		private BuildUpgradePanelData mPanelData = null;

		[SerializeField] private Image title_img;
		[SerializeField] private Button close_btn;
		[SerializeField] private Image icon_img;
		[SerializeField] private Text stall_text;
		[SerializeField] private Text stallDes_text;
		[SerializeField] private Button upFood_btn;
		[SerializeField] private Text upFood_text;
		[SerializeField] private Text tip1_text;
		[SerializeField] private Image food_img;
		[SerializeField] private Text food_text;
		[SerializeField] private Text next_text;
		[SerializeField] private Image fill_img;
		[SerializeField] private Text foodMaxLevel_text;
		[SerializeField] private Text time_text;
		[SerializeField] private Button remould_btn;
		[SerializeField] private Text remould_text;
		[SerializeField] private Text tip2_text;
		[SerializeField] private Text mealNum_text;
		[SerializeField] private Transform GridMeal_trans;
		[SerializeField] private Button upgradeMeal_btn;
		[SerializeField] private Text upgradeMeal_text;
		[SerializeField] private Text queueNum_text;
		[SerializeField] private Transform GridQueue_trans;
		[SerializeField] private Button Upgrade_btn;
		[SerializeField] private Text Upgrade_text;
	}
}
