using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class RoleDialoguePanel
	{
		private RoleDialoguePanelData mPanelData = null;

		[SerializeField] private Image Icon_img;
		[SerializeField] private Button left_btn;
		[SerializeField] private Text left_text;
		[SerializeField] private Button right_btn;
		[SerializeField] private Text righ_text;
		[SerializeField] private Text content_text;
		[SerializeField] private Text Title_Text;
		[SerializeField] private Button middle_btn;
		[SerializeField] private Text middle_text;
	}
}
