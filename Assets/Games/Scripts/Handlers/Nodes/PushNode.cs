using UnityEngine;

public class PushNode : StackNode
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject nodePad;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Vector3 playerDirection;

    protected bool isCollected;

    public override void DetectNode(Vector3 direction, Player player)
    {
        this.direction = direction;
        var worldDirection = (this.transform.position - player.NextPosition).normalized;

        playerDirection = transform.InverseTransformDirection(worldDirection);

        if (playerDirection.z < -0.5f)
        {
            this.direction = transform.right;
        }
        else if (playerDirection.x < -0.5f)
        {
            this.direction = transform.forward;
        }

        player.AddTargetPosition(player.NextPosition = this.transform.position);

        if (Physics.Raycast(transform.position, this.direction, out RaycastHit hit, 1.2f))
        {
            var nextNode = hit.collider.GetComponent<StackNode>();
            if (nextNode != null)
            {
                nextNode.DetectNode(this.direction, player);
                return;
            }
        }

        player.StartMoving();
    }

    protected override void OnPlayerTriggered()
    {
        animator.Play("Push");

        if (isCollected == false)
        {
            player.StackNode(true);
            nodePad.SetActive(false);
        }

        isCollected = true;
    }
}