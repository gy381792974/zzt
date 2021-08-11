using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class MainPanel
	{
		private MainPanelData mPanelData = null;

		[SerializeField] private Text Goldnum_text;
		[SerializeField] private Text bottle_text;
		[SerializeField] private Text Starnum_text;
		[SerializeField] private Button Setting_btn;
		[SerializeField] private Button Customs_btn;
		[SerializeField] private Text Custons_text;
		[SerializeField] private Button Staff_btn;
		[SerializeField] private GameObject StaffRedPoint_obj;
		[SerializeField] private Button customer_btn;
		[SerializeField] private GameObject customerRedPoint_obj;
	}
}
