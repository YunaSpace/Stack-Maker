using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    [SerializeField] private Transform mapHolder;
    [SerializeField] private GroundNode groundNodePrefab;
    [SerializeField] private UngroundNode ungroundNodePrefab;
    [SerializeField] private GameObject borderNodePrefab;
    [SerializeField] private GameObject pushNodePrefab;
    [SerializeField] private GameObject goalNodePrefab;

    [SerializeField] private LevelRecord mapRecord;

    [ContextMenu("Save Map")]
    private void SaveMap()
    {
        mapRecord.Nodes.Clear();

        var groundNodeAmount = 0;
        var ungroundNodeAmount = 0;
        var children = mapHolder.Cast<Transform>();

        foreach (var child in children)
        {
            var groundNode = child.GetComponent<GroundNode>();
            var ungroundNode = child.GetComponent<UngroundNode>();
            var pushNode = child.GetComponent<PushNode>();
            var surfaceNode = child.GetComponent<GoalNode>();

            var nodeData = new NodeData();

            if (groundNode != null)
            {
                groundNodeAmount++;
                nodeData.Type = NodeType.GroundNode;
            }
            else if (ungroundNode != null)
            {
                ungroundNodeAmount++;
                nodeData.Type = NodeType.UngroundNode;
            }
            else if (pushNode != null)
            {
                groundNodeAmount++;
                nodeData.Type = NodeType.PushNode;
            }
            else if (surfaceNode != null)
            {
                nodeData.Type = NodeType.GoalNode;
            }
            else
            {
                nodeData.Type = NodeType.BorderNode;
            }

            nodeData.Position = child.transform.localPosition;
            nodeData.Rotation = child.transform.rotation.eulerAngles;

            mapRecord.Nodes.Add(nodeData);
            mapRecord.StackNodeAmount = groundNodeAmount;
        }

        EditorUtility.SetDirty(mapRecord);
        AssetDatabase.SaveAssetIfDirty(mapRecord);
    }

    [ContextMenu("Load Level")]
    private void LoadLevel()
    {
        mapHolder.DestroyAllChildrenImmediately();

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
    }
}