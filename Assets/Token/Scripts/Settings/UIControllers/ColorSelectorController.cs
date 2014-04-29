using UnityEngine;
using System.Collections;

public class ColorSelectorController : MonoBehaviour
{
    [SerializeField]
    tk2dUIDropDownMenu _comboBox;

	private bool _isStarted;

	private void Start()
	{		
		Settings.Instance.ChipColorChanged += (sender, e) => this.OnColorSettingsChanged();

		this.OnColorSettingsChanged();

		this._isStarted = true;
	}

	private void OnColorSettingsChanged()
	{
		_comboBox.Index = (int)Settings.Instance.ChipColor;
	}

	private void OnDropDownIndexChanged()
    {
		if (this._isStarted)
			Settings.Instance.ChipColor = (ChipColors)_comboBox.Index;
    }
}
