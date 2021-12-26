using Godot;
using System;

public class PlayerMovement : KinematicBody2D
{
    private int _xVelocity = 700;// What te heck is this unit? mm/s ??
    private int _yVelocity = -2500;
    private int _gravity = 100;

    private Vector2 _velocity = new Vector2(0, 0);

    private AnimationPlayer _animationPlayer;
    private Joystick _joystick;

    private bool _jump = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _joystick = GetNode<Joystick>("JoystickCanvasLayer/JoyStickSprite/JoyStickTouchScreenButton");
        var skeleton = GetNode<Skeleton2D>("Skeleton2D");
        skeleton.Scale = new Vector2(0.2f, 0.2f);

        _joystick.Connect("JumpOnTouch", this, nameof(_OnJumpTouch));
        if (!OS.HasTouchscreenUiHint())
        {
            ((Sprite)_joystick.GetParent()).Hide();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        // Process user input that impacts the movement
        ProcessInput();

        // Update the animation based on the current velocity and world position
        UpdateAnimation();

        // Apply gravity
        _velocity.y += _gravity;

        // Apply velocity to body
        _velocity = MoveAndSlide(_velocity, new Vector2(0, -1));
    }

    private void ProcessInput()
    {
        if (OS.HasTouchscreenUiHint())
        {
            if (_jump == true)
            {
                _jump = false;
                _velocity = new Vector2(_joystick.GetValue().x * _xVelocity, _yVelocity);
            }
            else
            {
                _velocity = new Vector2(_joystick.GetValue().x * _xVelocity, _velocity.y);
            }

            // Alternative shorter version. Not tested because I dont know how, but should work.
            //if (_jump)
            //    _velocity.y = _yVelocity;
            //_velocity.x = _joystick.GetValue().x * _xVelocity;
        }
        else
        {
            _velocity.x = 0;
            bool right = Input.IsActionPressed("move_right");
            bool left = Input.IsActionPressed("move_left");
            bool jump = Input.IsActionPressed("jump");

            if (jump && IsOnFloor())
                _velocity.y = _yVelocity;
            if (right)
            {
                
                _velocity.x += _xVelocity;
            }
            if (left)
            {
                
                _velocity.x -= _xVelocity;
            }
        }
    }

    private void UpdateAnimation()
    {
        if (IsOnFloor())
        {
            if (_velocity.x != 0)
                _animationPlayer.Play("Run");
            else
                _animationPlayer.Play("Idle");
        }
        else
        {
            _animationPlayer.Play("Jump");
        }
    }

    private void _OnJumpTouch()
    {
        if (IsOnFloor())
        {
            _jump = true;
        }
    }

    private bool _alreadyMet = false;
    public void _On_Area2D_body_entered(Area2D area)
    {
        if (_alreadyMet == false)
        {
            AnimationPlayer worldPlayer = GetNode<AnimationPlayer>("../AnimationPlayer");
            worldPlayer.Play("MeetPedolino");
            _alreadyMet = true;
            var dialog = DialogicSharp.Start("FirstMeetPedolino");
            GetParent().AddChild(dialog);
        }

    }
}

