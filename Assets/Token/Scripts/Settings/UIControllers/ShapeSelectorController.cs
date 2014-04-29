using UnityEngine;
using System.Collections;

public class ShapeSelectorController : MonoBehaviour
{
    [SerializeField]
    tk2dUIToggleButtonGroup _radioGroup;

	private bool _isStarted;

	private void Start()
	{	
		Settings.Instance.ChipShapeChanged += (sender, e) => this.OnShapeSettingsChanged();

		this.OnShapeSettingsChanged();

		this._isStarted = true;
	}
	
	private void OnShapeSettingsChanged()
	{
		_radioGroup.SelectedIndex = (int)Settings.Instance.ChipShape;
	}
	
	private void OnRadioGroupIndexChanged()
	{
		if (this._isStarted)
			Settings.Instance.ChipShape = (ChipShapes)_radioGroup.SelectedIndex;
	}
}
