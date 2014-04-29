using UnityEngine;
using System.Collections;

public class SeriaCountSelectorController : SliderController
{	  
	private void Start()
	{	
		Settings.Instance.LinesCountChanged += (sender, e) => this.OnLineCountApply();

		this.OnLineCountApply();

		this.OnStart();
	}

	private void OnLineCountApply()
	{
		this.Value = Settings.Instance.LinesCount;
	}

	protected override void OnSaveValue ()
	{
		Settings.Instance.LinesCount = this.Value;
	}
}
