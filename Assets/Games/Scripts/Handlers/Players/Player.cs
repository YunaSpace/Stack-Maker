using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public enum Direction
    {
        Left, Right, Foward, Back
    }

    public class PlayerNode
    {
        public GameObject Node;
        public bool Visible;
    }

    public bool IsNearlyWin = false;
    public Vector3 NextPosition;

    [SerializeField] private Transform model;
    [SerializeField] private Animator modelAnimator;

    [SerializeField] private Transform raycaster;

    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private Transform playerNodeHolder;
    [SerializeField] private GameObject playerNodePrefab;
    [SerializeField] protected int maxStackAmount = 30;

    private int stackCount
    {
        get => GameManager.Instance.LevelManager.StackPoint;
        set => GameManager.Instance.LevelManager.StackPoint = value;
    }

    private List<PlayerNode> playerNodes = new();
    private int groundNodeAmount = 52;

    private List<Vector3> targetPositions = new();
    private int currentTargetPosition;
    private bool isMoving = false;
    private Vector3 startPointPosition;
    private Vector3 moveDirection;

    private Vector3 modelRotation;

    private void Awake()
    {
        CreateStack();
    }

    private void Update()
    {
        HandleInput();

        MovePlayer();

        model.localRotation = Quaternion.Lerp(model.localRotation, Quaternion.Euler(modelRotation), Time.deltaTime * 10f);
    }

    public void Initialize()
    {
        this.transform.position = new Vector3(-6.5f, 0, -2.5f);
        model.localPosition = new Vector3(0, 0.3f, 0);

        isMoving = false;
        targetPositions.Clear();
        moveDirection = Vector3.zero;
        currentTargetPosition = 0;

        startPointPosition = Vector3.zero;

        stackCount = 0;
        foreach (var node in playerNodes)
        {
            node.Node.SetActive(false);
            node.Visible = false;
        }

        modelAnimator.Play("Idle");
    }

    public void DetectNode(Vector3 direction)
    {
        NextPosition = this.transform.position;

        if (Physics.Raycast(raycaster.position, direction, out RaycastHit hit, 1.2f))
        {
            var node = hit.collider.GetComponent<StackNode>();

            node?.DetectNode(direction, this);
        }
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

    public void StackNode(bool isStack = true)
    {
        stackCount += isStack ? 1 : -1;

        if (stackCount < 0)
        {
            LevelManager.OnLevelStateChanged?.Invoke(LevelState.Lose);
            isMoving = false;
        }

        stackCount = Mathf.Clamp(stackCount, 0, groundNodeAmount);

        LevelManager.OnStackNodeChanged?.Invoke(stackCount, groundNodeAmount);

        UpdateStackVisual();
    }

    private void UpdateStackVisual()
    {
        var ratio = (float)stackCount / groundNodeAmount;
        var activeCount = Mathf.RoundToInt(ratio * maxStackAmount);

        for (int i = 0; i < playerNodes.Count; i++)
        {
            var shouldBeActive = i < activeCount;

            if (playerNodes[i].Visible != shouldBeActive)
            {
                playerNodes[i].Node.SetActive(shouldBeActive);
                playerNodes[i].Visible = shouldBeActive;
            }
        }

        var y = 0.3f + (activeCount - 1) * 0.125f;

        model.localPosition = new Vector3(0, y, 0);
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

                DetectNode(moveDirection);
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

                    if (IsNearlyWin)
                    {
                        modelAnimator.Play("Win");
                        modelRotation = new Vector3(0, 180, 0);

                        LevelManager.OnLevelStateChanged?.Invoke(LevelState.Win);
                        IsNearlyWin = false;
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

    private void CreateStack()
    {
        for (int i = 0; i < maxStackAmount; i++)
        {
            var node = Instantiate(playerNodePrefab, Vector3.zero, Quaternion.identity, playerNodeHolder);
            node.SetActive(false);
            node.transform.localPosition = new Vector3(0, 0.35f + i * 0.125f, 0);

            playerNodes.Add(new()
            {
                Node = node,
                Visible = false
            });
        }
    }
}