using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    public GameObject ufoPrefab;
    public Transform playerCamera;

    [Header("Spawn Settings")]
    public float spawnInterval = 0.7f;
    public float spawnDistance = 30f;

    [Header("Spawn Area")]
    public float horizontalRange = 7f;
    public float verticalRange = 5f;

    [Header("Difficulty")]
    public float minimumSpawnInterval = 0.25f;
    public float speedIncreasePerScore = 0.08f;
    public float maxSpeed = 14f;

    private bool spawning = false;

    public void StartSpawning()
    {
        if (spawning)
            return;

        spawning = true;

        InvokeRepeating(
            nameof(SpawnUFO),
            0.3f,
            spawnInterval
        );
    }

    public void StopSpawning()
    {
        spawning = false;
        CancelInvoke(nameof(SpawnUFO));
    }

    void SpawnUFO()
    {
        if (!spawning || ufoPrefab == null || playerCamera == null)
            return;

        float x = Random.Range(-horizontalRange, horizontalRange);
        float y = Random.Range(0.5f, verticalRange);

        Vector3 localPosition =
            new Vector3(x, y, spawnDistance);

        Vector3 spawnPosition =
            playerCamera.TransformPoint(localPosition);

        GameObject ufo = Instantiate(
            ufoPrefab,
            spawnPosition,
            Quaternion.LookRotation(
                playerCamera.position - spawnPosition
            )
        );

        UFOMovement movement =
            ufo.GetComponent<UFOMovement>();

        if (movement != null)
        {
            int score = UFOGameManager.Instance != null
                ? UFOGameManager.Instance.GetScore()
                : 0;

            movement.speed = Mathf.Min(
                8f + score * speedIncreasePerScore,
                maxSpeed
            );
        }

        int currentScore =
            UFOGameManager.Instance != null
                ? UFOGameManager.Instance.GetScore()
                : 0;

        float newInterval = Mathf.Max(
            minimumSpawnInterval,
            spawnInterval - currentScore * 0.01f
        );

        CancelInvoke(nameof(SpawnUFO));

        InvokeRepeating(
            nameof(SpawnUFO),
            newInterval,
            newInterval
        );
    }
}