using UnityEngine;
using EazyGF;
using DG.Tweening;
using System;

public class TextureRenderCamera : MonoBehaviour
{
    Camera renderCam;
    [SerializeField] float initOrthographicSize;
    [SerializeField] float endOrthographicSize;
    [SerializeField] float duration;
    float offset = 11;
    BitBenderGames.MobileTouchCamera cam;
    Sequence seq;
    Vector2 initMinBoundary;
    // Start is called before the first frame update
    void Start()
    {
        renderCam = GetComponent<Camera>();
        cam = GetComponent<BitBenderGames.MobileTouchCamera>();
        initMinBoundary = cam.BoundaryMin;
        seq = DOTween.Sequence();
        //renderCam.orthographicSize = cam.CamZoomMax;
    }

    private void OnEnable()
    {
        EazyGF.EventManager.Instance.RegisterEvent(EazyGF.EventKey.MoveCamerToTargetPos2, MoveCameraToTarget);
    }
    private void MoveCameraToTarget(object arg)
    {
        //seq.Kill();
        CameraViewMove camera = (CameraViewMove)arg;
        Vector3 tarPos = transform.forward * -21 + camera.point - transform.forward * offset;
        tarPos.y = transform.position.y;
        if (camera.type)
        {
            cam.BoundaryMin = new Vector2(initMinBoundary.x - 10, initMinBoundary.y - 10);
            //放大
            seq.Join(ShortcutExtensions.DOOrthoSize(renderCam, endOrthographicSize, duration));
            seq.Join(transform.DOMove(tarPos, duration).OnUpdate(CameraChangeViewSize));

        }
        else
        {
            cam.BoundaryMin = initMinBoundary;
            //缩小
            seq.Join(ShortcutExtensions.DOOrthoSize(renderCam, initOrthographicSize, duration));
            seq.Join(transform.DOMove(tarPos, duration).OnUpdate(CameraChangeViewSize));
            EazyGF.ColorGradientUtil.Instance.CanelCGradientEff();
        }
    }

    private void CameraChangeViewSize()
    {
        cam.CamZoom = renderCam.orthographicSize;
    }




}
