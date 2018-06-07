using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDamage2 : MonoBehaviour {

    public string Opponent;
    public float DamageAmt = 10.0f;    
    public RedHitController m_RedController;             


    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == Opponent) {

            m_RedController.HitCount = m_RedController.HitCount + 1;            
            m_RedController.Health = m_RedController.Health -= DamageAmt;        
            m_RedController.hit = true;
            m_RedController.m_ScoreUI.text = m_RedController.HitCount.ToString();
            m_RedController.m_HealthUI.value = m_RedController.Health;

        }   
    }

    private void OnTriggerExit(Collider other) {

        m_RedController.hit = false;

    }
}
