using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;


namespace RPG.Resources {

    public class Health : MonoBehaviour, ISaveable {

        [SerializeField] float regenerationPercentage = 70;

        private bool isDead = false;
        float healthPoints = -1f;
        BaseStats baseStats = null;

        private void Start() {
            
            baseStats = GetComponent<BaseStats>(); 

            baseStats.onLevelUp += RegenerateHealth;
            
            if (healthPoints < 0) {
                healthPoints = baseStats.GetStat(Stat.Health);
            }
        }
        
        public bool IsDead() {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage) {

            print(gameObject.name + " took damage: " + damage);

            healthPoints = Mathf.Max(healthPoints - damage, 0); 
            if (healthPoints <= 0) {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints() {
            return healthPoints;
        }

        public float GetMaxHealthPoints() {
            return baseStats.GetStat(Stat.Health);
        }

        private void Die() {

            if (IsDead()) return; 

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        
        public float GetPercentage() {
            return 100 * (healthPoints / baseStats.GetStat(Stat.Health));
        }

        private void AwardExperience(GameObject instigator) {

            Experience experience = instigator.GetComponent<Experience>();

            if (experience == null) return;
            experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth() {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * regenerationPercentage / 100;
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
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