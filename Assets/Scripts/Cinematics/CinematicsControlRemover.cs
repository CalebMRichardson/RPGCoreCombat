using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics {

    public class CinematicsControlRemover : MonoBehaviour {
        
        GameObject player; 
        IMoment[] moments; 

        void Start() {
        
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
            moments = transform.GetComponents<IMoment>(); 
        }

        void DisableControl(PlayableDirector pd) {
            
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;

            for (int i = 0; i < moments.Length; i++) {
                moments[i].StartSequence();
            }

        }

        void EnableControl(PlayableDirector pd) {

            player.GetComponent<PlayerController>().enabled = true; 
            
            for (int i = 0; i < moments.Length; i++) {
                moments[i].StopSequence();
            }
        }

    }

}