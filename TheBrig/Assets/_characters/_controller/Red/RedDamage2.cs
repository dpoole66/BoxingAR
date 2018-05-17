using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDamage2 : MonoBehaviour {

    public string Opponent;
    private int hitCount;         
    public SimpleRedControl m_RedController;
    private RedHealth m_Health;
    public Slider m_HealthUI;
    public Text m_ScoreUI;

    private void Awake() {

        m_Health = GetComponent<RedHealth>();

    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == Opponent) {

            hitCount =  hitCount + 1;
            m_RedController.HitCount = hitCount;
            m_Health.HealthAmount -= 10.0f;
            m_RedController.Health = m_Health.HealthAmount;
            m_RedController.hit = true;
            m_ScoreUI.text = hitCount.ToString();
            m_HealthUI.value = m_Health.HealthAmount;

        }

    }

    public int HitCount {

        get { return hitCount; }
        set { hitCount = HitCount; }


    }


}
