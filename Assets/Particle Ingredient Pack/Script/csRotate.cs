using UnityEngine;
public class csRotate : MonoBehaviour
{
    public float XRotateSpeed;
    public float YRotateSpeed;
    public float ZRotateSpeed;

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(XRotateSpeed, YRotateSpeed, ZRotateSpeed);
    }
}
