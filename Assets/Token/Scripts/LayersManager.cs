using UnityEngine;
using System.Collections;

public class LayersManager : MonoBehaviour
{
    [SerializeField] private SettingsController _settingsController;

	[SerializeField] private Game _game;

    private void OnSettingsShow()
    {		
        _settingsController.Show();
    }    

	private void OnSettingsClose()
	{
		_game.IsPaused = false;
		_settingsController.Hide();
	}
}
