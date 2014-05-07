using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EffectSelectorController : MonoBehaviour
{	  
    [SerializeField] tk2dUIDropDownMenu _comboBox;

	[SerializeField] private GameObject _effectsRoot;

	private ParticleSystem _ps;

	private bool _isStarted;
	    
	private void Start()
	{
	    var items = new List<string>();
	    var it = 0; var index = 0;
	    var needItemName = Settings.Instance.WinEffect;
	    foreach (Transform child in  _effectsRoot.transform)
	    {
	        items.Add(child.name);
	        if (child.name == needItemName)
	        {
	            index = it;
	        }
	        it++;
		}	   	
	    _comboBox.Index = index;   
     
		_isStarted = true;
        this.OnEffectSelectionChanged();
	}	 

    private void OnEffectSelectionChanged()
    {
		if (!this._isStarted)
			return;

		var sItem = _comboBox.ItemList[_comboBox.Index];
		var effectContainer = _effectsRoot.transform.FindChild(sItem).gameObject.GetComponent<EffectContainer>();

		this.Close();
		_ps = effectContainer.Show;

		Settings.Instance.WinEffect = sItem;
		_ps.gameObject.SetActive(true);
    }

	public void Close()
	{
		if (_ps != null)
		{
			_ps.gameObject.SetActive(false);
		}
	}
}
