using UnityEngine;
using System.Collections;

public class SoundSelectorController : MonoBehaviour
{
	[SerializeField]
	tk2dUIToggleButtonGroup _radioGroup;
	
	private bool _isStarted;
	
	private void Start()
	{	
		_radioGroup.SelectedIndex = Settings.Instance.TapSoundType;		
		this._isStarted = true;
	}
	
	private void OnRadioGroupIndexChanged()
	{
		if (!this._isStarted)
			return;

		switch (_radioGroup.SelectedIndex)
		{
		case 0:
			SoundManager.Instance.GetSound(SoundManager.Sounds.Tap1).Play();
			break;

		case 1:
			SoundManager.Instance.GetSound(SoundManager.Sounds.TapDzin1).Play();
			break;

		case 2:
			SoundManager.Instance.GetSound(SoundManager.Sounds.TapKvak1).Play();
			break;
		}

		Settings.Instance.TapSoundType = _radioGroup.SelectedIndex;
	}
}
