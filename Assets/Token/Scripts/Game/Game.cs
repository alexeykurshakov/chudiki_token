using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour 
{
	private UnityEngine.Color kBgColorBoyStyle = new Color(70f/255f, 89f/255f, 180f/255f);
	private UnityEngine.Color kBgColorGirlStyle = new Color(184f/255f, 32f/255f, 161f/255f);

	[SerializeField] private GameObject _effectsRoot;

	[SerializeField] private tk2dSlicedSprite _background;

	[SerializeField] private Chip _chipPrefab;

	[SerializeField] private GameObject _grid;

    private int _currentStep;

	private Chip[,] _chips;

	private bool _isPaused;
    public bool IsPaused 
	{ 
		get { return _isPaused; }
		set
		{
			_isPaused = value;
			if (!value)
			{
				_isTouch = false;
			}
		}
	}

    private bool _isTouch;

    private Vector3 _touchPos;

	private ParticleSystem _ps;

	private void OnStyleApply()
	{
		switch (Settings.Instance.GameStyle)
		{
		case GameStyles.Boy:
			this._background.color = kBgColorBoyStyle;
			break;

		case GameStyles.Girl:
			this._background.color = kBgColorGirlStyle;
			break;
		}
	}

	private void OnSettingsButtonDown()
	{
		if (!this.IsPaused)
		{
			this.IsPaused = true;
		}
	}

	private void OnSettingsButtonClick()
	{
		this.SendMessageUpwards("OnSettingsShow");
	}

    private void Reset()
    {
        _currentStep = 0;
        _isTouch = false;
    }

	private float _marginX;
	private float _marginY;


    private void OnGridCreation()
    {
        this.Reset();

        var gameSettings = Settings.Instance;
		var screenWidth = (float)1024;//Screen.width;
		var screenHeight = (float)768;//Screen.height;

        var paddingX = screenWidth/(gameSettings.ChipCountInLine + 1);
        _marginX = paddingX;

        var paddingY = screenHeight / (gameSettings.LinesCount + 1);
        _marginY = paddingY;

        _chips = new Chip[gameSettings.ChipCountInLine, gameSettings.LinesCount];

        {
            var childrens = new List<GameObject>();
            foreach (Transform child in this._grid.transform)
            {
                childrens.Add(child.gameObject);
            }
            childrens.ForEach(child => Destroy(child));
			this._grid.transform.DetachChildren();
        }        

        for (var y = 0; y < gameSettings.LinesCount; ++y)
        {
            for (var x = 0; x < gameSettings.ChipCountInLine; ++x)
            {
				var offset = new Vector2(_marginX+x*paddingX, -_marginY-y*paddingY);
                var chip = Chip.Create(this._chipPrefab.gameObject, offset);

                chip.gameObject.SetActive(true);
                chip.transform.parent = this._grid.transform;
                _chips[x, y] = chip;                
            }
        }
    }

    private void Start ()
	{
	    var gameSettings = Settings.Instance;

        gameSettings.GameStyleChanged += (object sender, System.EventArgs e) => this.OnStyleApply();
        gameSettings.ChipCountInLineChanged += (object sender, System.EventArgs e) => this.OnGridCreation();
        gameSettings.LinesCountChanged += (object sender, System.EventArgs e) => this.OnGridCreation();			       

        this.OnGridCreation(); 
        this.OnStyleApply();
	}

	IEnumerator Win() 
	{
		yield return new WaitForSeconds(4.0f);
		SoundManager.Instance.GetSound(SoundManager.Sounds.Fanfary).Play();
	}

	private ParticleSystem GetCurrentEffect()
	{
		var newPs = _effectsRoot.transform.FindChild(Settings.Instance.WinEffect).gameObject.GetComponent<EffectContainer>().Play;	
		if (_ps != null && _ps != newPs)
		{
			_ps.gameObject.SetActive(false);
		}
		_ps = newPs;
		return _ps;
	}

    private void OnTouch()
    {
        var gameSettings = Settings.Instance;
		if (_currentStep == gameSettings.ChipCountInLine * gameSettings.LinesCount)
		{
			return;
		}

        var soundToPlay = string.Empty;
        switch (gameSettings.TapSoundType)
        {
            case 0:
                soundToPlay = string.Format("Tap{0}", (_currentStep % 12)+1);
                break;

            case 1:
                soundToPlay = string.Format("TapDzin{0}", (_currentStep % 8)+1);
                break;

            case 2:
                soundToPlay = string.Format("TapKvak{0}", (_currentStep % 6)+1);
                break;
        }
        SoundManager.Instance.GetSound(soundToPlay).Play();

        var currentLine = _currentStep / gameSettings.ChipCountInLine;
        _chips[_currentStep++ % gameSettings.ChipCountInLine, currentLine].State = Chip.States.Normal;

        var newLine = _currentStep/gameSettings.ChipCountInLine;
		
        if (newLine != currentLine)
        {
			var ps = GetCurrentEffect();
			ps.GetComponent<tk2dCameraAnchor>().AnchorOffsetPixels = new Vector2(gameSettings.ChipCountInLine * _marginX, -newLine * _marginY);
			ps.gameObject.SetActive(true);		
			ps.Play();			
			SoundManager.Instance.GetSound(SoundManager.Sounds.IskorkiDev).Play();

		
            for (var x = 0; x < gameSettings.ChipCountInLine; ++x)
            {
                _chips[x, currentLine].State = Chip.States.Dark;
            }

			if (_currentStep == gameSettings.ChipCountInLine * gameSettings.LinesCount)
			{
				this.StartCoroutine("Win");
			}
        }      
    }

    private void OnSwipe(Vector2 swipeDirection)
    {
		if (_currentStep == 0)
			return;
		        
		SoundManager.Instance.GetSound(string.Format("Swipe{0}", Random.Range(1, 4))).Play();

        var gameSettings = Settings.Instance;
        var currentLine = _currentStep / gameSettings.ChipCountInLine;
        var currentCol = _currentStep % gameSettings.ChipCountInLine;
        if (currentCol == 0)
        {
            --currentLine;
            currentCol = gameSettings.ChipCountInLine;
        }
        
        for (var x = 0; x < currentCol; ++x)
        {
            _chips[x, currentLine].State = Chip.States.NotSelected;
        }
        _currentStep = currentLine * gameSettings.ChipCountInLine;
    }    
	
	private void Update ()
	{
	    if (IsPaused)
	        return;

	    if (_isTouch)
	    {
            if (Input.GetMouseButtonUp(0))
            {
                _isTouch = false;
                var diff = (Input.mousePosition - _touchPos);                
                if (diff.sqrMagnitude > 200)
                {
                    this.OnSwipe(diff);
                }
                else
                {
                    this.OnTouch();
                }
            }
	        return;
	    }

	    if (Input.GetMouseButtonDown(0))
	    {
	        _touchPos = Input.mousePosition;
	        _isTouch = true;
	    }	    
	}
}
