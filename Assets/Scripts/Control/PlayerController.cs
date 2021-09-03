using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using System;

namespace RPG.Control {
public class PlayerController : MonoBehaviour {
    
        private const int LEFT_MOUSE_BUTTON = 0;
        private Mover mover; 
        private Health health;

        [System.Serializable]
        struct CursorMapping {
            public CursorType type; 
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        private void Awake() {

            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }

        private void Update() {

            if (InteractWithUI()) return;

            if (health.IsDead()) {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;

            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
            
        }

        private bool InteractWithComponent() {
            
            RaycastHit[] hits = RaycastAllSorted();
        
            foreach(RaycastHit hit in hits) {
                
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                
                foreach(IRaycastable raycastable in raycastables) {
                    
                    if (raycastable.HandleRaycast(this)) {
                    
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastAllSorted() {

            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++) {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithUI() {
            
            if (EventSystem.current.IsPointerOverGameObject()) {
                SetCursor(CursorType.UI);
                return true;
            } 

            return false;
        }

        private bool  InteractWithMovement() {

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit) {
                if (Input.GetMouseButton(LEFT_MOUSE_BUTTON)) {
                    mover.StartMoveAction(hit.point, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType type) {

            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type) {
           
            foreach (CursorMapping mapping in cursorMappings) {
                if (mapping.type == type) {
                    return mapping;  
                }
            }
            print("Default");
           return cursorMappings[0];
        }

        private Ray GetMouseRay() {

            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}