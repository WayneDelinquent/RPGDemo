using UnityEngine;

namespace RPG.Progression
{
	public class BaseStats : MonoBehaviour
	{
		[Range(1, 99)]
		[SerializeField] private int startingLevel = 1;
		[SerializeField] private CharacterClass characterClass;
		[SerializeField] private Progression progression = null;

		public float GetHealth()
		{
			return progression.GetHealth(characterClass, startingLevel);
		}
	}
}
