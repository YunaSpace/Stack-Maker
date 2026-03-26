using Unity.Mathematics;
using UnityEngine;

public class Test : MonoBehaviour
{
    [ContextMenu("Test")]
    public void AAA()
    {
        foreach (Transform child in transform)
        {
            var position = child.localPosition;
            child.localPosition = new Vector3(position.x, 0, position.z + 0.5f);
        }
    }

    [ContextMenu("Clear Map")]
    public void ClearMap()
    {
        this.transform.DestroyAllChildrenImmediately();
    }
}