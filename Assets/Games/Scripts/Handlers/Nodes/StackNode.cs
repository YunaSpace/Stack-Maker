using UnityEngine;

public abstract class StackNode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerTriggered();
        }
    }

    public virtual void DetectNode(DetectData detectData)
    {
    }

    protected virtual void OnPlayerTriggered()
    {

    }

    protected bool DetectNextNode(DetectData detectData)
    {
        if (Physics.Raycast(transform.position, detectData.Direction, out RaycastHit hit, 1.2f))
        {
            var nextNode = hit.collider.GetComponent<StackNode>();
            
            if (nextNode != null)
            {
                nextNode.DetectNode(detectData);
                return true;
            }
        }

        return false;
    }
}