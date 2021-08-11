using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class PausePanel
	{
		private PausePanelData mPanelData = null;

		[SerializeField] private Image PauseBg_img;
		[SerializeField] private Button GoOn_btn;
		[SerializeField] private Button Reset_btn;
		[SerializeField] private Button ZhuyeBtn_btn;
		[SerializeField] private Button MusicBtn_btn;
		[SerializeField] private Button SoundBtn_btn;
		[SerializeField] private Image SoundIcon_img;
		[SerializeField] private Button CloseMusic_btn;
		[SerializeField] private Button CloseSound_btn;
	}
}
