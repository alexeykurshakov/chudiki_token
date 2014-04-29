using UnityEngine;
using System.Collections;

public class Chip : MonoBehaviour {

	public enum States
	{
		NotSelected,
		Normal,
		Dark,
	}

	[SerializeField] private tk2dSprite _sprite;

    [SerializeField] private tk2dCameraAnchor _cameraAnchor;

    private States _state;
    public States State
    {
        get { return _state; }
        set
        {
            _state = value;

            switch (value)
            {
                case States.Dark:
                    this._sprite.color = new Color(180f / 255f, 180f / 255f, 180f / 255f);
                    break;

                default:
                    this._sprite.color = new Color(1f, 1f, 1f);                    
                    break;
            }

			this.OnSpriteStyleApply(null, System.EventArgs.Empty);
        }
    }

	public static Chip Create(GameObject chipPrefab, Vector2 offset, States defaultState = States.NotSelected)
	{
	    var obj = (GameObject)Object.Instantiate(chipPrefab);
        var chip = obj.GetComponent<Chip>();	    
        chip._cameraAnchor.AnchorOffsetPixels = offset;
	    chip.State = defaultState;
		return chip;
	}

	private void OnDestroy() 
	{
		var gameSettings = Settings.Instance;
		gameSettings.ChipShapeChanged -= this.OnSpriteStyleApply;
		gameSettings.ChipColorChanged -= this.OnSpriteStyleApply;	    
	}

	private void OnSpriteStyleApply(System.Object sender, System.EventArgs args)
    {
        var gameSettings = Settings.Instance;

        var sColor = this.State == States.NotSelected ? "Inactive" : gameSettings.ChipColor.ToString();
        var sShape = gameSettings.ChipShape.ToString();

		this._sprite.SetSprite(string.Format("{0}_{1}", sShape, sColor));       
    }
			
	private void Start ()
	{
	    var gameSettings = Settings.Instance;

	    gameSettings.ChipShapeChanged += this.OnSpriteStyleApply;
        gameSettings.ChipColorChanged += this.OnSpriteStyleApply;	    

		this.OnSpriteStyleApply(null, System.EventArgs.Empty);
	}		
}
