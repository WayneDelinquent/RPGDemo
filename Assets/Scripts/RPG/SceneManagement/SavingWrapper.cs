using System;
using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
	public class SavingWrapper : MonoBehaviour
	{
		private SavingSystem _ss;
		private const string DefaultSaveFile = "save";

		private IEnumerator Start()
		{
			Fader fader = FindObjectOfType<Fader>();
			fader.FadeOutImmediate();
			yield return GetComponent<SavingSystem>().LoadLastScene(DefaultSaveFile);
			yield return fader.FadeIn(1f);
		}

		private void Awake()
		{
			_ss = GetComponent<SavingSystem>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.L))
			{
				Load();
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				Save();
			}
		}

		public void Save()
		{
			_ss.Save(DefaultSaveFile);
		}

		public void Load()
		{
			_ss.Load(DefaultSaveFile);
		}
	}
}
