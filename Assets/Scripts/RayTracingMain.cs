using UnityEngine;

public class RayTracingMain : MonoBehaviour
{
    public ComputeShader RayTracingShader;

    private RenderTexture _target;

    private Camera _camera;

    public Texture SkyboxTexture;

    [SerializeField] Vector3 focus_a = new Vector3(0.0f, 0.0f, 1.0f);
    [SerializeField] Vector3 focus_b = new Vector3(0.0f, 1.0f, 0.0f);
    [SerializeField] Vector3 focus_c = new Vector3(1.0f, 0.0f, 0.0f);
    [SerializeField] float focal_length = 0.25f;
    [SerializeField] float f_stop = 1.0f;
    [SerializeField] Vector3 middle = new Vector3(0.0f, 0.0f, 1.0f);
    [SerializeField] float sensor_size = 1.0f;
    [SerializeField] float s = 1.0f;

    // float3 _FocusA;
    // float3 _FocusB;
    // float3 _FocusC;
    // float _FocalLength;
    // float _FStop;
    // float3 _Middle;
    // float _SensorSize;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void SetShaderParameters()
    {
        RayTracingShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
        RayTracingShader.SetTexture(0, "_SkyboxTexture", SkyboxTexture);
        int frameCount = Time.frameCount; // or keep your own counter
        RayTracingShader.SetInt("_FrameCount", frameCount);

        RayTracingShader.SetVector("_FocusA", focus_a);
        RayTracingShader.SetVector("_FocusB", focus_b);
        RayTracingShader.SetVector("_FocusC", focus_c);
        RayTracingShader.SetFloat("_FocalLength", focal_length);
        RayTracingShader.SetFloat("_FStop", f_stop);
        RayTracingShader.SetVector("_Middle", middle);
        RayTracingShader.SetFloat("_SensorSize", sensor_size);
        RayTracingShader.SetFloat("_S", s);

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
            _target = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
    }
}