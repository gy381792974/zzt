using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class RoleCardPanel
	{
		private RoleCardPanelData mPanelData = null;

		[SerializeField] private Image RoleCardPanel_img;
		[SerializeField] private Image RoleCardBg_img;
		[SerializeField] public Image RoleIcon_img;
		//[SerializeField] private GameObject AreaPoint_obj;
		//[SerializeField] private GameObject AreaPoint_obj1;
		//[SerializeField] private GameObject AreaPoint_obj2;
		[SerializeField] private Text Effect1;
		[SerializeField] private Text Effect2;
		[SerializeField] private Text GreenEffect1;
		[SerializeField] private Text Effect3;
		[SerializeField] private Button ClientClose_btn;
	}
}
