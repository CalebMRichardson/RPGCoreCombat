using UnityEngine;

namespace RPG.Core {

    public class DestoyAfterEffect : MonoBehaviour {
        
        
        void Update() {
            
            if (!GetComponent<ParticleSystem>().IsAlive()) {
                Destroy(gameObject);
            }
        }
    }
}
