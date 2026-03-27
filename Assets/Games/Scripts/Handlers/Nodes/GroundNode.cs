using UnityEngine;

public class GroundNode : StackNode
{
    [SerializeField] private GameObject nodePad;

    protected bool isCollected;

    public override void DetectNode(DetectData detectData)
    {
        if (DetectNextNode(detectData))
        {
            return;
        }

        PlayerManager.OnNextPositionAdded?.Invoke(transform.position);
        PlayerManager.OnStartMoving?.Invoke();
    }

    protected override void OnPlayerTriggered()
    {
        if (isCollected == false)
        {
            PlayerManager.OnNodeStacked?.Invoke(true);
            nodePad.SetActive(false);
        }

        isCollected = true;
    }
}