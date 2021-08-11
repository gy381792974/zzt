using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace EazyGF
{
	public partial class DancingGirlGamePanel
	{
		private DancingGirlGamePanelData mPanelData = null;
		[SerializeField] private List<DancingGirlGameItem> dGItemList;
		[SerializeField] private Button closeBtn;
	}
}
