using UnityEngine;
using System.Collections;

public class EffectContainer : MonoBehaviour 
{
	[SerializeField] private ParticleSystem _play;

	public ParticleSystem Play { get { return _play; } }

	[SerializeField] private ParticleSystem _show;

	public ParticleSystem Show { get { return _show; } }
}
