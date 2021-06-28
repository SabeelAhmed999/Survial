using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFunctionality : MonoBehaviour
{
    public GameObject bulletPrefab;
    [SerializeField]
    private Transform firePoint;
    [Range(0.1f,1)]
    public float fireRate;
    [Range(1,5)]
    public int damagePower;
    private float timer;
    bool startGun=false;
    private void Start() {
        StartCoroutine(ShootingDelay());
    }
    private void FixedUpdate() {
        if(GameManager.Instance.gameState==GameState.Running)
        {
            timer+=Time.fixedDeltaTime;
            if(timer>=fireRate)
            {
                timer=0f;
                if(startGun)
                Shoot();
            }
        }
    }
    void Shoot()
    {
        GameObject bullet =Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);
        bullet.GetComponent<Bullet>().firedBy=this;
        Rigidbody rb=bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.up*10 ,ForceMode.Impulse);
    }

    IEnumerator ShootingDelay()
    {
        yield return new WaitForSeconds(1.5f);
        startGun=true;
        yield return null;
    }
}
