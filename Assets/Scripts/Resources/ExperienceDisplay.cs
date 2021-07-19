using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources {
    
    public class ExperienceDisplay : MonoBehaviour {
        
        Experience experience; 
        Text experienceText; 

        private void Awake() {
            
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            experienceText = GetComponent<Text>();
        }

        private void Update() {
            experienceText.text = String.Format("{0:0}", experience.GetPoints());
        }


    }
}