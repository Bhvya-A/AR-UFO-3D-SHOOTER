using UnityEngine;

public class MeteorMovement : MonoBehaviour
{
    public float speed = 4f;
    public float destroyDistance = 0.5f;

    [Header("Size")]
    public float minScale = 0.9f;
    public float maxScale = 1.1f;

    [Header("Rotation")]
    public float rotationX = 120f;
    public float rotationY = 80f;
    public float rotationZ = 60f;

    private Transform playerCamera;

    void Start()
    {
        if (Camera.main != null)
            playerCamera = Camera.main.transform;

        float randomScale = Random.Range(minScale, maxScale);
        transform.localScale *= randomScale;
    }

    void Update()
    {
        if (playerCamera == null)
            return;

        Vector3 direction = playerCamera.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= destroyDistance)
        {
            Destroy(gameObject);
            return;
        }

        direction.Normalize();

        transform.position += direction * speed * Time.deltaTime;

        transform.Rotate(
            rotationX * Time.deltaTime,
            rotationY * Time.deltaTime,
            rotationZ * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UFOGameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }
}