using UnityEngine;

public class SCR_ScopeController : MonoBehaviour
{
    [field: Header("_")]
    [field: SerializeField] private FullScreenPassRendererFeature scopeRenderer = null;

    [field: Space(10), Header("_")]
    [field: SerializeField] private Material scopeMaterial = null;

    [field: Space(10), Header("_")]
    [field: SerializeField] private Camera scopeCamera = null;

    [field: Space(10), Header("_")]
    [field: SerializeField, Range(1, 90)] private float scopeZoomTargetFov = 15;
    [field: SerializeField, Min(0)] private float scopeZoomAcceleration = 2.5f;

    [field: Space(10), Header("_")]
    [field: SerializeField] private Vector2 scopeSwayAmplitude = Vector2.one;
    [field: SerializeField, Min(0)] private float scopeSwayAcceleration = 2.5f;

    private float currentZoomValue = 0;
    private float defaultZoomValue = 0;
    private bool isZoomed = false;
    private bool isEnabled = false;
    private Vector2 targetVector = Vector2.zero;
    private Vector2 currentVector = Vector2.zero;

    private void Awake()
    {
        currentZoomValue = scopeCamera.fieldOfView;
        defaultZoomValue = scopeCamera.fieldOfView;
    }
    private void Start() => Hide();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            isEnabled = !isEnabled;

            if (isEnabled)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        if (!isEnabled)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {            
            currentZoomValue = !isZoomed ? scopeZoomTargetFov : defaultZoomValue;

            isZoomed = !isZoomed;
        }

        targetVector.y += Input.GetAxisRaw("Mouse Y") * scopeSwayAmplitude.y * Time.deltaTime;
        targetVector.x += Input.GetAxisRaw("Mouse X") * scopeSwayAmplitude.x * Time.deltaTime;
        targetVector = Vector2.Lerp(targetVector, Vector2.zero, scopeSwayAcceleration * Time.deltaTime);
        currentVector = Vector2.Lerp(currentVector, targetVector, scopeSwayAcceleration * Time.deltaTime);
        scopeMaterial.SetVector("_VignetteOffset", currentVector);
        scopeCamera.fieldOfView = Mathf.Lerp(scopeCamera.fieldOfView, currentZoomValue, scopeZoomAcceleration * Time.deltaTime);
    }
    private void Show()
    {
        scopeRenderer.SetActive(true);
        scopeCamera.fieldOfView = currentZoomValue;
    }
    private void Hide()
    {
        scopeRenderer.SetActive(false);
        targetVector = Vector2.zero;
        currentVector = Vector2.zero;
        scopeCamera.fieldOfView = currentZoomValue;
        scopeMaterial.SetVector("_VignetteOffset", Vector2.zero);
    }
}
