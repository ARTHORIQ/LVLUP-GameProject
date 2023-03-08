using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject explosionPrefab;

    public GameObject[] powerUpPrefabs;
    public float dropChance = 0.5f;
    public float dropSpeed = 0.5f;
    public float destroyYPos = -10f;

    void Update()
    {
        if (transform.position.y < destroyYPos)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "LaserPrefab")
        {
            GameObject explosionObject = Instantiate(explosionPrefab);
            explosionObject.transform.SetParent(transform.parent.parent);
            explosionObject.transform.position = transform.position;
            Destroy(explosionObject, 1.5f);

            Destroy(gameObject);
            Destroy(other.gameObject);


        }
    }
}
