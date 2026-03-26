using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Vector3 minDistance;
    [SerializeField] private Vector3 maxDistance;

    private Vector3 currentDistance;

    private void Awake()
    {
        LevelManager.OnStackNodeChanged += OnStackNodeChanged;
        LevelManager.OnCameraForgeFocused += OnCameraForgeFocused;
    }

    private void OnDestroy()
    {
        LevelManager.OnStackNodeChanged -= OnStackNodeChanged;
        LevelManager.OnCameraForgeFocused -= OnCameraForgeFocused;
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
    }
}