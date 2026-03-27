using UnityEngine;

public class UngroundNode : StackNode
{
    [SerializeField] private GameObject nodePad;
    protected bool isPlaced;

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
        if (isPlaced == false)
        {
            PlayerManager.OnNodeStacked?.Invoke(false);
            nodePad.SetActive(true);
        }

        isPlaced = true;
    }
}