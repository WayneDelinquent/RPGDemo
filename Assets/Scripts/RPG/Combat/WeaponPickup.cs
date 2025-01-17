using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
	public class WeaponPickup : MonoBehaviour
	{
		[SerializeField] private Weapon weapon;
		[SerializeField] private float respawnTime = 5f;
		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Player")) return;
			other.GetComponent<Fighter>().EquipWeapon(weapon);
			StartCoroutine(HideForSeconds(respawnTime));
		}

		private IEnumerator HideForSeconds(float seconds)
		{
			ShowPickup(false);
			yield return new WaitForSeconds(seconds);
			ShowPickup(true);
		}

		private void ShowPickup(bool shouldShow)
		{
			Collider collider = GetComponent<Collider>();
			collider.enabled = shouldShow;
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(shouldShow);
			}
		}
	}
}
