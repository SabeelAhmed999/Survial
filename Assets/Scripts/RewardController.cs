using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RewardType{IncreaseDamage,IncreaseFireRate,HealthIncrease,Sheild,Gold}
public class RewardController : MonoBehaviour
{
    public RewardType rewardType;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            switch (rewardType)
            {
                case RewardType.Sheild:
                    PlayerContoller.Instance.shield=true;
                    PlayerContoller.Instance.StartCoroutine(PlayerContoller.Instance.ShieldController());
                    break;
                case RewardType.IncreaseDamage:
                    if(other.gameObject.GetComponent<GunFunctionality>().damagePower<5)
                    {
                        other.gameObject.GetComponent<GunFunctionality>().damagePower=other.gameObject.GetComponent<GunFunctionality>().damagePower+1;
                    }
                    break;
                case RewardType.IncreaseFireRate:
                    if(other.gameObject.GetComponent<GunFunctionality>().fireRate>0.1f)
                    {
                        other.gameObject.GetComponent<GunFunctionality>().fireRate=other.gameObject.GetComponent<GunFunctionality>().fireRate-0.1f;
                    }
                    break;
                case RewardType.HealthIncrease:
                    if(PlayerContoller.Instance.health<15)
                    {
                        PlayerContoller.Instance.health=PlayerContoller.Instance.health+1;
                    }
                    break;
                default:
                    return;
            }
            Destroy(gameObject);
        }
    }

}
