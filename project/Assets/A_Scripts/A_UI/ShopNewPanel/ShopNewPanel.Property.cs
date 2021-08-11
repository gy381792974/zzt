using UnityEngine.UI;
using UnityEngine;

namespace EazyGF
{
    public partial class ShopNewPanel
    {
        private ShopNewPanelData mPanelData = null;

        [SerializeField] private Toggle Stall_toggle;
        [SerializeField] private Text stall_text;
        [SerializeField] private Toggle Facility_toggle;
        [SerializeField] private Text Facility_text;
        [SerializeField] private Toggle Decora_toggle;
        [SerializeField] private Text decora_text;
        [SerializeField] private Button Close_btn;
        [SerializeField] private Text starNum_text;
        [SerializeField] private Text coinNum_text;
    }
}
