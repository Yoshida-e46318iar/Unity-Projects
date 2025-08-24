using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CanvasCameraManager : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas;
    Camera renderCamera;

    void Start()
    {

    }

    public void AtachMainCamera()
    {
        Camera cam = Camera.main; // MainCamera�^�O���t���Ă���J����

        if (targetCanvas != null && cam != null)
        {
            targetCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            targetCanvas.worldCamera = cam;
        }

    }

}
