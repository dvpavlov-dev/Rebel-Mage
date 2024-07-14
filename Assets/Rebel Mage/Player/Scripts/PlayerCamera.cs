using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform target;
    private Vector3 cameraPosition = new Vector3(0, 20, -12);

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + cameraPosition;
        }
    }

    private void OnEnable()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
}
