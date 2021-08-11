using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class CubeFailPanel
	{
		private CubeFailPanelData mPanelData = null;

		[SerializeField] private Text Text_text;
		[SerializeField] private Button revival_btn;
		[SerializeField] private Text revival_text;
		[SerializeField] private Button restart_btn;
		[SerializeField] private Text coin_text;
		[SerializeField] private Button close_btn;
	}
}
