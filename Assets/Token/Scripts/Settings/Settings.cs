using System;
using System.Globalization;
using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }    

    public event EventHandler GameStyleChanged;

    public event EventHandler ChipShapeChanged;

    public event EventHandler ChipColorChanged;

    public event EventHandler WinEffectChanged;

    public event EventHandler ChipCountInLineChanged;

    public event EventHandler LinesCountChanged;     

    private GameStyles _gameStyle;
    public GameStyles GameStyle
    {
        get { return _gameStyle; }
        set
        {
			if (_gameStyle == value)
				return;

            _gameStyle = value;
            if (GameStyleChanged != null)
            {
                GameStyleChanged(this, EventArgs.Empty);
            }
        }
    }

    private ChipShapes _chipShape;
    public ChipShapes ChipShape
    {
        get { return _chipShape; }
        set
        {
			if (_chipShape == value)
				return;

            _chipShape = value;
            if (ChipShapeChanged != null)
            {
                ChipShapeChanged(this, EventArgs.Empty);
            }
        }
    }

    private ChipColors _chipColor;
    public ChipColors ChipColor
    {
        get { return _chipColor; }
        set
        {
			if (_chipColor == value)
				return;

            _chipColor = value;
            if (ChipColorChanged != null)
            {
                ChipColorChanged(this, EventArgs.Empty);
            }
        }
    }

    private WinEffects _winEffect;
    public WinEffects WinEffect
    {
        get { return _winEffect; }
        set
        {
			if (_winEffect == value)
				return;

            _winEffect = value;
            if (WinEffectChanged != null)
            {
                WinEffectChanged(this, EventArgs.Empty);
            }
        }
    }

    private int _chipCountInLine;
    public int ChipCountInLine
    {
        get { return _chipCountInLine; }
        set
        {
			if (_chipCountInLine == value)
				return;

            _chipCountInLine = value;
            if (ChipCountInLineChanged != null)
            {
                ChipCountInLineChanged(this, EventArgs.Empty);
            }
        }
    }

    private int _linesCount;
    public int LinesCount
    {
        get { return _linesCount; }
        set
        {
			if (_linesCount == value)
				return;

            _linesCount = value;
            if (LinesCountChanged != null)
            {
                LinesCountChanged(this, EventArgs.Empty);
            }
        }
    }

    public bool IsFinalAnimationEnabled { get; set; }

    public int FinalAnimationDurationSec { get; set; }

	public int TapSoundType { get; set; }
  
    private void OnSaveSettings()
    {
        var store = Store.Instance;

        store.SetValue("LinesCount", this._linesCount.ToString());
        store.SetValue("ChipCountInLine", this._chipCountInLine.ToString());
        store.SetValue("FinalAnimationDuration", this.FinalAnimationDurationSec.ToString());
        store.SetValue("IsFinalAnimationEnabled", this.IsFinalAnimationEnabled.ToString());
		store.SetValue("TapSoundType", this.TapSoundType.ToString());

        store.SetValue("ChipColor", this._chipColor.ToString());
        store.SetValue("ChipShape", this._chipShape.ToString());
        store.SetValue("GameStyle", this._gameStyle.ToString());
        store.SetValue("WinEffect", this._winEffect.ToString());

        store.Flush();	
   	}

    private void OnLoadSettings()
    {
        var store = Store.Instance;        

        this._linesCount = store.GetIntValue("LinesCount", null, 2);
        this._chipCountInLine = store.GetIntValue("ChipCountInLine", null, 3);
        this.FinalAnimationDurationSec = store.GetIntValue("FinalAnimationDuration", null, 10);
		this.TapSoundType = store.GetIntValue("TapSoundType", null, 0);

        this._chipColor = store.GetEnumValue<ChipColors>("ChipColor", null, ChipColors.Blue);
        this._chipShape = store.GetEnumValue<ChipShapes>("ChipShape", null, ChipShapes.Circle);
        this._gameStyle = store.GetEnumValue<GameStyles>("GameStyle", null, GameStyles.Boy);
        this._winEffect = store.GetEnumValue<WinEffects>("WinEffect", null, WinEffects.Firework);        

        const bool kIsFinalAnimationEnabled = true;
        try
        {
            this.IsFinalAnimationEnabled = kIsFinalAnimationEnabled;
            this.IsFinalAnimationEnabled = Boolean.Parse(store.GetValue("IsFinalAnimationEnabled", null, kIsFinalAnimationEnabled.ToString()));
        }
        catch (FormatException e)
        {
            Debug.LogError(e.Message);
        }               
    }
    
    private void Awake()
    {
        Instance = this;
        this.OnLoadSettings();
    }
}

