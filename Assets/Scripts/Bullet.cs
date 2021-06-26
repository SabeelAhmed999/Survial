using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public GunFunctionality firedBy;
    private void OnCollisionEnter(Collision other) {

        if(other.gameObject.CompareTag("Enemy"))
        {
            if(firedBy.gameObject.tag!="Enemy")
            {
                this.gameObject.GetComponent<Collider>().enabled=false;
                other.gameObject.GetComponent<EnemyController>().HealthController(firedBy.damagePower);
                GameObject effect =Instantiate(hitEffect,transform.position,Quaternion.identity);
                Destroy(effect,5f);
                Destroy(gameObject);
                return;
            }

        }
        if(other.gameObject.CompareTag("Player"))
        {
            if(PlayerContoller.Instance.shield==false)
            {
                if(firedBy.gameObject.tag!="Player")
                {
                    other.gameObject.GetComponent<PlayerContoller>().HealthController(firedBy.damagePower);
                    this.gameObject.GetComponent<Collider>().enabled=false;
                    GameObject effect =Instantiate(hitEffect,transform.position,Quaternion.identity);
                    Destroy(effect,5f);
                    Destroy(gameObject);
                    return;
                }
            }
            return;
        } 
        else
        {
            GameObject effect =Instantiate(hitEffect,transform.position,Quaternion.identity);
            Destroy(effect,5f);
            Destroy(gameObject);
        }
    }
}
