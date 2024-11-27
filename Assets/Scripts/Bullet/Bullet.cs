using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Bullet speed
    public float lifespan = 5f; // Time before bullet is destroyed

    private Vector2 bulletDirection;
    private string bulletFromPlayer;

    void Start()
    {
        // Destroy the bullet after 'lifespan' seconds
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        if(bulletDirection == null || !gameObject.activeSelf)
        {
            return;
        }

        // Move the bullet forward (along the x-axis or direction of travel)
        transform.Translate(bulletDirection * speed * Time.deltaTime);
    }

    public void setBulletFrom(string bulletFromPlayer)
    {
        this.bulletFromPlayer = bulletFromPlayer;
    }

    public void setBulletDirection(Vector2 bulletDirection)
    {
        this.bulletDirection = bulletDirection;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") || other.gameObject.name.Equals(bulletFromPlayer))
        {
            return;
        }

        HealthDataStore.getInstance(other.gameObject.name).reduceHealth();
        gameObject.SetActive(false);
    }
}
