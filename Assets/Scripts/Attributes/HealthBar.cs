using UnityEngine;

namespace RPG.Attributes {
 
    public class HealthBar : MonoBehaviour {

        [SerializeField] Health healthComponenet = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        private void Update() {

            if (Mathf.Approximately(healthComponenet.GetFraction(), 0)
                || Mathf.Approximately(healthComponenet.GetFraction(), 1)) {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;

            foreground.localScale = new Vector3(healthComponenet.GetFraction(), 1, 1);
        }
    }
}