using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : MonoBehaviour
{
    private bool canShoot = true;
    private float shootCD = 1.5f;
    private float curShootCD = 0f;
    private Transform projectilSpawn;
    private float projectileSpeed = 15f;

    [SerializeField]
    private GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCD();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            canShoot = false;
            projectilSpawn = transform;

            // Définit la rotationactuelle du joueur
            Quaternion rotation = transform.rotation;

            // Calculer les positions locales des projectiles (gauche et droite)
            Vector2 offsetLeft = (Vector2)(rotation * Vector2.up) * 1f;
            Vector2 offsetRight = (Vector2)(rotation * Vector2.down) * 1f;

            // Instancier et tirer les projectiles
            SpawnProjectile((Vector2)transform.position + offsetLeft, rotation * Vector2.up);
            SpawnProjectile((Vector2)transform.position + offsetRight, rotation * Vector2.down);
        }
    }

    private void SpawnProjectile(Vector2 spawnPosition, Vector2 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = direction.normalized * projectileSpeed;
        }
    }

    private void UpdateCD()
    {
        if (!canShoot)
        {
            curShootCD += Time.deltaTime;
            if (curShootCD >= shootCD)
            {
                canShoot = true;
                curShootCD = 0;
            }
        }
    }

}
