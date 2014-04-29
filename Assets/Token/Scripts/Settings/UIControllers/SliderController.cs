using System;
using UnityEngine;
using System.Collections;

public abstract class SliderController : MonoBehaviour
{
    [SerializeField]
    protected tk2dUIScrollbar Slider;

    [SerializeField]
    protected tk2dUITextInput TextInput;

    [SerializeField]
    protected int MinSliderValue;

    [SerializeField]
    protected int MaxSliderValue;

	protected float SliderValue;
	
	protected string TextInputValue;
	
	private bool _isSlideHold;
	
	private bool _isStarted;
	
	private GameObject _innerTextInputField;

    private int _value;
    public virtual int Value
    {
        get { return _value; }
        set
        {
            if (value > MaxSliderValue)
            {
                _value = MaxSliderValue;
            }
            else if (value < MinSliderValue)
            {
                _value = MinSliderValue;
            }
            else
            {
                _value = value;
            }

            var diff = MaxSliderValue - MinSliderValue;

            SliderValue = ((float) (value - MinSliderValue))/diff;
			if (SliderValue >= 1f)
			{
				SliderValue = 0.999999f;
			}
            TextInputValue = FormatTextInput(value.ToString());					

            TextInput.Text = TextInputValue;
        }
    }	   

    protected virtual string FormatTextInput(string val)
    {
        return val;
    }

	protected abstract void OnSaveValue();

    protected virtual void OnStart()
    {
		this._isStarted = true;
		_innerTextInputField = TextInput.transform.FindChild("TextBoxStart/Cursor").gameObject;
    }   

	protected void OnSliderFocus()
    {
		this._isSlideHold = true;
    }

	protected void OnSliderRelease()
    {
		this._isSlideHold = false;
    }

	protected void OnTextInputFocus()
	{	
		this.TextInput.Text = this.Value.ToString();
	}

	protected void OnSlidedValueChanged()
    {
		if (!this._isStarted || Mathf.Abs(this.Slider.Value - this.SliderValue) < 0.001f)
            return;

		var diff = this.Slider.Value * (MaxSliderValue - MinSliderValue);
		this.Value = (int)(MinSliderValue + diff);
	}

	private void Update()
	{	
		if (!_innerTextInputField.activeSelf && this.TextInputValue != this.TextInput.Text)
		{		
			var i = 0;
			if (Int32.TryParse(this.TextInput.Text, out i))
			{
				this.Value = i;
				this.OnSaveValue();
			}
			else
			{
				this.TextInput.Text = this.TextInputValue;
			}
		}

		if (!this._isSlideHold && this.Slider.Value != this.SliderValue)
		{
			this.Slider.Value = this.SliderValue;
			this.OnSaveValue();
		}
	}
}
