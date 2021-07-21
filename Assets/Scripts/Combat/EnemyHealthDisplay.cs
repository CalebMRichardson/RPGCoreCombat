using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;

namespace RPG.Combat {
    
    public class EnemyHealthDisplay : MonoBehaviour {
        
        Fighter fighter;
        Text healthText; 

        private void Awake() {
            
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>(); 
            healthText = GetComponent<Text>();
            healthText.text = String.Empty;
        }

        private void Update() {
            
            if (fighter.GetTarget() == null) {
                healthText.text = String.Empty;
                return;
            }

            Health health = fighter.GetTarget().GetComponent<Health>();

            healthText.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}