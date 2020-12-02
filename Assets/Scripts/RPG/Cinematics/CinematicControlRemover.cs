using System;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
	public class CinematicControlRemover : MonoBehaviour
	{
		private ActionScheduler _as;
		private PlayerController _player;

		private void Start()
		{
			GetComponent<PlayableDirector>().played += DisableControl;
			GetComponent<PlayableDirector>().stopped += EnableControl;
			GameObject player = GameObject.FindWithTag("Player");
			_as = player.GetComponent<ActionScheduler>();
			_player = player.GetComponent<PlayerController>();
		}

		private void DisableControl(PlayableDirector _)
		{
			_as.StopCurrentAction();
			_player.enabled = false;
		}

		private void EnableControl(PlayableDirector _)
		{
			_player.enabled = true;
		}
	}
}
