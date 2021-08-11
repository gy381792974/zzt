using EazyGF;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastMgr : Singleton<RaycastMgr>
{
    private Ray ray;
    private RaycastHit raycastHit;

    private void Update()
    {
#if UNITY_EDITOR
        ShootRay_Mouse();
#else
       ShootRay_Touch();
#endif
    }

    //按下鼠标后是否移动过了
    private bool mouseMoved = false;
    //允许鼠标移动的距离，在该距离内认为鼠标并未移动过--可以根据需要手动调节
    private float mouseMoveDistance = 0.02f;
    private void ShootRay_Mouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                mouseMoved = true;
                return;
            }
            mouseMoved = false;
        }
        else if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            if (mouseX >= mouseMoveDistance || mouseY >= mouseMoveDistance)
            {
                mouseMoved = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //如果没有移动，则根据射线Tag进行判断
            if (!mouseMoved)
            {
                ShootPointRay(Input.mousePosition);
            }
        }
    }

    //手指刚按下的坐标
    private Vector2 enterVec2;
    //手指刚按下的时间
    private float enterTime;
    //第一个按下的手指
    private Touch firsTouch;
    //允许手指移动的距离
    private float fingerMoveDistance = 20;
    //允许点击的时间
    private float clickTime = 1.5f;

    void ShootRay_Touch()
    {
        if (Input.touchCount > 0)
        {
            firsTouch = Input.GetTouch(0);

            if (firsTouch.phase == TouchPhase.Began)
            {
                //UI阻挡
                if (EventSystem.current.IsPointerOverGameObject(firsTouch.fingerId))
                {
                    mouseMoved = true;
                    return;
                }

                mouseMoved = false;
                enterVec2 = firsTouch.position;
                enterTime = Time.time;
            }
            else if (firsTouch.phase == TouchPhase.Moved )
            {
                if (Vector2.Distance(firsTouch.position, enterVec2) > fingerMoveDistance)
                {
                    mouseMoved = true;
                }
            }
            else if (firsTouch.phase == TouchPhase.Ended)
            {
                if (!mouseMoved && Time.time - enterTime <= clickTime)
                {
                    ShootPointRay(firsTouch.position);
                }
            }
        }
    }

    //private bool IsPointerOverUIObject(Vector2 screenPosition)
    //{
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //    eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    //    return results.Count > 0;
    //}

    private void ShootPointRay(Vector3 inputMousePostion)
    {
        if (CarmerMgr.Instance.MainCamera != null)
        {
            ray = CarmerMgr.Instance.MainCamera.ScreenPointToRay(inputMousePostion);
            if (Physics.Raycast(ray, out raycastHit, 100))
            {
                HandleCompareTag();
            }
        }
    }

    private void HandleCompareTag()
    {
        Transform hitTrans= raycastHit.transform;
        //点击到收银台
        if (hitTrans.CompareTag(TagMgr.Instance.GetTag(TagType.CashCheck)))
        {
           //Debug.Log("点击收银台"+hitTrans.GetComponent<CashCheckBandID>().CashCheckID);

        }
        else if (hitTrans.CompareTag(TagMgr.Instance.GetTag(TagType.Reception)))
        {
          
        }
    }

}
