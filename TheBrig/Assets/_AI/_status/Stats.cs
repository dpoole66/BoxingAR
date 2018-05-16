using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/Stats")]


public class Stats : ScriptableObject {

    public float moveSpeed = 1;
    public float rotateSpeed = 3.0f;
    public float moveRecovRate = 1.0f;
    public int score = 0;

    public float lookRange = 4.0f;
    public float lookSphereCastRadius = 4.0f;

    //public float alertRange = 4.0f;
    //public float advanceRange = 2.5f;                            
    public float chaseRange = 2.0f;
    //public float stopRange = 1.2f;
    //public float attackRange = 1.0f;   
    
    //public float attackRate = 1f;
    //public float attackForce = 15f;
    public int attackDamage = 50;                
    public float health = 100f;

    public bool enemyAttacking, enemyDefending;

}
