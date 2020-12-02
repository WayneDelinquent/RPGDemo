using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
	public class Portal : MonoBehaviour
	{
		private enum DestinationIdentifier
		{
			A, B, C
		}

		[SerializeField] private int targetScene;
		[SerializeField] private Transform spawnPoint;
		[SerializeField] private DestinationIdentifier destination;

		[SerializeField] private float fadeOutTime = 2f;
		[SerializeField] private float fadeInTime = 1f;
		[SerializeField] private float fadeWaitTime = 0.5f;
		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Player")) return;
			StartCoroutine(SceneTransition());
		}

		private IEnumerator SceneTransition()
		{
			DontDestroyOnLoad(gameObject);
			Fader fader = FindObjectOfType<Fader>();

			yield return fader.FadeOut(fadeOutTime);

			SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
			wrapper.Save();

			yield return SceneManager.LoadSceneAsync(targetScene);

			wrapper.Load();

			Portal otherPortal = GetOtherPortal();
			UpdatePlayer(otherPortal);

			wrapper.Save();

			yield return new WaitForSeconds(fadeWaitTime);
			yield return fader.FadeIn(fadeInTime);

			Destroy(gameObject);
		}

		private void UpdatePlayer(Portal otherPortal)
		{
			if (otherPortal == null) return;
			GameObject player = GameObject.FindWithTag("Player");
			player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
			player.transform.rotation = otherPortal.spawnPoint.rotation;
		}

		private Portal GetOtherPortal()
		{
			foreach (Portal portal in FindObjectsOfType<Portal>())
			{
				if (portal == this) continue;
				if (portal.destination == destination) return portal;
			}

			return null;
		}
	}
}
