using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[Header("------------ Audio Source ------------")]
	[SerializeField] AudioSource MusicSource;
	[SerializeField] AudioSource SFXSource;

	[Header("------------ Audio Clip ------------")]
	public AudioClip Background;
	public AudioClip Death;
	public AudioClip Dodge;
	public AudioClip Jump;
	public AudioClip CoinCollect;
	public AudioClip Slash;

	private void Start()
	{
		// Assign background music and play it
		MusicSource.clip = Background;
		MusicSource.Play();
	}

	public void PlaySFX(AudioClip clip)
	{
		// Play a sound effect using SFXSource
		SFXSource.PlayOneShot(clip);
	}

	public void StopBackgroundMusic()
	{
		MusicSource.Stop();
	}
}
