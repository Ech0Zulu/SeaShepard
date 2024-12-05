using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : MonoBehaviour
{
    private bool canShoot = true;
    private float shootCD = 1.5f;
    private float curShootCD = 0f;
    private Vector2 mousePos;
    private Transform projectilSpawn;

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
        Aim();
        Shoot();
    }

    private void Aim()
    {
         mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot)
            {
                canShoot = false;
                // Calculer la direction du tir
                Vector2 direction = ( mousePos - (Vector2)projectilSpawn.position).normalized;

                // Calculer l'angle de rotation
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Créer le projectile avec la bonne rotation
                GameObject projectile = Instantiate(projectilePrefab, (Vector2)projectilSpawn.position, Quaternion.Euler(0, 0, angle));

                // Ajouter une vitesse au projectile (si Rigidbody2D est attaché)
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                Debug.Log("Fire !");
            }
            else Debug.Log("Can't shoot now !");
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
