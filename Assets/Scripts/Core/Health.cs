using UnityEngine;

namespace RPG.Core {
    public class Health : MonoBehaviour {
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
    
    
    } // end of health.cs
}