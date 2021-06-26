using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum DeathThrough{None,Shock,Shoot,Explosion}
public class EnemyController : MonoBehaviour
{
    public DeathThrough deathThrough;
    public GameObject[] rewardPrefabs;
    public Vector3 rewardOffset;
    public GameObject dieEffect;
    public GameObject explodeEffect;
    public GameObject shockEffect;
    private bool deathByExplosion,deathByShoot,deathByShock,Died;
    [SerializeField]
    private Slider healthSlider;
    private int maxHealth;
    private Animator animator;
    [SerializeField]
    [Range(2,10)]
    private int health;
    [SerializeField]
    private AudioSource audioSource;
    private Rigidbody rigidbodyPlayer;
    [SerializeField]
    [Range(50,100)]
    private float moveSpeed;
    private Collider mainCollider;
    private Collider[] allPresentColliders;
    private Rigidbody[] activeRB;
    void Start()
    {
        healthSlider.value=health;
        animator=GetComponent<Animator>();
        rigidbodyPlayer=GetComponent<Rigidbody>();
        allPresentColliders=this.gameObject.GetComponentsInChildren<Collider>();
        mainCollider=this.gameObject.GetComponent<Collider>();
        activeRB=this.gameObject.GetComponentsInChildren<Rigidbody>();
        for(int i=1;i<allPresentColliders.Length;i++)
        {
            allPresentColliders[i].isTrigger=true;
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(GameManager.Instance.gameState==GameState.Running)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                if(PlayerContoller.Instance.shield==false&&Died==false)
                {
                    if(!deathByShoot&&!deathByShock&&!deathByExplosion)
                    {
                        deathByExplosion=true;
                        DeathByExplosion();
                        PlayerContoller.Instance.FallOff();
                    }

                }
                if(PlayerContoller.Instance.shield==true&&deathThrough==DeathThrough.None)
                {
                    if(!deathByShoot&&!deathByExplosion&&!deathByShock)
                    {
                        deathByShock=true;
                        ShockDeath();
                    }
                }
            }
        }
    }
    private void OnTriggerStay(Collider other) {
        if(GameManager.Instance.gameState==GameState.Running)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                animator.SetBool("Walk",true);
                rigidbodyPlayer.AddRelativeForce(Vector3.forward*moveSpeed*Time.deltaTime,ForceMode.VelocityChange);
                transform.LookAt(other.transform);
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(GameManager.Instance.gameState==GameState.Running)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject,4f);
            }
        }
    }
    public void HealthController(int damage)
    {
        if(health>0)
        {
            healthSlider.gameObject.SetActive(true);
            health=health-damage;
            healthSlider.maxValue=health;
        }
        if(health<=0)
        {
            deathByShoot=true;
            DeathByBullet();
            return;
        }
    }
    void DeathByBullet()
    {
        if(!Died)
        {
            if(deathByShoot==true&&deathByExplosion==false)
            {
                Died=true;
                int rewardNo=Random.Range(0,rewardPrefabs.Length);
                GameObject reward =Instantiate(rewardPrefabs[rewardNo],transform.position+rewardOffset,transform.rotation);
                healthSlider.gameObject.SetActive(false);
                GameObject effect =Instantiate(dieEffect,transform.position,Quaternion.identity);
                Destroy(effect,5f);
                Destroy(this.gameObject.GetComponent<GunFunctionality>());
                rigidbodyPlayer.useGravity=false;
                mainCollider.enabled=false;
                animator.enabled=false;
                animator.avatar=null;
                for(int i=1;i<allPresentColliders.Length;i++)
                {
                    allPresentColliders[i].isTrigger=false;
                }
                for(int j=1;j<activeRB.Length;j++)
                {
                    activeRB[j].velocity=Vector3.zero;
                }
                Destroy(gameObject,2f);
            }
        }

    }

    void DeathByExplosion()
    {
        if(deathByExplosion==true&&deathByShoot==false&&deathByShock==false)
        {
            Died=true;
            healthSlider.gameObject.SetActive(false);
            GameObject effect =Instantiate(explodeEffect,transform.position,Quaternion.identity);
            Destroy(effect,5f);
            rigidbodyPlayer.useGravity=false;
            mainCollider.enabled=false;
            animator.enabled=false;
            animator.avatar=null;
            for(int i=1;i<allPresentColliders.Length;i++)
            {
                allPresentColliders[i].isTrigger=false;
            }
            for(int j=1;j<activeRB.Length;j++)
            {
                activeRB[j].velocity=Vector3.zero;
            }
            Destroy(gameObject,4f);
        }
        return;
    }

    void ShockDeath()
    {
        if(deathByShock==true&&deathByShoot==false&&deathByExplosion==false)
        {
            Died=true;
            healthSlider.gameObject.SetActive(false);
            GameObject effect =Instantiate(shockEffect,transform.position,Quaternion.identity);
            Destroy(effect,3f);
            rigidbodyPlayer.useGravity=false;
            mainCollider.enabled=false;
            animator.enabled=false;
            animator.avatar=null;
            for(int i=1;i<allPresentColliders.Length;i++)
            {
                allPresentColliders[i].isTrigger=false;
            }
            for(int j=1;j<activeRB.Length;j++)
            {
                activeRB[j].velocity=Vector3.zero;
            }
            Destroy(gameObject,1);
        }
    }
}
