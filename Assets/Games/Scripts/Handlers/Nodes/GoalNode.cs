using UnityEngine;

public class GoalNode : StackNode
{
    public override void DetectNode(Vector3 direction, Player player)
    {
        player.IsNearlyWin = true;

        player.AddTargetPosition(transform.position);
        player.StartMoving();
    }
}