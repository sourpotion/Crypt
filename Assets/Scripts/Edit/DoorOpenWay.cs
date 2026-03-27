using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class DoorOpenWay : MonoBehaviour
{
    [SerializeField] private bool isActive = false;
    private float xOffset = .2f;
    private float lengt = 1f;
    private float height = 2f;

    void OnDrawGizmos()
    {
        if (!isActive) {return;}

        Gizmos.color = Color.red;

        Vector3 begin = transform.position + -transform.right * xOffset;
        Vector3 x2 = begin + transform.up * height;

        Gizmos.DrawLine(begin, begin + -transform.forward * lengt + transform.up * height);
        Gizmos.DrawLine(x2, x2 + -transform.forward * lengt + -transform.up * height);
    }

    public void ToggleDebugState()
    {
        isActive = !isActive;
    }
}
