using UnityEngine;
using UnityEngine.AI;
using UnityEditor; 
using RPG.Core;

namespace RPG.Saving {
    
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour {
    
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueIdentifier() {
            return uniqueIdentifier;
        }

        public object CaptureState() {
            
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state) {

            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

#if UNITY_EDITOR
        private void Update() {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; // checks that the prefab is not in the scene (this will return true until the prefab has been added into a scene)

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier"); 

            if (string.IsNullOrEmpty(property.stringValue)) {
                
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

        }
#endif
    }
}