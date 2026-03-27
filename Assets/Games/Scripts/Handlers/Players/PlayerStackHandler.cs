using System.Collections.Generic;
using UnityEngine;

public class PlayerStackHandler : MonoBehaviour
{
    [SerializeField] private Transform raycaster;
    [SerializeField] private Transform playerNodeHolder;
    [SerializeField] private GameObject playerNodePrefab;
    [SerializeField] protected int maxStackAmount = 30;

    private int stackCount
    {
        get => GameManager.Instance.LevelManager.StackPoint;
        set => GameManager.Instance.LevelManager.StackPoint = value;
    }

    private int stackNodeAmount
    {
        get => GameManager.Instance.LevelManager.StackNodeAmount;
    }


    private PlayerManager manager;

    private List<PlayerNode> playerNodes = new();


    private DetectData detectData;

    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
        manager.OnStartDetectingNode += DetectNode;
    }

    private void Start()
    {
        detectData = new();

        CreateStack();
    }

    public void OnInitialize()
    {
        stackCount = 0;

        foreach (var node in playerNodes)
        {
            node.Node.SetActive(false);
            node.Visible = false;
        }
    }

    public void StackNode(bool isStack = true)
    {
        stackCount += isStack ? 1 : -1;

        if (stackCount < 0)
        {
            manager.OnLose();
        }

        stackCount = Mathf.Clamp(stackCount, 0, stackNodeAmount);

        LevelManager.OnStackNodeChanged?.Invoke(stackCount, stackNodeAmount);

        UpdateStackVisual();
    }

    private void DetectNode(Vector3 direction)
    {
        detectData.Position = this.transform.position;
        detectData.Direction = direction;

        if (Physics.Raycast(raycaster.position, direction, out RaycastHit hit, 1.2f))
        {
            var node = hit.collider.GetComponent<StackNode>();

            node?.DetectNode(detectData);
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

    private void UpdateStackVisual()
    {
        var ratio = (float)stackCount / stackNodeAmount;
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

        manager.OnActiveNodeChanged?.Invoke(activeCount);
    }
}