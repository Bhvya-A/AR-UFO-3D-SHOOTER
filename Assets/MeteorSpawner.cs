using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public Transform playerCamera;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float spawnDistance = 35f;

    [Header("Spawn Area")]
    public float horizontalRange = 7f;
    public float verticalRange = 5f;

    [Header("Difficulty")]
    public float minimumSpawnInterval = 0.8f;
    public float speedIncreasePerScore = 0.1f;
    public float maxSpeed = 17f;

    private bool spawning = false;

    public void StartSpawning()
    {
        if (spawning)
            return;

        spawning = true;

        InvokeRepeating(
            nameof(SpawnMeteor),
            1f,
            spawnInterval
        );
    }

    public void StopSpawning()
    {
        spawning = false;
        CancelInvoke(nameof(SpawnMeteor));
    }

    void SpawnMeteor()
    {
        if (!spawning || meteorPrefab == null || playerCamera == null)
            return;

        float x = Random.Range(-horizontalRange, horizontalRange);
        float y = Random.Range(-verticalRange, -0.5f);

        Vector3 localPosition =
            new Vector3(x, y, spawnDistance);

        Vector3 spawnPosition =
            playerCamera.TransformPoint(localPosition);

        GameObject meteor = Instantiate(
            meteorPrefab,
            spawnPosition,
            Quaternion.LookRotation(
                playerCamera.position - spawnPosition
            )
        );

        MeteorMovement movement =
            meteor.GetComponent<MeteorMovement>();

        if (movement != null)
        {
            int score = UFOGameManager.Instance != null
                ? UFOGameManager.Instance.GetScore()
                : 0;

            movement.speed = Mathf.Min(
                10f + score * speedIncreasePerScore,
                maxSpeed
            );
        }

        int currentScore =
            UFOGameManager.Instance != null
                ? UFOGameManager.Instance.GetScore()
                : 0;

        float newInterval = Mathf.Max(
            minimumSpawnInterval,
            spawnInterval - currentScore * 0.015f
        );

        CancelInvoke(nameof(SpawnMeteor));

        InvokeRepeating(
            nameof(SpawnMeteor),
            newInterval,
            newInterval
        );
    }
}