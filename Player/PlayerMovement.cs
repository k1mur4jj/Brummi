using Godot;
using System;

public class PlayerMovement : KinematicBody2D
{
	private Vector2 _velocity = new Vector2(0,0);
	private int _maxSpeed = 600;
	private int _speed = 700;
	private int _gravity = 100;
	private int _jumpSpeed = 5;
	private AnimationPlayer _animationPlayer;
	private Joystick _joystick;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_joystick = GetNode<Joystick> ("JoystickCanvasLayer/JoyStickSprite/JoyStickTouchScreenButton");
		_joystick.Connect("JumpOnTouch",this,nameof(_OnJumpTouch));
		if(!OS.HasTouchscreenUiHint())
		{
			((Sprite)_joystick.GetParent()).Hide();
		}
		
	}

	private bool _jump = false;

	public void _OnJumpTouch() 
	{
		if(IsOnFloor())
		{
			_jump = true;
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{

		if(OS.HasTouchscreenUiHint())
		{
			if (_jump == true)
			{
				_jump = false;
				_velocity = new Vector2(_joystick.GetValue().x*_speed,-2500.0f);
				
			}
			else
			{
					_velocity = new Vector2(_joystick.GetValue().x*_speed,_velocity.y);
			}
			
		}
		else
		{
			if(IsOnFloor())
			{
			
				if(Input.IsActionPressed("jump"))
				{
					_animationPlayer.Play("Jump");
					_velocity = new Vector2(0,-2500.0f);
					
				}
				else if(Input.IsActionPressed("move_right"))
				{
					_velocity.x = _speed;
				}
				else if(Input.IsActionPressed("move_left"))
				{
					_velocity.x = -_speed;
				}		
				else 
				{
					_velocity.x = 0;
					_animationPlayer.Play("Idle");
				}
			
			}
			else
			{
				if (_velocity.y < 0)
				{
					_animationPlayer.Play("Jump");
				}
				if(Input.IsActionPressed("move_right"))
				{
					_velocity.x = _speed;
				}
				if(Input.IsActionPressed("move_left"))
				{
					_velocity.x = -_speed;				
				}		
			}

		}
		if(_velocity.x > 0)		
		{
			GD.Print("Right",_velocity);
			// ApplyScale(new Vector2(1,1));
		}
		else if(_velocity.x < 0 )
		{
			GD.Print("Left",_velocity);
			// ApplyScale(new Vector2(-1,1));
		}
	

		if(_velocity.x != 0 && IsOnFloor())
		{
			_animationPlayer.Play("Run");
			GD.Print("RUN");
		}
		else if( IsOnFloor())
		{
			_animationPlayer.Play("Idle");
		}
		
		_velocity.y += _gravity;
		_velocity = MoveAndSlide(_velocity,new Vector2(0,-1));
		
	}

}
