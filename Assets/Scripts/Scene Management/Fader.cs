using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {

    public class Fader : MonoBehaviour {
    
        CanvasGroup canvasGroup;

        void Start() {
            canvasGroup = GetComponent<CanvasGroup>();  
        }

        IEnumerator FadeOutIn() {
            
            yield return FadeOut(3f);
           
            yield return FadeIn(1f);
        }

        public IEnumerator FadeOut(float time) {

            while (canvasGroup.alpha < 1) {
                
                canvasGroup.alpha += Time.deltaTime / time; // increases alpha by the same amount each frame for [float time] seconds

                yield return null; // waits one frame
            }
        }

        public IEnumerator FadeIn(float time) {
            
            while (canvasGroup.alpha > 0) {

                canvasGroup.alpha -= Time.deltaTime / time; // decreses alpha by the same amount each from for [float time] seconds
                
                yield return null;
            }
        }

    }
}