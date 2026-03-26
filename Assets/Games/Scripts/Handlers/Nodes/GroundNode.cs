using UnityEngine;

public class GroundNode : StackNode
{
    [SerializeField] private GameObject nodePad;

    protected bool isCollected;

    public override void DetectNode(Vector3 direction, Player player)
    {
        base.player ??= player;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1.2f))
        {
            var nextNode = hit.collider.GetComponent<StackNode>();
            if (nextNode != null)
            {
                nextNode.DetectNode(direction, player);
                return;
            }
        }

        player.AddTargetPosition(transform.position);
        player.StartMoving();
    }

    protected override void OnPlayerTriggered()
    {
        if (isCollected == false)
        {
            player.StackNode(true);
            nodePad.SetActive(false);
        }

        isCollected = true;
    }
}