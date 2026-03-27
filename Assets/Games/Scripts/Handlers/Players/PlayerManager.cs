using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{   
    public static Action<Vector3> OnNextPositionAdded { get; set; }
    public static Action OnStartMoving { get; set; }
    public static Action<bool> OnNodeStacked { get; set; }
    public static Action OnNearlyWin { get; set; }
    public static Action<Vector3> OnPointerInputed { get; set; }

    public Action<Vector3> OnStartDetectingNode { get; set; }
    public Action<int> OnActiveNodeChanged { get; set; }

    public bool IsNearlyWin;

    private PlayerMovementHandler movementHandler;
    private PlayerStackHandler stackHandler;

    private void Awake()
    {
        movementHandler = GetComponent<PlayerMovementHandler>();
        stackHandler = GetComponent<PlayerStackHandler>();

        OnNextPositionAdded += movementHandler.AddTargetPosition;
        OnStartMoving += movementHandler.StartMoving;
        OnNodeStacked += stackHandler.StackNode;
        OnNearlyWin += SetNearlyWin;
    }

    private void OnDestroy()
    {
        OnNextPositionAdded -= movementHandler.AddTargetPosition;
        OnStartMoving -= movementHandler.StartMoving;
        OnNodeStacked -= stackHandler.StackNode;
        OnNearlyWin -= SetNearlyWin;
    }

    private void Start()
    {
        OnInitialize();
    }

    public void OnInitialize()
    {
        movementHandler.OnInitialize();
        stackHandler.OnInitialize();
    }

    public void OnDespawn()
    {

    }

    public void OnWin()
    {
        LevelManager.OnLevelStateChanged?.Invoke(LevelState.Win);
        IsNearlyWin = false;

        movementHandler.OnWin();
    }

    public void OnLose()
    {
        LevelManager.OnLevelStateChanged?.Invoke(LevelState.Lose);
        movementHandler.EnableMovement(false);
    }

    private void SetNearlyWin()
    {
        IsNearlyWin = true;
    }
}