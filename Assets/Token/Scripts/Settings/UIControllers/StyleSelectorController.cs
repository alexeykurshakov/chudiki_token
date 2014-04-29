using UnityEngine;
using System.Collections;

public class StyleSelectorController : MonoBehaviour
{
    [SerializeField]
    tk2dUIToggleButtonGroup _radioGroup;

	private bool _isStarted;

	private void Start()
	{
		Settings.Instance.GameStyleChanged += (sender, e) => this.OnStyleSettingsChanged();

		this.OnStyleSettingsChanged();

		this._isStarted = true;
	}

	private void OnStyleSettingsChanged()
	{
		_radioGroup.SelectedIndex = (int)Settings.Instance.GameStyle;
	}
	
	private void OnRadioGroupIndexChanged()
	{
		if (this._isStarted)
			Settings.Instance.GameStyle = (GameStyles)_radioGroup.SelectedIndex;
	}
}
