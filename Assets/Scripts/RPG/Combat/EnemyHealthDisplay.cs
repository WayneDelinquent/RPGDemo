using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
	public class EnemyHealthDisplay : MonoBehaviour
	{
		private Fighter _target;

		private void Awake()
		{
			_target = GameObject.FindWithTag("Player").GetComponent<Fighter>();
		}

		private void FixedUpdate()
		{
			GetComponent<Text>().text = _target.GetTarget() != null ? $"{_target.GetTarget().GetHealth().GetPercentage()}%" : "N/A";
		}
	}
}
