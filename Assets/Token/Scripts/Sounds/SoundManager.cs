using System;
using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	public enum Sounds
	{
		Fanfary,

		Swipe1,   
		Swipe2,    
		Swipe3,   

		Tap1,
		Tap2,
		Tap3,
		Tap4,
		Tap5,
		Tap6,
		Tap7,
		Tap8,
		Tap9,
		Tap10,
		Tap11,
		Tap12,

		TapDzin1,
		TapDzin2,
		TapDzin3,
		TapDzin4,
		TapDzin5,
		TapDzin6,
		TapDzin7,
		TapDzin8,

		TapKvak1,
		TapKvak2,
		TapKvak3,
		TapKvak4,
		TapKvak5,
		TapKvak6,	
	}
	
	private AudioSource[] _sounds;
	
	private bool _isSoundEnabled;
	public bool IsSoundEnabled 
	{ 
		get { return _isSoundEnabled; }
		set
		{
			if (_isSoundEnabled == value)
				return;
			
			_isSoundEnabled = value;
			AudioListener.volume = value ? 1 : 0;
		}
	}
	
	public static SoundManager Instance { get; private set; }
	
	public AudioSource GetSound(Sounds type)
	{
		return _sounds[(int) type];        
	}

    public AudioSource GetSound(string type)
    {
        var sound = (Sounds)Enum.Parse(typeof (Sounds), type);
        return _sounds[(int) sound];
    }
	
	public void StopAll()
	{
		foreach (var sound in _sounds)
		{
			sound.Stop ();
		}
	}
	
	private void Awake()
	{
		Instance = this;
		
		_isSoundEnabled = true;
		
		var enumSounds = (Sounds[])Enum.GetValues(typeof(Sounds));
		_sounds = new AudioSource[enumSounds.Length];
		
		foreach (Sounds sound in enumSounds)
		{
			_sounds[(int)sound] = CreateAudioSource(sound.ToString().ToLower());
		}
	}
	
	private AudioSource CreateAudioSource(string clipName)
	{
		var obj = new GameObject();
		obj.name = clipName;
		
		var val = obj.AddComponent<AudioSource>();
		val.transform.parent = transform;
		val.transform.localPosition = new Vector3();
		val.clip = Resources.Load("Sounds/" + clipName) as AudioClip;
		
		return val;
	}	
}
