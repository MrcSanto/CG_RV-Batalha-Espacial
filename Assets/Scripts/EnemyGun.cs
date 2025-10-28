using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject EnemyBulletGO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("FireEnemyBullet", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // função para disparar uma bala
    void FireEnemyBullet()
    {
        GameObject playerShip = GameObject.Find ("PlayerGO");

        if (playerShip != null)
        {
            GameObject bullet = (GameObject)Instantiate (EnemyBulletGO);

            bullet.transform.position = transform.position;

            Vector2 dir = playerShip.transform.position - bullet.transform.position;

            bullet.GetComponent<EnemyBullet>().SetDirection(dir);
        }
    }
}
