using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control {
public class PlayerController : MonoBehaviour {
    
        private const int LEFT_MOUSE_BUTTON = 0;
        private Mover mover; 
        private Health health;

        private void Start() {

            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }

        private void Update() {

            if (health.IsDead()) return;

            if (InteractWithCombat()) return;

            if (InteractWithMovement()) return;
            
        }

        private bool InteractWithCombat() {

            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            
            foreach(RaycastHit hit in hits) {
                
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) {
                    continue;
                }

                if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON)) {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }

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
                return true;
            }
            return false;
        }

        private Ray GetMouseRay() {

            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}