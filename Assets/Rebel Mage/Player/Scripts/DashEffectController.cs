using UnityEngine;

public class DashEffectController : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(DestroyEffect), 1f);
    }

    private void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
