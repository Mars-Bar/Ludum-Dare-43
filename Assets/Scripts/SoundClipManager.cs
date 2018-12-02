using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClipManager
	: MonoBehaviour
{
	public enum SoundID
	{
		None,
		ZombieGroan,
	}

	[System.Serializable]
	public class SoundEffectList
	{
		public SoundID effectID;
		public List<AudioClip> Sounds;
	}

	public List<SoundEffectList> SoundEffects;

	public AudioClip GetRandomSoundEffect(SoundID sound)
	{
		SoundEffectList effects = SoundEffects.Find(x => x.effectID == sound);
		if (effects == null)
			return null;

		List<AudioClip> sounds = effects.Sounds;
		return sounds[Random.Range(0, sounds.Count)];
	}
}
