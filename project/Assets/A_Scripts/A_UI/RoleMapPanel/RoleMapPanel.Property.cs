using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class RoleMapPanel
	{
		private RoleMapPanelData mPanelData = null;

		[SerializeField] private Transform ImgGrid_trans;
		[SerializeField] private Button Close_btn;
		[SerializeField] private GameObject unLock_obj;
		[SerializeField] private Image icon_img;
		[SerializeField] private GameObject lock_obj;
	}
}
