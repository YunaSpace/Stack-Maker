using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Vector3 minDistance;
    [SerializeField] private Vector3 maxDistance;

    [SerializeField] private Vector3 currentDistance;

    private void Awake()
    {
        LevelManager.OnStackNodeChanged += OnStackNodeChanged;
        LevelManager.OnCameraForceFocused += OnCameraForgeFocused;
    }

    private void OnDestroy()
    {
        LevelManager.OnStackNodeChanged -= OnStackNodeChanged;
        LevelManager.OnCameraForceFocused -= OnCameraForgeFocused;
    }

    private void Start()
    {
        currentDistance = minDistance;
    }

    private void Update()
    {
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, currentDistance, Time.deltaTime * 5f);
    }

    private void OnStackNodeChanged(int stackCount, int groundNodeAmount)
    {
        currentDistance = Vector3.Lerp(minDistance, maxDistance, (float)stackCount / groundNodeAmount);
    }

    private void OnCameraForgeFocused()
    {
        currentDistance = minDistance;
        cameraTransform.localPosition = minDistance;
        this.transform.localPosition = new Vector3(-6.5f, 0, -2.5f);
    }
}