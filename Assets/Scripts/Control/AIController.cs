using UnityEngine;
using RPG.Core;
using RPG.Combat;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;
using System;

namespace RPG.Control {
    
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggroCooldownTime = 5f;
        [SerializeField] PatrolPath patrolPath; 
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0,1)] [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance = 5f; 

        GameObject player;
        Fighter fighter; 
        Health health; 
        Mover mover; 
        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
       
        int currentWaypointIndex = 0;

        private void Awake() {
            
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition() {
            return transform.position;
        }
    
        private void Start() {
            guardPosition.ForceInit();
        }

        private void Update() {

            if (health.IsDead()) return;

            if (IsAggrevated() && fighter.CanAttack(player)) {

                AttackBehaviour();
            }

            else if (timeSinceLastSawPlayer < suspicionTime) {
                // Suspicion state
                SuspicionBehaviour();
            
            } else {
                
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        public void Aggrevate() {

            timeSinceAggrevated = 0;
        }

        private bool IsAggrevated() {

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            // check aggrevated
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggroCooldownTime;
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null) {
                
                if (AtWayPoint()) {
                    
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }

        private Vector3 GetCurrentWaypoint() {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint() {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWayPoint() {

            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance; 
        }

        private void SuspicionBehaviour() {

            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour() {

            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies() {
            
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

            foreach (RaycastHit hit in hits) {
                AIController ai = hit.collider.GetComponent<AIController>(); 
                
                if (ai == null) continue;

                ai.Aggrevate();
            }

        }

        // Called by unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }



        private void UpdateTimers() {

            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

    }
}
