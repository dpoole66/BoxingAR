using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDamage2 : MonoBehaviour {

    public string Opponent;
    private int hitCount;
    public SimpleRedControl m_RedController;             
    [SerializeField] private float healthAmount = 100f;

    



    public float HealthAmount {

        get { return healthAmount; }
        set {

            healthAmount = value;
            if (HealthAmount <= 0.0f) {

                m_RedController.dead = true;

            }     
        }   
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == Opponent) {

            hitCount = hitCount + 1;
            m_RedController.HitCount = hitCount + 1;
            m_RedController.Health = HealthAmount -= 10.0f;
            //m_RedController.Health = HealthAmount;
            m_RedController.hit = true;
            m_RedController.m_ScoreUI.text = m_RedController.HitCount.ToString();
            m_RedController.m_HealthUI.value = HealthAmount;

        }   
    }

    public int HitCount {

        get { return hitCount; }
        set { hitCount = HitCount; }

    }

   
}
