using System;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
	public class CinematicTrigger : MonoBehaviour
	{
		private bool _hasTriggered = false;
		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Player")) return;
			if (_hasTriggered) return;
			
			_hasTriggered = true;
			GetComponent<PlayableDirector>().Play();

		}
	}
}
