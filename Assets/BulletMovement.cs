using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [Header("Bullet")]
    public float speed = 40f;
    public float lifetime = 2f;

    [Header("Explosion")]
    public GameObject explosionEffect;

    private Vector3 direction;

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;

        transform.rotation =
            Quaternion.LookRotation(direction);
    }

    void Start()
    {
        if (direction == Vector3.zero)
            direction = transform.forward;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        float distanceThisFrame =
            speed * Time.deltaTime;

        if (Physics.Raycast(
            transform.position,
            direction,
            out RaycastHit hit,
            distanceThisFrame))
        {
            // UFO
            UFOMovement ufo =
                hit.collider.GetComponentInParent<UFOMovement>();

            if (ufo != null)
            {
                if (explosionEffect != null)
                {
                    Instantiate(
                        explosionEffect,
                        ufo.transform.position,
                        Quaternion.identity
                    );
                }

                if (GameAudioManager.Instance != null)
                {
                    GameAudioManager.Instance.PlayUFODestroy();
                }

                Destroy(ufo.gameObject);

                if (UFOGameManager.Instance != null)
                {
                    UFOGameManager.Instance.AddScore();
                }

                Destroy(gameObject);
                return;
            }

            // Meteor
            MeteorMovement meteor =
                hit.collider.GetComponentInParent<MeteorMovement>();

            if (meteor != null)
            {
                if (explosionEffect != null)
                {
                    Instantiate(
                        explosionEffect,
                        meteor.transform.position,
                        Quaternion.identity
                    );
                }

                if (GameAudioManager.Instance != null)
                {
                    GameAudioManager.Instance.PlayMeteorDestroy();
                }

                Destroy(meteor.gameObject);
                Destroy(gameObject);

                return;
            }
        }

        transform.position +=
            direction * distanceThisFrame;
    }
}