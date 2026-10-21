using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float destroyAfter = 1f;

    void Start()
    {
        Destroy(gameObject, destroyAfter);
    }
}