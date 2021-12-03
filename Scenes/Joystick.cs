using Godot;
using System;

public class Joystick : TouchScreenButton
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    [Signal]
    public delegate void JumpOnTouch();

    private Vector2 _vectorNormalPosition;
    private float _boundary = 100.0F;

    private int _ongoingDrag = -1;

    private int _returnAccel = 20;
    private int _threshold = 20;

    public override void _Ready()
    {
        var vector = this.Normal.GetSize();
        _vectorNormalPosition = new Vector2(vector.x / 2, vector.y / 2);

    }

    public override void _Process(float delta)
    {
        if (_ongoingDrag == -1)
        {
            var posDiff = (new Vector2(0, 0) - _vectorNormalPosition) - Position;
            Position += posDiff * _returnAccel * delta;
        }

    }
    public override void _Input(InputEvent inputEvent)
    {


        if (inputEvent is InputEventScreenDrag dragEvent)
        {

            CalculateMoveVectorAndEmitAsSignal(dragEvent.Position, dragEvent.Index);
        }
        if (inputEvent is InputEventScreenTouch touchEvent)
        {
            int index = touchEvent.Index;

            var currentViewPort = GetViewportRect();
            if (touchEvent.Position.x > currentViewPort.Size.x * 0.4)
            {
                EmitSignal(nameof(JumpOnTouch));
            }
            else
            {

                if (this.IsPressed())
                {
                    CalculateMoveVectorAndEmitAsSignal(touchEvent.Position, index);
                }
                else
                {
                    if (index == _ongoingDrag)
                    {
                        _ongoingDrag = -1;
                    }
                }
            }
        }

    }

    public void CalculateMoveVectorAndEmitAsSignal(Vector2 position, int index)
    {
      
        Sprite parent = (Sprite)GetParent();
        var eventDistanceFromCenter = (position - parent.GlobalPosition).Length();

        if (eventDistanceFromCenter <= _boundary * GlobalScale.x || index == _ongoingDrag)
        {
            GlobalPosition = position - _vectorNormalPosition * GlobalScale;
            if (GetButtonPosition().Length() > _boundary)
            {
                Position = GetButtonPosition().Normalized() * _boundary - _vectorNormalPosition;
            }
            _ongoingDrag = index;
        }
        // var textureCenter = this.Position + _vectorNormalPosition;
        // var vector = (position - textureCenter).Normalized();
        // EmitSignal(nameof(MoveVectorSignal),vector);
    }

    public Vector2 GetButtonPosition()
    {
        return Position + _vectorNormalPosition;
    }

    public Vector2 GetValue()
    {
        if (GetButtonPosition().Length() > _threshold)
        {
            return GetButtonPosition().Normalized();
        }
        return new Vector2(0, 0);

    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
