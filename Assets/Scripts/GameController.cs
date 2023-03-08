using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Shooting Mechanics")]
    public float shootingInterval = 3f;
    public float shootingSpeed = 2f;
    public GameObject enemiesLaserPrefab;
    private float shootingTimer;

    [Header("Enemy Movement")]
    public float movingSpeed = 1f;
    public GameObject enemyContainer;
    public float horizontalLimit = 3f;
    public float verticalLimit = 1.4f;
    private float movingDirection = 1;
    private Vector2 targetPosition;
    public PlayerController player;
    private float phase = 0;
    private int enemyCount;

    private bool playerKilled = false;

    private void Start()
    {
        shootingTimer = shootingInterval;
        enemyCount = GetComponentsInChildren<EnemyController>().Length;

    }

    private void Update()
    {
        int currentEnemyCount = GetComponentsInChildren<EnemyController>().Length;



        // Shooting Mechanics
        shootingTimer -= Time.deltaTime;
        if (currentEnemyCount > 0 && shootingTimer <= 0f)
        {
            shootingTimer = shootingInterval;
            EnemyController[] enemies = GetComponentsInChildren<EnemyController>();
            EnemyController randomEnemy = enemies[Random.Range(0, enemies.Length)];
            GameObject missileInstance = Instantiate(enemiesLaserPrefab);
            missileInstance.transform.SetParent(transform);
            missileInstance.transform.position = randomEnemy.transform.position;
            Rigidbody2D missileRb = missileInstance.GetComponent<Rigidbody2D>();
            missileRb.velocity = (Vector2)(player.transform.position - missileInstance.transform.position).normalized * shootingSpeed;
            missileRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(UnfreezeRotation(missileRb, 0.2f));
            Destroy(missileInstance, 5f);
        }

        IEnumerator UnfreezeRotation(Rigidbody2D rb, float delay)
        {
            yield return new WaitForSeconds(delay);
            rb.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
        }

        if (currentEnemyCount == 0)
        {
            float currentMovingSpeed = movingSpeed + 0.2f;
            float currentShootingInterval = shootingInterval - 0.1f;

            StartCoroutine(ReloadSceneAfterDelay(3f, currentMovingSpeed, currentShootingInterval));
        }

        IEnumerator ReloadSceneAfterDelay(float delay, float newMovingSpeed, float newShootingInterval)
        {
            yield return new WaitForSeconds(delay);

            GameController gameController = FindObjectOfType<GameController>();
            gameController.movingSpeed = newMovingSpeed;
            gameController.shootingInterval = newShootingInterval;

            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        // Check if player is killed
        if (!playerKilled && player == null)
        {
            playerKilled = true;
            Debug.Log("Player killed");
            StartCoroutine(ChangeSceneAfterDelay(2f));
        }

        //Movement
        enemyContainer.transform.position = Vector2.MoveTowards(
            enemyContainer.transform.position, targetPosition, Time.deltaTime * movingSpeed
        );

        float endMostPosition = 0f;
        foreach (EnemyController enemy in GetComponentsInChildren<EnemyController>())
        {
            if (movingDirection > 0)
                endMostPosition = enemy.transform.position.x > endMostPosition
                 ? enemy.transform.position.x : endMostPosition;
            else
                endMostPosition = enemy.transform.position.x < endMostPosition
                 ? enemy.transform.position.x : endMostPosition;
        }

        if (Mathf.Abs(endMostPosition) > horizontalLimit)
        {
            movingDirection *= -1;
            targetPosition = new Vector2(endMostPosition, enemyContainer.transform.position.y - 0.2f);
        }

        targetPosition = new Vector2((horizontalLimit * movingDirection) + endMostPosition, targetPosition.y);
    }

    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Menu");
    }
}