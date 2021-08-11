using UnityEngine;
using EazyGF;
using DG.Tweening;

public class TextureRenderCamera : MonoBehaviour
{
    Camera renderCam;
    [SerializeField] float initOrthographicSize;
    [SerializeField] float endOrthographicSize;
    [SerializeField] float duration;
    [SerializeField] float offset;
    BitBenderGames.MobileTouchCamera cam;
    Tween tween;
    Vector2 initMinBoundary;
    // Start is called before the first frame update
    void Start()
    {
        renderCam = GetComponent<Camera>();
        cam = GetComponent<BitBenderGames.MobileTouchCamera>();
        initMinBoundary = cam.BoundaryMin;
        //renderCam.orthographicSize = cam.CamZoomMax;
    }

    private void OnEnable()
    {
        EazyGF.EventManager.Instance.RegisterEvent(EazyGF.EventKey.MoveCamerToTargetPos2, MoveCameraToTarget);
    }
    private void MoveCameraToTarget(object arg)
    {
        CameraViewMove camera = (CameraViewMove)arg;
        offset = camera.point.y > 12 ? 8 : 11;
        Vector3 tarPos = transform.forward * -21 + camera.point - transform.forward * offset;
        tarPos.y = transform.position.y;
        if (camera.type)
        {
            cam.BoundaryMin = new Vector2(initMinBoundary.x - 10, initMinBoundary.y - 10);
            //放大
            ShortcutExtensions.DOOrthoSize(renderCam, endOrthographicSize, duration);
            tween = transform.DOMove(tarPos, duration);
            tween.OnUpdate(() =>
            {
                cam.CamZoom = renderCam.orthographicSize;
            });
        }
        else
        {
            cam.BoundaryMin = initMinBoundary;
            //缩小
            ShortcutExtensions.DOOrthoSize(renderCam, initOrthographicSize, duration);
            tween = transform.DOMove(tarPos, duration);
            tween.OnUpdate(() =>
            {
                //cam.Awake();
                cam.CamZoom = renderCam.orthographicSize;
            });
            EazyGF.ColorGradientUtil.Instance.CanelCGradientEff();
        }
    }



}
