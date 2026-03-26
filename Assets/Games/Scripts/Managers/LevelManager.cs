using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum LevelAction { Replay, Start, Next }
public enum LevelState { Play, Pause, Win, Lose }

public class LevelManager : MonoBehaviour
{
    public static Action<LevelAction> OnLevelAction { get; set; }
    public static Action<LevelState> OnLevelStateChanged { get; set; }
    public static Action<int, int> OnStackNodeChanged { get; set; }
    public static Action OnCameraForgeFocused { get; set; }

    public LevelState LevelState;
    public int StackPoint;

    [SerializeField] private Player player; 

    [SerializeField] private Transform mapHolder;
    [SerializeField] private GroundNode groundNodePrefab;
    [SerializeField] private UngroundNode ungroundNodePrefab;
    [SerializeField] private GameObject borderNodePrefab;
    [SerializeField] private GameObject pushNodePrefab;
    [SerializeField] private GameObject goalNodePrefab;

    [SerializeField] private int currentLevel = 1;

    [SerializeField] private List<LevelRecord> levelRecords = new();

    private bool readyToStartNextLevel = false;

    private void Awake()
    {
        OnLevelAction += HandleLevelAction;
        OnLevelStateChanged += HandleLevelStateChanged;
        ViewManager.OnNextLevelPanelMasked += StartNextLevel;
    }

    private void OnDestroy()
    {
        OnLevelAction -= HandleLevelAction;
        OnLevelStateChanged -= HandleLevelStateChanged;
        ViewManager.OnNextLevelPanelMasked -= StartNextLevel;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        player.Initialize();

        ClearLevel();
        LoadLevel();
    }

    private void HandleLevelAction(LevelAction action)
    {
        if (action == LevelAction.Replay)
        {
            Initialize();
        }
        else if (action == LevelAction.Next)
        {
            readyToStartNextLevel = true;

            ViewManager.OnNextLevelPanelShowed?.Invoke(true);
        }
    }

    private void StartNextLevel()
    {
        if (readyToStartNextLevel == false)
        {
            return;
        }

        currentLevel++;
        Initialize();

        readyToStartNextLevel = false;
    }

    private void HandleLevelStateChanged(LevelState state)
    {
        LevelState = state;

        if (state == LevelState.Pause)
        {
            GameManager.Instance.HandlePauseGame();
        }
        else if (state == LevelState.Win)
        {
            GameManager.Instance.HandleWinGame();
        }
        else if (state == LevelState.Lose)
        {
            GameManager.Instance.HandleLoseGame();
        }
    }

    private void ClearLevel()
    {
        mapHolder.DestroyAllChildren();
    }

    private void LoadLevel()
    {
        var mapRecord = levelRecords[currentLevel - 1];

        foreach (var nodeData in mapRecord.Nodes)
        {
            if (nodeData.Type == NodeType.GroundNode)
            {
                Instantiate(groundNodePrefab, nodeData.Position, Quaternion.Euler(nodeData.Rotation), mapHolder);
            }
            else if (nodeData.Type == NodeType.UngroundNode)
            {
                Instantiate(ungroundNodePrefab, nodeData.Position, Quaternion.Euler(nodeData.Rotation), mapHolder);
            }
            else if (nodeData.Type == NodeType.PushNode)
            {
                Instantiate(pushNodePrefab, nodeData.Position, Quaternion.Euler(nodeData.Rotation), mapHolder);
            }
            else if (nodeData.Type == NodeType.BorderNode)
            {
                Instantiate(borderNodePrefab, nodeData.Position, Quaternion.Euler(nodeData.Rotation), mapHolder);
            }
            else if (nodeData.Type == NodeType.GoalNode)
            {
                Instantiate(goalNodePrefab, nodeData.Position, Quaternion.Euler(nodeData.Rotation), mapHolder);
            }
        }

        if (currentLevel != 1)
        {
            ViewManager.OnNextLevelPanelShowed?.Invoke(false);
        }

        LevelManager.OnCameraForgeFocused?.Invoke();
    }
}