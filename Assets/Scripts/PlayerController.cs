using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.5f;
    public float projectileSpeed = 3f;
    public float firingCooldown = 1f;
    public float firingSpeed = 3f;
    public GameObject LaserPrefab;
    public GameObject explosionPrefab;


    [SerializeField]
    public float horizontalLimit = 2.5f;
    private float cooldownTimer;
    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        //Player Movement
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, 0f);

        //Player BorderLimit
        if (transform.position.x > horizontalLimit)
        {
            transform.position = new Vector3(horizontalLimit, transform.position.y, transform.position.z);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        }
        if (transform.position.x <- horizontalLimit) {
            transform.position = new Vector3(-horizontalLimit, transform.position.y, transform.position.z);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        //Player Attacks
        if (Input.GetMouseButtonDown(0))
        {
            if (cooldownTimer < 0) 
            {
                cooldownTimer = firingCooldown;

                GameObject laserObject = Instantiate(LaserPrefab);
                laserObject.transform.SetParent(transform.parent);
                laserObject.transform.position = transform.position;
                laserObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, firingSpeed);
                Destroy(laserObject, 2f); // Destroy object setelah 2 detik

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemiesLaserPrefab" || other.tag == "Enemy")
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
