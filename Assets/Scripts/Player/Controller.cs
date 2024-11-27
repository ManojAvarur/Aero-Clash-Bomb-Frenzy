using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.1f;

    [Header("Bullet Controller")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireRate = 0.5f; // How often the player can shoot
    private float nextFireTime = 0f;

    private Rigidbody2D player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = HealthDataStore.getInstance(gameObject.name).getHealthColor();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + 1f / fireRate;
        }

        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        player.velocity = movement * moveSpeed;

        spriteRenderer.color = HealthDataStore.getInstance(gameObject.name).getHealthColor();
    }

    void FireBullet()
    {
        Vector3 spawnPosition = gameObject.transform.position + new Vector3(0, .7f, 0);

        GameObject newBullet = Instantiate(bullet, spawnPosition, gameObject.transform.rotation);
        newBullet.name = gameObject.name + "'s Bullet";

        Bullet bulletClass = newBullet.GetComponent<Bullet>();

        bulletClass.setBulletFrom(gameObject.name);
        bulletClass.setBulletDirection(Vector2.up);
        newBullet.SetActive(true);
    }
}
