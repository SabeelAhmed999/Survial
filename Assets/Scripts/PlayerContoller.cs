using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller : MonoBehaviour
{
    public bool shield;
    public GameObject dieEffect;
    public GameObject shieldEffect;
    private bool isGrounded;
    [SerializeField]
    private int gravity;
    public static PlayerContoller Instance;
    [Range(10,15)]
    public int health;
    [SerializeField]
    private Slider healthSlider;
    private Animator animator;
    [SerializeField]
    private AudioSource audioSource;
    private Rigidbody rigidbodyPlayer;
    [SerializeField]
    [Range(50,100)]
    private float moveSpeed;
    private float moveDirection;
    private Collider mainCollider;
    private Collider[] allPresentColliders;
    private Rigidbody[] activeRB;


    private void Awake() {
        if(Instance==null)
        {
            Instance=this;
        }
        else
            Destroy(this);
    }


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

    void Update()
    {
        if(GameManager.Instance.gameState==GameState.Running)
        {
            rigidbodyPlayer.AddForce(Vector3.forward*moveSpeed*Time.deltaTime,ForceMode.VelocityChange);
             if(SwipeManager.swipeRight)
            {
                moveDirection=moveDirection+30;
                rigidbodyPlayer.AddForce(Vector3.right*moveDirection*Time.deltaTime,ForceMode.Impulse);
                animator.SetBool("WalkRight",true);
            }
            if(SwipeManager.swipeLeft)
            {
                moveDirection=moveDirection+30;
                rigidbodyPlayer.AddForce(Vector3.left*moveDirection*Time.deltaTime,ForceMode.Impulse);
                animator.SetBool("WalkLeft",true);
            }
            if(SwipeManager.swipeLeft==false&&SwipeManager.swipeRight==false)
            {
                animator.SetBool("WalkLeft",false);
                animator.SetBool("WalkRight",false);
                moveDirection=0;
            }
            if(!isGrounded)
            {
                rigidbodyPlayer.AddForce(Vector3.down*gravity*Time.deltaTime,ForceMode.VelocityChange);
            }
        }   
    }

    public IEnumerator ShieldController()
    {
        GameObject effect =Instantiate(shieldEffect,transform.position,Quaternion.identity);
        effect.transform.parent=this.transform;
        Destroy(effect,5f);
        yield return new WaitForSeconds(5);
        shield=false;
        yield return null;
    }
    public void Died()
    {
        GameManager.Instance.gameState=GameState.GameOver;
        GameObject effect =Instantiate(dieEffect,transform.position,Quaternion.identity);
        Destroy(effect,5f);
        GameManager.Instance.ModifyGameState();
        rigidbodyPlayer.useGravity=false;
        mainCollider.enabled=false;
        animator.enabled=false;
        animator.avatar=null;
        for(int i=1;i<allPresentColliders.Length;i++)
        {
            allPresentColliders[i].isTrigger=false;
            activeRB[i].velocity=Vector3.zero;
        }
        StartCoroutine(Slowmo());
    }

    public void HealthController(int damage)
    {
        if(health>0)
        {
            health=health-damage;
            healthSlider.maxValue=health;
        }
        if(health<=0)
        {
            if(GameManager.Instance.gameState==GameState.Running)
                Died();
            else
                return;
        }
    }
    IEnumerator Slowmo()
    {
        Time.timeScale=0.2f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale=1;
        yield return null;
    }
    
    private void OnCollisionExit(Collision other) {
        if(other.gameObject.CompareTag("Ground"))
        {
            isGrounded=false;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            isGrounded=true;
        }
    }
    public void FallOff()
    {
        GameManager.Instance.gameState=GameState.GameOver;
        GameManager.Instance.ModifyGameState();
        rigidbodyPlayer.useGravity=false;
        mainCollider.enabled=false;
        animator.enabled=false;
        animator.avatar=null;
        for(int i=1;i<allPresentColliders.Length;i++)
        {
            allPresentColliders[i].isTrigger=false;
            activeRB[i].velocity=Vector3.zero;
        }
        StartCoroutine(Slowmo());
    }
}
