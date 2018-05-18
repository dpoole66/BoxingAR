using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHealth : MonoBehaviour {

    [SerializeField] private float healthAmount = 100f;

    public float HealthAmount{

        get{ return healthAmount; }
        set{

                    healthAmount = value;
                    if(HealthAmount <= 0.0f){

                        Debug.Log("Dead");

                    }

        }

    }

    private void Start() {

        StartCoroutine(ReviveHealth());

    }

    IEnumerator ReviveHealth(){

        while(true){

            if (healthAmount < 100.0f){

                healthAmount += 10.0f;
                Debug.Log("Revive");
                yield return new WaitForSeconds(1);

            }   else {

                yield return null;

            }

        }

    }

}
