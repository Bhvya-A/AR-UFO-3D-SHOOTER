using UnityEngine;

public class UFOMovement : MonoBehaviour
{
    public float speed = 8f;
    public float destroyDistance = 1f;

    [Header("Visual")]
    public float rotationSpeed = 100f;

    [Header("Size")]
    public float minScale = 0.9f;
    public float maxScale = 1.1f;

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

        Vector3 direction =
            playerCamera.position - transform.position;

        float distance = direction.magnitude;

        // UFO reaches player
        if (distance <= destroyDistance)
        {
            if (UFOGameManager.Instance != null)
            {
                UFOGameManager.Instance.GameOver();
            }

            Destroy(gameObject);
            return;
        }

        direction.Normalize();

        transform.position +=
            direction * speed * Time.deltaTime;

        transform.LookAt(playerCamera);

        transform.Rotate(
            0f,
            rotationSpeed * Time.deltaTime,
            0f,
            Space.Self
        );
    }
}