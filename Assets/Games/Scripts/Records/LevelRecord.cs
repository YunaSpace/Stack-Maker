using System.Collections.Generic;
using UnityEngine;

public enum NodeType : byte { GroundNode, UngroundNode, BorderNode, PushNode, GoalNode }

[System.Serializable]
public class NodeData
{
    public NodeType Type;
    public Vector3 Position;
    public Vector3 Rotation;
}

[CreateAssetMenu(fileName = "Level Record", menuName = "Level Record")]
public class LevelRecord : ScriptableObject
{
    public string Name = "Level 1";
    public List<NodeData> Nodes = new();
}