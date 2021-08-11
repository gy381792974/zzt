using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace EazyGF
{
	public partial class ComRewardPanel
	{
		private ComRewardPanelData mPanelData = null;

        [SerializeField] private Button getBtn;

        [SerializeField] private List<AwardGrid> awardGrids;

        [SerializeField] private GameObject offLine_obj;
    }
}
