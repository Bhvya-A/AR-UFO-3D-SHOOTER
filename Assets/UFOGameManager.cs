using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class UFOGameManager : MonoBehaviour
{
    public static UFOGameManager Instance;

    [Header("Camera")]
    public Camera playerCamera;

    [Header("UI")]
    public TMP_Text scoreText;
    public GameObject playButton;
    public GameObject gameOverPanel;
    public GameObject crosshair;

    [Header("Spawners")]
    public UFOSpawner ufoSpawner;
    public MeteorSpawner meteorSpawner;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public float bulletSpawnDistance = 0.5f;

    private int score = 0;
    private bool gameStarted = false;
    private bool gameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        score = 0;
        UpdateScore();

        gameStarted = false;
        gameOver = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (playButton != null)
            playButton.SetActive(true);

        if (crosshair != null)
            crosshair.SetActive(false);

        if (ufoSpawner != null)
            ufoSpawner.StopSpawning();

        if (meteorSpawner != null)
            meteorSpawner.StopSpawning();
    }

    void Update()
    {
        if (!gameStarted || gameOver)
            return;

        // New Input System
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            FireBullet();
        }

#if UNITY_EDITOR
        // Mouse testing in Unity Editor
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            FireBullet();
        }
#endif
    }

    public void StartGame()
    {
        score = 0;
        UpdateScore();

        gameStarted = true;
        gameOver = false;

        if (playButton != null)
            playButton.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (crosshair != null)
            crosshair.SetActive(true);

        if (ufoSpawner != null)
            ufoSpawner.StartSpawning();

        if (meteorSpawner != null)
            meteorSpawner.StartSpawning();
    }

    void FireBullet()
    {
        if (playerCamera == null || bulletPrefab == null)
            return;

        // ALWAYS shoot through the center/crosshair.
        // The location of the tap does not matter.

        Vector3 direction =
            playerCamera.transform.forward;

        Vector3 spawnPosition =
            playerCamera.transform.position +
            direction * bulletSpawnDistance;

        GameObject bullet = Instantiate(
            bulletPrefab,
            spawnPosition,
            Quaternion.LookRotation(direction)
        );

        BulletMovement bulletMovement =
            bullet.GetComponent<BulletMovement>();

        if (bulletMovement != null)
        {
            bulletMovement.SetDirection(direction);
        }
    }

    public void AddScore()
    {
        score++;
        UpdateScore();
    }

    public int GetScore()
    {
        return score;
    }

    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score;
        }
    }

    public void GameOver()
    {
        if (gameOver)
            return;

        gameOver = true;
        gameStarted = false;

        if (ufoSpawner != null)
            ufoSpawner.StopSpawning();

        if (meteorSpawner != null)
            meteorSpawner.StopSpawning();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (crosshair != null)
            crosshair.SetActive(false);

        Debug.Log("GAME OVER");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}