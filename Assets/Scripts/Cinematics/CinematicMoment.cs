using UnityEngine;
using UnityEngine.Playables;
using RPG.Movement;
using RPG.Control;
using RPG.Core;


namespace RPG.Cinematics {

    public class CinematicMoment : MonoBehaviour, IMoment {
        

        /* This is all really bad and should never be done. It was just a quick prototype to see how a cinematic moment could look. plz forgive */

        GameObject player; 
        GameObject deadEnemy; 
        GameObject chaseEnemy;
        bool sequenceStarted; 
        PlayableDirector playableDirector;
        Vector3 playerStartingPosition;
        float rangeTimeBuffer = 0.05f;
        
        public Transform playerMark;
        public Transform enemyMark; 

        void Start() {
            
            player = GameObject.FindWithTag("Player");
            playerStartingPosition = player.transform.position;
            deadEnemy = GameObject.FindWithTag("Killed Enemy");
            chaseEnemy = GameObject.FindWithTag("Chase Enemy");
            deadEnemy.GetComponent<AIController>().enabled = false;
            sequenceStarted = false;
            playableDirector = GetComponent<PlayableDirector>();
            
        }
 
        void Update() {
            
            if (!sequenceStarted) return;

             if (WithinRange(1.35f, rangeTimeBuffer)) {
                 deadEnemy.GetComponent<Mover>().StartMoveAction(enemyMark.position, 0.3f);
                 player.GetComponent<ActionScheduler>().CancelCurrentAction();
                 player.transform.position = playerStartingPosition;
             }

             if (WithinRange(3f, rangeTimeBuffer)) {
                 player.GetComponent<Mover>().StartMoveAction(playerMark.position, 1f);
                 player.transform.LookAt(deadEnemy.transform);
             }

            if (WithinRange(6.5f, rangeTimeBuffer)) {
               player.transform.LookAt(deadEnemy.transform.position);
               deadEnemy.transform.LookAt(player.transform.position);
            }

            if (WithinRange(8f, rangeTimeBuffer)) {
                deadEnemy.GetComponent<Health>().TakeDamage(Mathf.Infinity);
            }

            if (WithinRange(9.05f, rangeTimeBuffer)) {
                chaseEnemy.GetComponent<AIController>().enabled = false;
                chaseEnemy.GetComponent<Mover>().StartMoveAction(player.transform.position, 1f);
            }
        }

        public void StartSequence() {
            sequenceStarted = true;
            player.GetComponent<Mover>().StartMoveAction(playerMark.position, 1f);
        }

        public void StopSequence() {
            chaseEnemy.GetComponent<AIController>().enabled = true;
        }

        private bool WithinRange(float timeToCheck, float closeEnough) {

            return playableDirector.time > (timeToCheck - closeEnough) && playableDirector.time < (timeToCheck + closeEnough); 

        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(playerMark.position, 0.3f);
            Gizmos.DrawSphere(enemyMark.position, 0.3f);
        }
    }
}
