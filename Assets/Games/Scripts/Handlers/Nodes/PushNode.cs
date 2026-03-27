using UnityEngine;

public class PushNode : StackNode
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject nodePad;

    protected bool isCollected;

    public override void DetectNode(DetectData detectData)
    {
        var direction = detectData.Direction;
        var worldDirection = (this.transform.position - detectData.Position).normalized;
        var playerDirection = transform.InverseTransformDirection(worldDirection);

        if (playerDirection.z < -0.5f)
        {
            direction = transform.right;
        }
        else if (playerDirection.x < -0.5f)
        {
            direction = transform.forward;
        }

        detectData.Position = this.transform.position;
        detectData.Direction = direction;

        PlayerManager.OnNextPositionAdded?.Invoke(detectData.Position);

        if (DetectNextNode(detectData))
        {
            return;
        }

        PlayerManager.OnStartMoving?.Invoke();
    }

    protected override void OnPlayerTriggered()
    {
        animator.Play("Push");

        if (isCollected == false)
        {
            PlayerManager.OnNodeStacked?.Invoke(true);
            nodePad.SetActive(false);
        }

        isCollected = true;
    }
}