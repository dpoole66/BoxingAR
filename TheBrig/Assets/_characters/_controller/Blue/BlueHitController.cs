using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

public class BlueHitController : MonoBehaviour {

    //Health
    public float Health;
    public float MaxHealth = 100.0f;
    public float RegenAmt = 10.0f;
    public bool hit;
    public bool dead;
    public int HitCount;
    public Slider m_HealthUI;
    public Text m_ScoreUI;

    // Monobehaviour
    private void Start() {

        Health = MaxHealth;

    }

    void Update() {

        if (Health <= MaxHealth) {

            Health += RegenAmt * Time.deltaTime;
            m_HealthUI.value = Health;
            //Debug.Log(Health);

        } else if (Health > MaxHealth) {

            Health = MaxHealth;

        }
    }
}
