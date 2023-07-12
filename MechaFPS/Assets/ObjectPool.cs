using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public GameObject bulletPrefab;
    public int bulletPoolSize = 10;
    private List<GameObject> bulletPool;

    public GameObject enemyPrefab;
    public int enemyPoolSize = 5;
    private List<GameObject> enemyPool;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Bullet Object Pool'u oluþtur
        bulletPool = new List<GameObject>();
        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }

        // Enemy Object Pool'u oluþtur
        enemyPool = new List<GameObject>();
        for (int i = 0; i < enemyPoolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    public GameObject GetBullet()
    {
        // Boþta olan bir mermi nesnesini döndür
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
            {
                return bulletPool[i];
            }
        }

        // Eðer tüm mermi nesneleri kullanýlýyorsa, yeni bir mermi oluþtur ve Object Pool'a ekle
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        bulletPool.Add(newBullet);
        return newBullet;
    }

    public GameObject GetEnemy()
    {
        // Boþta olan bir düþman nesnesini döndür
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (!enemyPool[i].activeInHierarchy)
            {
                return enemyPool[i];
            }
        }

        // Eðer tüm düþman nesneleri kullanýlýyorsa, yeni bir düþman oluþtur ve Object Pool'a ekle
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.SetActive(false);
        enemyPool.Add(newEnemy);
        return newEnemy;
    }

}
