using UnityEngine;

public abstract class StackNode : MonoBehaviour
{
    protected Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player == null)
            {
                player = other.GetComponent<Player>();
            }

            OnPlayerTriggered();
        }
    }

    public virtual void DetectNode(Vector3 direction, Player player)
    {
    }

    protected virtual void OnPlayerTriggered()
    {

    }
}