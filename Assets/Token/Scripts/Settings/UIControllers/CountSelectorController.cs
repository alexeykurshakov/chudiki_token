using UnityEngine;
using System.Collections;

public class CountSelectorController : SliderController
{	   
    private void Start()
	{
		Settings.Instance.ChipCountInLineChanged += (sender, e) => this.OnChipCountInLineApply();
		
		this.OnChipCountInLineApply();
		
		this.OnStart();

    }

	private void OnChipCountInLineApply()
	{
		this.Value = Settings.Instance.ChipCountInLine;
	}

	protected override void OnSaveValue ()
	{
		Settings.Instance.ChipCountInLine = this.Value;
	}		
}
