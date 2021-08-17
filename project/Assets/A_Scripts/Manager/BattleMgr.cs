using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EazyGF
{

    public class BattleMgr : MonoBehaviour
    {
        public Camera UICamera;
        public RectTransform UICanvas;

        int uiShowNum = 3;//主界面， 点击特效界面， 气泡界面

        private Vector3 lastMouPos;

        int cSLayout = 9;
        int otherLayout = 12;
        int buildLayout = 13;
        int buildAreaLayout = 14;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (UIPanelManager.Instance.GetDicShowingUI() <= uiShowNum)
                {
                    ShowDjEffect();

                    lastMouPos = Input.mousePosition;
                }
                 
                if (UIPanelManager.Instance.GetDicShowingUI() <= uiShowNum + 1) //人物对话
                {
                    if (CheckGuiRaycastObjects())
                    {
                        return;
                    }

                    OnClickSC();
                   // OnClickWaiter();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (CheckGuiRaycastObjects())
                {
                    return;
                }


                if (UIPanelManager.Instance.GetDicShowingUI() <= uiShowNum)
                {
                    if (Vector3.Distance(lastMouPos, Input.mousePosition) <= 1f)
                    {
                        OnClickBuild();

                        OnClickAreaBuild();
                    }
                }
            }

            ColorGradientUtil.Instance.UpdateSEffect();

        }

        private void OnClickCoin()
        {
            DropCoin dc = GetCoinFromMouseClick();

            if (dc != null)
            {
                dc.PackUpCoin();
            }
        }

        private void OnClickWaiter()
        {
            ClickGainGold waiter = GetWaiter();
            if (waiter != null)
            {
                waiter.GainGold();
            }
        }

        ClickGainGold GetWaiter()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out hitInfo, 1000);
            if (hit)
            {
                return hitInfo.transform.GetComponent<ClickGainGold>();
            }
            return null;
        }

        private void OnClickSC()
        {
            CustomerSpe cs = GetRigidbodyFromMouseClick();

            if (cs != null && cs.curSRoleStage == SRoleStage.Action && !cs.isClicked)
            {
                cs.MHashCode = cs.GetHashCode();
                UIMgr.ShowPanel<RoleDialoguePanel>(new RoleDialoguePanelData(cs.Data, cs.GetHashCode()));
                UIMgr.GetUI<RoleDialoguePanel>().ShowClickDialogue();
            }
        }

        void OnClickBuild()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out hitInfo, 1000, 1 << buildLayout);

            if (hit)
            {
                BuildItem buildItem = hitInfo.transform.GetComponentInParent<BuildItem>();

                int id = -1;

                if (buildItem.MBuildDataModel.CommboType >= 1)
                {
                    id = hitInfo.transform.name.ToInt();
                }

                Debug.LogWarning($"onclickName  {buildItem.gameObject.name} {hitInfo.transform.name} ");

                BuildUpgradeMgr.Instance.BuildUnLockAndUpgrade(buildItem.MBuildDataModel, id);

                //EventManager.Instance.TriggerEvent(EventKey.MoveCamerToTargetPos, buildItem.GetShowBuildBoxTf().position);
            }
        }

        void OnClickAreaBuild()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out hitInfo, 1000, 1 << buildAreaLayout);

            if (hit)
            {
                BuildAreaItem buildItem = hitInfo.transform.GetComponentInParent<BuildAreaItem>();
                BuildAreaMgr.Instance.TriggerUnlockArea(buildItem.AreaType, buildItem.id);

                //if (buildItem.AreaType == 0)
                //{
                //}
                //EventManager.Instance.TriggerEvent(EventKey.MoveCamerToTargetPos, buildItem.GetShowBuildBoxTf().position);
            }
        }

        CustomerSpe GetRigidbodyFromMouseClick()
        {
            RaycastHit hitInfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out hitInfo, 1000, 1 << cSLayout);
            if (hit)
            {
                return hitInfo.collider.GetComponent<CustomerSpe>();
            }
            return null;
        }

        DropCoin GetCoinFromMouseClick()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out hitInfo, 1000, 1 << otherLayout);
            if (hit)
            {
                return hitInfo.collider.GetComponent<DropCoin>();
            }
            return null;
        }

        ScreenHandlePanel screenHandlePanel;

        public void ShowDjEffect()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                MusicMgr.Instance.PlayMusicEff("d_click");
            }

            if (screenHandlePanel == null)
            {
                UIMgr.ShowPanel<ScreenHandlePanel>();
                screenHandlePanel = UIMgr.GetUI<ScreenHandlePanel>();
            }

            Transform eff = screenHandlePanel.dj_obj.transform;

            eff.localPosition = UICommonUtil.Instance.GetUIPosBySceenPos(UICanvas);

            eff.gameObject.SetActive(false);
            eff.gameObject.SetActive(true);

            //Debug.LogError($"width {screenWidth} height {screenHeight} \n x{Input.mousePosition.x} y {Input.mousePosition.y}");
        }

        public EventSystem eventSystem;
        public GraphicRaycaster graphicRaycaster;

        bool CheckGuiRaycastObjects()
        {
            PointerEventData eventData = new PointerEventData(eventSystem);
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> list = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, list);

            //Debug.Log(list.Count);
            //return list.
            //bool IsPOGameObject = false;IPHONE || ANDROID

#if IPHONE || ANDROID
			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
            if (EventSystem.current.IsPointerOverGameObject())
#endif
                return true;
            else
            {
                return false || list.Count > 0;
            }
        }
    }
}
