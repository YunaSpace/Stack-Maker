using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum LevelAction { Replay, Start, Next }
public enum LevelState { Play, Pause, Win, Lose }

public class LevelManager : MonoBehaviour
{
    public static Action<int> OnLevelChanged { get; set; }
    public static Action<LevelAction> OnLevelAction { get; set; }
    public static Action<LevelState> OnLevelStateChanged { get; set; }
    public static Action<int, int> OnStackNodeChanged { get; set; }
    public static Action OnCameraForceFocused { get; set; }

    public int StackNodeAmount => levelRecords[currentLevel].StackNodeAmount;

    public LevelState LevelState;
    public int StackPoint;

    [SerializeField] private PlayerManager player; 

    [SerializeField] private Transform mapHolder;

    [SerializeField] private List<GameObject> nodePrefabs = new();

    [SerializeField] private int currentLevel = 0;

    [SerializeField] private List<LevelRecord> levelRecords = new();

    private bool readyToStartNextLevel = false;
    private bool isMasking = false;

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
        player.OnInitialize();

        ClearLevel();
        LoadLevel(0);
    }

    private void HandleLevelAction(LevelAction action)
    {
        if (action == LevelAction.Replay)
        {
            currentLevel--;
            
            ReadyToStartLevel();
        }
        else if (action == LevelAction.Next)
        {
            ReadyToStartLevel();
        }
    }

    private void ReadyToStartLevel()
    {
        isMasking = true;

        readyToStartNextLevel = true;

        ViewManager.OnNextLevelPanelShowed?.Invoke(true);
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

        OnLevelChanged?.Invoke(currentLevel);
    }

    private void HandleLevelStateChanged(LevelState state)
    {
        LevelState = state;
    }

    private void ClearLevel()
    {
        mapHolder.DestroyAllChildren();
    }

    private void LoadLevel(int level)
    {
        var mapRecord = levelRecords[level];

        foreach (var nodeData in mapRecord.Nodes)
        {
            Instantiate(nodePrefabs[(int)nodeData.Type], nodeData.Position, Quaternion.Euler(nodeData.Rotation), mapHolder);
        }

        if (isMasking)
        {
            ViewManager.OnNextLevelPanelShowed?.Invoke(false);
        }

        LevelManager.OnCameraForceFocused?.Invoke();
    }
}