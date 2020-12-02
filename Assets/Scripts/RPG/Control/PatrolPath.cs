using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Control
{
	public class PatrolPath : MonoBehaviour
	{
		public List<Transform> waypoints;

		private void Awake()
		{
			waypoints = GetComponentsInChildren<Transform>().Where(x => x != transform).ToList();
		}

		private void OnDrawGizmos()
		{
			waypoints = GetComponentsInChildren<Transform>().Where(x => x != transform).ToList();
			for (int index = 0; index < waypoints.Count; index++)
			{
				Gizmos.DrawSphere(waypoints[index].position, 0.5f);
				if (index > 0)
				{
					Gizmos.DrawLine(waypoints[index].position, waypoints[index - 1].position);
				}
				else
				{
					Gizmos.DrawLine(waypoints[0].position, waypoints[waypoints.Count - 1].position);
				}
			}
		}
	}
}
