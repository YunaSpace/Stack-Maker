using UnityEngine;

public class GoalNode : StackNode
{
    public override void DetectNode(DetectData detectData)
    {
        PlayerManager.OnNearlyWin?.Invoke();

        PlayerManager.OnNextPositionAdded?.Invoke(transform.position);
        PlayerManager.OnStartMoving?.Invoke();
    }
}