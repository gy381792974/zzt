using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
	public partial class SettingPanel
	{
		private SettingPanelData mPanelData = null;

		[SerializeField] private Image SettingBg_img;
		[SerializeField] private Image SettingTitle_img;
		[SerializeField] private Image MusicIcon_img;
		[SerializeField] private Image CaseBG_img;
		[SerializeField] private Text MusicON_text;
		[SerializeField] private Text MusicOFF_text;
		[SerializeField] private Button MusicON_btn;
		[SerializeField] private Image MusicON_img;
		[SerializeField] private Button MusicOFF_btn;
		[SerializeField] private Image MusicOFF_img;
		[SerializeField] private Image SoundIcon_img;
		[SerializeField] private Image CaseYXBG_img;
		[SerializeField] private Text SoundON_text;
		[SerializeField] private Text SoundOFF_text;
		[SerializeField] private Button SoundON_btn;
		[SerializeField] private Image SoundON_img;
		[SerializeField] private Button SoundOFF_btn;
		[SerializeField] private Image SoundOFF_img;
		[SerializeField] private Button SettingClose_btn;
	}
}
