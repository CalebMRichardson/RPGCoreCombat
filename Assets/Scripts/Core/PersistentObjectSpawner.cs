using UnityEngine;

namespace RPG.Core
{

    public class PersistentObjectSpawner : MonoBehaviour {

        [SerializeField] GameObject persistentObjectPrefab;

        static bool hasSpawned = false;

        private void Awake() {
            if (hasSpawned) {
                return;
            }
            
            SpawnPersistenObjects();

            hasSpawned = true;
        }

        private void SpawnPersistenObjects() {
            
            GameObject peristentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(peristentObject);
        }
    }
}
