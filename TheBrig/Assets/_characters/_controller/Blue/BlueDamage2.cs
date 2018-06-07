using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueDamage2 : MonoBehaviour {

    public string Opponent;
    public float DamageAmt = 10.0f;
    public BlueHitController m_BlueController;

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == Opponent) {

            m_BlueController.HitCount = m_BlueController.HitCount + 1;
            m_BlueController.Health = m_BlueController.Health -= DamageAmt;
            m_BlueController.hit = true;
            m_BlueController.m_ScoreUI.text = m_BlueController.HitCount.ToString();
            m_BlueController.m_HealthUI.value = m_BlueController.Health;

        }
    }

    private void OnTriggerExit(Collider other) {

        m_BlueController.hit = false;

    }
}
