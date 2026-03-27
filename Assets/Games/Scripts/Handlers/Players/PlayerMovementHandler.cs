using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private Transform model;
    [SerializeField] private Animator modelAnimator;

    [SerializeField] private float moveSpeed = 10f;

    private PlayerManager manager;

    private bool isMoving = false;

    private Vector3 moveDirection;
    private Vector3 modelRotation;

    private List<Vector3> targetPositions = new();
    private int currentTargetPosition;

    private Vector3 startPointPosition;

    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
        manager.OnActiveNodeChanged += UpdateModelPosition;
    }

    private void Update()
    {
        HandleInput();

        MovePlayer();

        model.localRotation = Quaternion.Lerp(model.localRotation, Quaternion.Euler(modelRotation), Time.deltaTime * 10f);
    }

    public void OnInitialize()
    {
        isMoving = false;

        moveDirection = Vector3.zero;
        
        targetPositions.Clear();
        currentTargetPosition = 0;

        this.transform.position = new Vector3(-6.5f, 0, -2.5f);
        model.localPosition = new Vector3(0, 0.3f, 0);
        model.localRotation = Quaternion.Euler(modelRotation = new(0, 180, 0));

        startPointPosition = Vector3.zero;

        modelAnimator.Play("Idle");
    }

    public void OnWin()
    {
        modelAnimator.Play("Win");
        modelRotation = new Vector3(0, 180, 0);
    }

    public void EnableMovement(bool isEnabled)
    {
        isMoving = isEnabled;
    }

    public void StartMoving()
    {
        isMoving = true;
        currentTargetPosition = 0;
    }

    public void AddTargetPosition(Vector3 position)
    {
        targetPositions.Add(position);
    }

    private void HandleInput()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (isMoving)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startPointPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            var position = Input.mousePosition;
            var delta = position - startPointPosition;

            var absDeltaX = math.abs(delta.x);
            var absDeltaY = math.abs(delta.y);

            if (absDeltaX > 1 || absDeltaY > 1)
            {
                if (absDeltaX > absDeltaY)
                {
                    moveDirection = delta.x > 0 ? Vector3.right : Vector3.left;
                }
                else
                {
                    moveDirection = delta.y > 0 ? Vector3.forward : Vector3.back;
                }

                manager.OnStartDetectingNode?.Invoke(moveDirection);
            }
        }
    }

    private void MovePlayer()
    {
        if (isMoving)
        {
            if (targetPositions.Count < 1)
            {
                return;
            }

            var targetPosition = targetPositions[currentTargetPosition];
            var step = moveSpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                currentTargetPosition++;

                if (currentTargetPosition >= targetPositions.Count)
                {
                    targetPositions.Clear();
                    isMoving = false;

                    if (manager.IsNearlyWin)
                    {
                        manager.OnWin();
                    }

                    return;
                }
            }

            UpdatetRotationDirection(targetPosition);
        }
    }

    private void UpdatetRotationDirection(Vector3 direction)
    {
        var delta = (direction - transform.position).normalized;

        if (math.abs(delta.x) > math.abs(delta.z))
        {
            modelRotation = new Vector3(0, delta.x > 0 ? 90 : -90, 0);
        }
        else
        {
            modelRotation = new Vector3(0, delta.z > 0 ? 0 : 180, 0);
        }
    }

    private void UpdateModelPosition(int activeCount)
    {
        var y = 0.3f + (activeCount - 1) * 0.125f;

        model.localPosition = new Vector3(0, y, 0);
    }
}