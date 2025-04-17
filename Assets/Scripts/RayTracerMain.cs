using UnityEngine;

public class RayTracerMain : MonoBehaviour
{
    public ComputeShader RayTracingShader;
    private RenderTexture _target;
    private Camera _camera;
    public Texture SkyboxTexture;

    public Transform PlaneOrigin;
    public Transform PlaneNormalRef;

    enum CameraMode
    {
        Pinhole,
        PinholeWithAntialiasing,
        ThinLens,
        TiltShift,
    };

    [SerializeField]
    private CameraMode _cameraMode = CameraMode.Pinhole;

    [Range(-45.0f, 45.0f)]
    public float _XTilt = 0.0f;
    [Range(-45.0f, 45.0f)]
    public float _YTilt = 0.0f;

    private Vector3 _PlaneNormal = Vector3.zero;

    public bool _ShowPlaneDebug = true;
    [Range(0.01f, 3.0f)]
    public float _DefocusRadius = 0.0f;
    [Range(1.0f, 10.0f)]
    public float _FocusDistance = 4.0f;

    // list of spheres to be rendered
    public GameObject[] Spheres;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void SetShaderParameters()
    {
        RayTracingShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
        RayTracingShader.SetTexture(0, "_SkyboxTexture", SkyboxTexture);

        if (Spheres.Length != 9)
        {
            Debug.LogError("Spheres array must contain exactly 9 elements.");
            return;
        }

        for (int i = 0; i < Spheres.Length; i++)
        {
            RayTracingShader.SetVector("_Sphere" + i, new Vector4(Spheres[i].transform.position.x, Spheres[i].transform.position.y, Spheres[i].transform.position.z, Spheres[i].transform.localScale.x / 2.0f));
        }

        RayTracingShader.SetFloat("_DefocusRadius", _DefocusRadius);
        RayTracingShader.SetFloat("_FocusDistance", _FocusDistance);

        RayTracingShader.SetInt("_CameraMode", (int)_cameraMode);

        RayTracingShader.SetFloat("_XTilt", _XTilt);
        RayTracingShader.SetFloat("_YTilt", _YTilt);

        RayTracingShader.SetFloat("_PlaneNormalX", _PlaneNormal.x);
        RayTracingShader.SetFloat("_PlaneNormalY", _PlaneNormal.y);
        RayTracingShader.SetFloat("_PlaneNormalZ", _PlaneNormal.z);

        RayTracingShader.SetFloat("_PlaneOriginX", PlaneOrigin.position.x);
        RayTracingShader.SetFloat("_PlaneOriginY", PlaneOrigin.position.y);
        RayTracingShader.SetFloat("_PlaneOriginZ", PlaneOrigin.position.z);

        _PlaneNormal = PlaneNormalRef.position - PlaneOrigin.position;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetShaderParameters();
        Render(destination);
    }

    private void Render(RenderTexture destination)
    {
        // Make sure we have a current render target
        InitRenderTexture();

        // Set the target and dispatch the compute shader
        RayTracingShader.SetTexture(0, "Result", _target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        RayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        // Blit the result texture to the screen
        Graphics.Blit(_target, destination);
    }

    private void InitRenderTexture()
    {
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
        {
            // Release render texture if we already have one
            if (_target != null)
                _target.Release();

            // Get a render target for Ray Tracing
            _target = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
    }
}