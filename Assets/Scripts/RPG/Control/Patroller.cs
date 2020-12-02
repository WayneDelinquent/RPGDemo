using System;
using System.Collections.Generic;
using RPG.Core.Interfaces;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
	public class Patroller : MonoBehaviour, ISchedulableAction
	{
		[Range(0, 1)] [SerializeField] private float patrolSpeedFraction = 0.2f;
		[SerializeField] private PatrolPath path;
		[SerializeField] private float patrolWaypointLeeway = 2f;
		[SerializeField] private float sentryDuration = 1f;

		private Mover _mover;

		private bool _isPatrolling = true;
		private int _currentWaypointIndex;
		private float _currentSentryTime = Mathf.Infinity;

		private void Awake()
		{
			_mover = GetComponent<Mover>();
			_currentWaypointIndex = 0;
		}

		private void Update()
		{
			if (!HasPatrolPath() || !_isPatrolling) return;
			if (CheckSentry()) return;
			float distance = Vector3.Distance(transform.position, path.waypoints[_currentWaypointIndex].transform.position);
			if (!(distance < patrolWaypointLeeway)) return;
			StartSentry();
		}

		private void StartSentry()
		{
			_currentSentryTime = 0;
			_mover.Stop();
		}

		private bool CheckSentry()
		{
			if (_currentSentryTime > sentryDuration) return false;
			_currentSentryTime += Time.deltaTime;
			if (!(_currentSentryTime > sentryDuration)) return true;
			_currentWaypointIndex =
				_currentWaypointIndex + 1 < path.waypoints.Count ? _currentWaypointIndex + 1 : 0;
			_mover.StartMoving(path.waypoints[_currentWaypointIndex].position, patrolSpeedFraction);
			return false;
		}

		public bool HasPatrolPath()
		{
			return path != null && path.waypoints?.Count > 0;
		}

		public void StartPatrolling()
		{
			_isPatrolling = true;
			_mover.StartMoving(path.waypoints[_currentWaypointIndex].position, patrolSpeedFraction);
		}

		public void StopAction()
		{
			_isPatrolling = false;
		}
	}
}
