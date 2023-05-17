using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform SpawnPoint;
        [SerializeField] DestinationIdentifier Destination;
        [SerializeField] float FadeInTime = 3f;
        [SerializeField] float FadeOutTime = 3f;
        [SerializeField] float FadeWaitTime = 0.3f;
        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());

            }
        }
        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }
            Fader fader = FindObjectOfType<Fader>();
            //SceneManager.LoadScene(sceneToLoad);
            DontDestroyOnLoad(gameObject);

            PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled= false;

            yield return fader.FadeOut(FadeOutTime);

            SavingWraper wraper = FindObjectOfType<SavingWraper>();

            wraper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            PlayerController newplayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            newplayerController.enabled = false;
            wraper.Load();
            
            

            Portal otherPortal = GetOtherPortal();

            UpdatePlayer(otherPortal);
            
            wraper.Save();
            
            yield return new WaitForSeconds(FadeWaitTime);
            fader.FadeIn(FadeInTime);

            newplayerController.enabled = true;
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            Transform Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            //Player.GetComponent<NashMeshAgent>().Warp(otherPortal.spawnPoint.position can solve issue with nevmesh not liking TP locayion 
            Player.GetComponent<NavMeshAgent>().enabled = false;

            Player.SetPositionAndRotation(otherPortal.SpawnPoint.position, otherPortal.SpawnPoint.rotation);
            Player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {


            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this || portal.Destination != Destination)
                {
                    continue;
                }
                return portal;
            }
            return null;

        }
    }
}