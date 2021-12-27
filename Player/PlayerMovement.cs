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
        // skeleton.Scale = new Vector2(0.2f, 0.2f);

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
                // ApplyScale(new Vector2(-1,1));
            }
        }
    }

// func h_flip_children():
// 	for n in node.get_children():
// 		if not (n extends Node2D):
// 			continue
		
// 		if n extends CollisionShape2D or n extends CollisionObject2D:
// 			n.rotate(-2.0 * n.get_rot())
// 		else:
// 			n.scale(Vector2(-1, 1))
			
// 		var pos = n.get_pos()
// 		n.translate(Vector2(-2.0 * pos.x, 0.0))

    private void FlipChildrenHorizontal()
    {
        foreach(var n in GetChildren())
        {
            var type = n.GetType();
            if(type == typeof(Skeleton2D))
            {
                Skeleton2D skel = ((Skeleton2D)n);
                skel.ApplyScale(new Vector2(-1,1));
            }
            if (type != typeof(Node2D))
            {
                continue;
            }
            Node2D node = (Node2D)n;
            if(type == typeof(CollisionObject2D))
            {
                GD.Print(type.Name);
                CollisionObject2D colObj = ((CollisionObject2D)node);
                colObj.Rotate(-2.0f*colObj.Rotation);
            }             
            else if(type == typeof(CollisionShape2D))
            {
                CollisionShape2D colShape = ((CollisionShape2D)node);
                colShape.Rotate(-2.0f*colShape.Rotation);
            }
         
            else
            {
               node.ApplyScale(new Vector2(-1,1));
            }
            node.Translate(new Vector2(-2.0f*node.Position.x,0.0f));


        }
    }


    private bool _flipH = false;
    private void UpdateAnimation()
    {
        if (IsOnFloor())
        {
            if (_velocity.x < 0)
            {
                if(_flipH == false)
                {
                    FlipChildrenHorizontal();
                }
                _flipH = true;
                
                _animationPlayer.Play("Run");
            }
            else if (_velocity.x > 0)
            {

                if(_flipH == true)
                {
                    FlipChildrenHorizontal();
                }
                _flipH = false;
                _animationPlayer.Play("Run");
            }
            else
            {
                _animationPlayer.Play("Idle");
            }
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

