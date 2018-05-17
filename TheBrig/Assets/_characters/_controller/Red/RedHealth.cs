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

}
