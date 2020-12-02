using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Progression
{
	[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
	public class Progression : ScriptableObject
	{
		[SerializeField] private ProgressionCharacterClass[] progressions;

		public float GetHealth(CharacterClass className, int level)
		{
			List<int> healths = progressions.First(x => x.className == className).health;
			return healths[Mathf.Clamp(level - 1, 0, healths.Count - 1)];
		}

		[Serializable]
		class ProgressionCharacterClass
		{
			[SerializeField] public CharacterClass className;
			[SerializeField] public List<int> damage;
			[SerializeField] public List<int> health;
		}
	}
}
