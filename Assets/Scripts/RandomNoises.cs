using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoises 
	: MonoBehaviour
{
	private void Awake()
	{
		_soundManager = FindObjectOfType<SoundClipManager>();
		_audioSource = GetComponent<AudioSource>();

		GenerateNewInterval();
	}

	SoundClipManager _soundManager;
	AudioSource _audioSource;

	public float MinTimeBetweenNoises = 1f;
	public float MaxTimeBetweenNoises = 2f;
	public SoundClipManager.SoundID SoundEffectID;

	float _soundCooldown = 0f;
	float _prevSoundDuration = 0f;

	float _soundTimer = 0f;

	void Update ()
	{
		if (GameStateManager.Instance.State != GameStateManager.GameState.Moving)
			return;

		_soundTimer += Time.deltaTime;
		if (_soundTimer > _soundCooldown)
		{
			PlayNoise();
			GenerateNewInterval();
		}
	}

	void GenerateNewInterval()
	{
		_soundCooldown = _prevSoundDuration + Random.Range(MinTimeBetweenNoises, MaxTimeBetweenNoises);
		_soundTimer = 0f;
	}

	void PlayNoise()
	{
		if (_audioSource == null)
			return;

		AudioClip sound = _soundManager.GetRandomSoundEffect(SoundEffectID);

		if (sound == null)
			return;

		_prevSoundDuration = sound.length;
		_audioSource.PlayOneShot(sound);
	}
}
