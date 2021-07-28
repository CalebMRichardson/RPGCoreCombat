using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;

namespace RPG.Resources {

    public class Health : MonoBehaviour, ISaveable {

        [SerializeField] float regenerationPercentage = 70;

        bool isDead = false;
        LazyValue<float> healthPoints;
        BaseStats baseStats = null;

        private void Awake() {
            healthPoints = new LazyValue<float>(GetInitialHealth);
            baseStats = GetComponent<BaseStats>();
        }

        private float GetInitialHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable() {
            baseStats.onLevelUp += RegenerateHealth;
        }

        private void OnDisable() {
            baseStats.onLevelUp -= RegenerateHealth;    
        }

        private void Start() {

            healthPoints.ForceInit();
        }
        
        public bool IsDead() {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage) {

            print(gameObject.name + " took damage: " + damage);

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0); 
            if (healthPoints.value <= 0) {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints() {
            return healthPoints.value;
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
            return 100 * (healthPoints.value / baseStats.GetStat(Stat.Health));
        }

        private void AwardExperience(GameObject instigator) {

            Experience experience = instigator.GetComponent<Experience>();

            if (experience == null) return;
            experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth() {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * regenerationPercentage / 100;
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        // Called by ISaveable
        public object CaptureState() {
            
            return healthPoints.value;
        }

        // Called by ISaveable
        public void RestoreState(object state) {

            healthPoints.value = (float)state;

            if (healthPoints.value <= 0) {
                Die();
            }
        }

    } // end of health.cs
}