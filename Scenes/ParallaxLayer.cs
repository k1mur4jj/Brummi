using Godot;
using System;

public class ParallaxLayer : Godot.ParallaxLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private Camera2D _camera;
    [Export]
    private float _cloudSpeed=-15;
    private Vector2 _lastScreenSize;
    public override void _Ready()
    {
    }
    public override void _Process(float delta)
    {
        MotionOffset = new Vector2(MotionOffset.x+_cloudSpeed*delta,0);
        // var screen_size = GetViewportRect().Size * _camera.Zoom;
        // if (screen_size != _lastScreenSize)
        // {
        //     var sprite = (Sprite)GetChildren()[0];
        //     sprite.Scale = new Vector2(_spriteScale,_spriteScale);
        //     var spriteSize = sprite.GetRect().Size;
        //     float ceiled = Mathf.Ceil(screen_size.x / spriteSize.x/_spriteScale);
        //     var rect = new Rect2(0,0,spriteSize.x * ceiled, spriteSize.y);
        //     sprite.RegionRect = rect;
        //     sprite.RegionEnabled = true;
        //     MotionMirroring = new Vector2(rect.Size.x*_spriteScale,0);
        //     _lastScreenSize = screen_size;
        // }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
