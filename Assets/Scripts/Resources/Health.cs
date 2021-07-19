using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources {

    public class Health : MonoBehaviour, ISaveable {

        private bool isDead = false;
        [SerializeField] private float healthPoints = 100f;
        BaseStats baseStats = null;

        private void Start() {
            baseStats = GetComponent<BaseStats>(); 
            healthPoints = baseStats.GetStat(Stat.Health);
        }
        
        public bool IsDead() {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage) {

            healthPoints = Mathf.Max(healthPoints - damage, 0); 
            if (healthPoints <= 0) {
                Die();
                AwardExperience(instigator);
            }
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
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
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