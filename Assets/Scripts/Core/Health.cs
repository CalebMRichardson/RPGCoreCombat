using UnityEngine;
using RPG.Saving;

namespace RPG.Core {
    public class Health : MonoBehaviour, ISaveable {

        private bool isDead = false;
        [SerializeField] private float healthPoints = 100f;

        public bool IsDead() {
            return isDead;
        }

        public void TakeDamage(float damage) {

            healthPoints = Mathf.Max(healthPoints - damage, 0); 
            if (healthPoints == 0)
                Die();
        }

        private void Die() {

            if (IsDead()) return; 

    
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        
        // Called by ISaveable
        public object CaptureState() {
            
            return healthPoints;
        }

        // Called by ISaveable
        public void RestoreState(object state) {

            healthPoints = (float)state;

            if (healthPoints <= 0) {
                Die();
            }
        }

    
    } // end of health.cs
}