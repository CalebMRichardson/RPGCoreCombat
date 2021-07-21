using UnityEngine;

namespace RPG.Core {

    public class DestoyAfterEffect : MonoBehaviour {
        
        [SerializeField] GameObject targetToDestory = null;
        
        void Update() {
            
            if (!GetComponent<ParticleSystem>().IsAlive()) {
                
                if (targetToDestory != null) {
                    Destroy(targetToDestory);
                } else {
                    Destroy(gameObject);
                }
            }
        }
    }
}
