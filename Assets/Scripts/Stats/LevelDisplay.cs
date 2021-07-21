using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats {

    public class LevelDisplay : MonoBehaviour {

        BaseStats baseStats;
        Text progressionText; 

        private void Awake() {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            progressionText = GetComponent<Text>();
        }

        private void Update() {
            progressionText.text = string.Format("{0:0}", baseStats.GetLevel());
        }
        
    }
}
