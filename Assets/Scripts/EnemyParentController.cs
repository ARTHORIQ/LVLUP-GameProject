using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParentController : MonoBehaviour
{
    [Header("Enemy Movement")]
    public float movingSpeed = 1f;
    public float speedIncreaseAmount = 0.2f;
    public float maxMovingSpeed = 10f;
    public GameObject enemyContainer;
    public float horizontalLimit = 1.25f;
    public float verticalLimit = 1.4f;

    private List<EnemyController> enemies = new List<EnemyController>();
    private float movingDirection = 1;
    private Vector2 targetPosition;

    private void Start()
    {
        // Get all the enemy controllers and add them to the list
        EnemyController[] enemyControllers = GetComponentsInChildren<EnemyController>();
        foreach (EnemyController enemy in enemyControllers)
        {
            enemies.Add(enemy);
        }
    }

    private void Update()
    {
        // Move the enemy container
        enemyContainer.transform.Translate(movingDirection * movingSpeed * Time.deltaTime, 0f, 0f);

        // Check if any enemy is at the edge of the screen
        bool reachedEdge = false;

        foreach (EnemyController enemy in enemies)
        {
            float enemyX = enemy.transform.position.x;
            if (movingDirection > 0)
            {
                float rightmostPosition = float.MinValue; // set initial value to lowest possible float
                foreach (EnemyController enemyChild in GetComponentsInChildren<EnemyController>())
                {
                    if (!enemyChild.gameObject.activeSelf) continue; // skip enemies that are not active

                    if (enemyChild.transform.position.x > rightmostPosition)
                        rightmostPosition = enemyChild.transform.position.x;
                }
                if (rightmostPosition > horizontalLimit)
                {
                    movingDirection *= -1;
                    targetPosition = new Vector2(rightmostPosition, enemyContainer.transform.position.y - 0.2f);
                }
                else
                    targetPosition = new Vector2(horizontalLimit, targetPosition.y);
            }
            else
            {
                float leftmostPosition = float.MaxValue; // set initial value to highest possible float
                foreach (EnemyController enemyChild in GetComponentsInChildren<EnemyController>())
                {
                    if (!enemyChild.gameObject.activeSelf) continue; // skip enemies that are not active

                    if (enemyChild.transform.position.x < leftmostPosition)
                        leftmostPosition = enemyChild.transform.position.x;
                }
                if (leftmostPosition < -horizontalLimit)
                {
                    movingDirection *= -1;
                    targetPosition = new Vector2(leftmostPosition, enemyContainer.transform.position.y - 0.2f);
                }
                else
                    targetPosition = new Vector2(-horizontalLimit, targetPosition.y);
            }

        }

        // If an enemy has reached the edge, reverse direction and move down
        if (reachedEdge)
        {
            movingDirection *= -1;
            targetPosition = new Vector2(enemyContainer.transform.position.x, enemyContainer.transform.position.y - 0.2f);
        }
    }
}
