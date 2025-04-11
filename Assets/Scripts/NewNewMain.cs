using UnityEngine;

public class NewNewMain : MonoBehaviour
{
    public ComputeShader RayTracingShader;
    private RenderTexture _target;
    private Camera _camera;
    public Texture SkyboxTexture;

    public GameObject RedSphere;
    public GameObject GreenSphere;
    public GameObject BlueSphere;

    public float DefocusAngle = 0.0f;
    public float FocusDist = 10.0f;

    private float _Random = 0.0f;

    private void Awake()
    {
        _Random = Random.Range(0.0f, 1.0f);
        _camera = GetComponent<Camera>();
    }

    private void SetShaderParameters()
    {
        RayTracingShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
        RayTracingShader.SetTexture(0, "_SkyboxTexture", SkyboxTexture);

        // store the sphere centers and scale as a vector4
        RayTracingShader.SetVector("_RedSphere",   new Vector4(RedSphere.transform.position.x,   RedSphere.transform.position.y,   RedSphere.transform.position.z,   RedSphere.transform.localScale.x / 2.0f));
        RayTracingShader.SetVector("_GreenSphere", new Vector4(GreenSphere.transform.position.x, GreenSphere.transform.position.y, GreenSphere.transform.position.z, GreenSphere.transform.localScale.x  / 2.0f));
        RayTracingShader.SetVector("_BlueSphere",  new Vector4(BlueSphere.transform.position.x,  BlueSphere.transform.position.y,  BlueSphere.transform.position.z,  BlueSphere.transform.localScale.x  / 2.0f));

        RayTracingShader.SetFloat("_DefocusAngle", DefocusAngle);
        RayTracingShader.SetFloat("_FocusDist", FocusDist);

        RayTracingShader.SetFloat("_Random", _Random);
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