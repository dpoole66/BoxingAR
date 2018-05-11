using UnityEngine;
using System.Collections;
//------------------------------------------
public class AI_Enemy : MonoBehaviour
{
    //------------------------------------------
    public enum ENEMY_STATE { IDLE, PATROL, CHASE, ATTACK };
    //------------------------------------------
    public ENEMY_STATE CurrentState {
        get { return currentstate; }

        set {
            //Update current state
            currentstate = value;

            //Stop all running coroutines
            StopAllCoroutines();

            switch (currentstate) {

                case ENEMY_STATE.IDLE:
                    StartCoroutine(AIIdle());
                    break;

                case ENEMY_STATE.PATROL:
                StartCoroutine(AIPatrol());
                break;

                case ENEMY_STATE.CHASE:
                StartCoroutine(AIChase());
                break;

                case ENEMY_STATE.ATTACK:
                StartCoroutine(AIAttack());
                break;
            }
        }
    }
    //------------------------------------------
    [SerializeField]
    private ENEMY_STATE currentstate = ENEMY_STATE.PATROL;            //Set IDLE as default

    private LineSight ThisLineSight = null;    
    private UnityEngine.AI.NavMeshAgent ThisAgent = null; 
    private Animator ThisAnim = null;    
    private Transform ThisTransform = null;       
   // [HideInInspector] public PlayerHealth PlayerHealth = null;          
    private Transform PlayerTransform = null;       
    //public Transform PatrolDestination = null;
    GameObject[] waypoints;
    int currentWP;

    //Range between players
    public float range;

    //Damage amount per second
    public float MaxDamage = 10f;


    //------------------------------------------
    void Awake() {
        ThisLineSight = GetComponent<LineSight>();
        ThisAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        ThisAnim = GetComponent<Animator>();
        ThisTransform = GetComponent<Transform>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }
    //------------------------------------------
    void Start() {
        //Configure starting state
        //CurrentState = ENEMY_STATE.IDLE;
    }

    private void Update() {

        //Get range to player
        range = Vector3.Distance(PlayerTransform.position, this.transform.position);
        Debug.Log(range);
        //ThisAgent.SetDestination(PatrolDestination.position);

    }

    //------------------------------------------
    public IEnumerator AIIdle() {
        //Loop while patrolling
        while (currentstate == ENEMY_STATE.IDLE) {
            //Set loose search
            ThisLineSight.Sensitity = LineSight.SightSensitivity.LOOSE;

            //anim params
            ThisAnim.SetBool("Idle", true);
            ThisAnim.SetBool("Move", false);
            ThisAnim.SetBool("Attack", false);
            ThisAnim.SetBool("Defend", false);                              

            //Look at player position
            var direction = PlayerTransform.position - this.transform.position;
            ThisAgent.isStopped = true;
            ThisAgent.transform.rotation.SetLookRotation(direction);

            //If we can see the target then start chasing
            if (ThisLineSight.CanSeeTarget && range > 0.1f) {
                Debug.Log("Chase now");
                ThisAgent.isStopped = false;
                CurrentState = ENEMY_STATE.CHASE;
                yield break;
            }

            //Wait until next frame
            yield return null;
        }
    }
    //------------------------------------------
    //------------------------------------------
    public IEnumerator AIPatrol() {
        //Loop while patrolling
        while (currentstate == ENEMY_STATE.PATROL) {
            //Set strict search
            ThisLineSight.Sensitity = LineSight.SightSensitivity.STRICT;
            ThisAnim.SetBool("Idle", false);
            ThisAnim.SetBool("Move", true);
            ThisAnim.SetBool("Attack", false);
            ThisAnim.SetBool("Defend", false);

            //waypoints list
            if (waypoints.Length == 0) {
                Debug.Log("No waypoints");
                yield break;
            }
            if (Vector3.Distance(waypoints[currentWP].transform.position, this.transform.position) <= ThisAgent.stoppingDistance) {

                currentWP++;
                if (currentWP >= waypoints.Length) {
                    currentWP = 0;
                }
            }
            //Chase to patrol position
            var direction = waypoints[currentWP].transform.position - this.transform.position;
            ThisAgent.isStopped = false;
            ThisAgent.SetDestination(direction);

            //Wait until path is computed
            while (ThisAgent.pathPending)
                yield return null;

            //If we can see the target then start chasing
            if (ThisLineSight.CanSeeTarget) {
                ThisAgent.isStopped = true;
                CurrentState = ENEMY_STATE.CHASE;
                yield break;
            }

            //Wait until next frame
            yield return null;
        }
    }
    //------------------------------------------
    public IEnumerator AIChase() {
        //Loop while chasing
        while (currentstate == ENEMY_STATE.CHASE) {
            //Set loose search
            ThisLineSight.Sensitity = LineSight.SightSensitivity.LOOSE;
            ThisAnim.SetBool("Idle", false);
            ThisAnim.SetBool("Move", true);
            ThisAnim.SetBool("Attack", false);
            ThisAnim.SetBool("Defend", false);

            //Chase to last known position
            ThisAgent.isStopped = false;
            ThisAgent.SetDestination(ThisLineSight.LastKnowSighting);

            //Wait until path is computed
            while (ThisAgent.pathPending)
                yield return null;

            //Have we reached destination?
            if (ThisAgent.remainingDistance <= ThisAgent.stoppingDistance) {

                //Stop agent
                ThisAgent.isStopped = true;

                //Reached destination but cannot see player
                if (!ThisLineSight.CanSeeTarget)
                    CurrentState = ENEMY_STATE.CHASE;
                else //Reached destination and can see player. Reached attacking distance
                    CurrentState = ENEMY_STATE.ATTACK;

                yield break;
            }

            //Wait until next frame
            yield return null;
        }
    }
    //------------------------------------------
        public IEnumerator AIAttack() {
                //Loop while chasing and attacking
                while (currentstate == ENEMY_STATE.ATTACK) {
           
                    ThisAgent.isStopped = true;

                    //Rotate to player
                    Vector3 lookRotation = PlayerTransform.position - this.transform.position;       
                    transform.LookAt(lookRotation);

                    if (ThisAgent.remainingDistance >= ThisAgent.stoppingDistance) {

                        CurrentState = ENEMY_STATE.CHASE;
                        yield break;

                    }   else{

                        ThisAnim.SetBool("Idle", false);
                        ThisAnim.SetBool("Move", false);
                        ThisAnim.SetBool("Defend", false);
                        ThisAnim.SetBool("Attack", true);

                    }

                     yield return null;

                 } 
                
        }

    }
