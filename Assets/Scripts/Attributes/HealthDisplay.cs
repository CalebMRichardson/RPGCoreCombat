using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes {
    
    public class HealthDisplay : MonoBehaviour {
        
        Health health; 
        Text healthText; 

        private void Awake() {
            
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthText = GetComponent<Text>();
        }

        private void Update() {
            healthText.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}