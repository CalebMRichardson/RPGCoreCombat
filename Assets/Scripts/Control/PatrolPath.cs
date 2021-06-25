using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control {
    
    public class PatrolPath : MonoBehaviour {

        [SerializeField] float wayPointRadius = 0.5f;

        private void OnDrawGizmos() {

            for (int i = 0; i < transform.childCount; i++) {

                Transform waypoint = transform.GetChild(i);
                Gizmos.DrawSphere(waypoint.position, wayPointRadius);

                int j = GetNextIndex(i);

                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(j).position);
            }
        } // end of OnDrawGizmos

        public int GetNextIndex(int i) {

            if (i + 1 == transform.childCount) {
                return 0;
            }
            
            return i + 1; 
        }

        public Vector3 GetWaypoint(int i) {
            
            return transform.GetChild(i).position;
        }

    } // end of PatrolPath.cs
}

