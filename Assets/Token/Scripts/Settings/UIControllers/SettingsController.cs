using UnityEngine;
using System.Collections;

public class SettingsController : MonoBehaviour
{
	[SerializeField] 
	private EffectSelectorController _effectSelector;

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}

    void OnCloseClick()
    {     
		this.SendMessageUpwards("OnSettingsClose");
		this.SendMessageUpwards("OnSaveSettings");

		_effectSelector.Close ();
    }
}
