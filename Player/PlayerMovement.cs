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
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}
	
	public override void _PhysicsProcess(float delta)
	{
		if(IsOnFloor())
		{
			if(Input.IsActionPressed("jump"))
			{
				_animationPlayer.Play("Jump");
				_velocity = new Vector2(0,-2500.0f);
				
				// _animatedSprite.Animation = "run";
			}
			else if(Input.IsActionPressed("move_right"))
			{
				_velocity.x = _speed;
				_animationPlayer.Play("Run");
				// _animatedSprite.Animation = "run";
			}
			else if(Input.IsActionPressed("move_left"))
			{
				_velocity.x = -_speed;
				_animationPlayer.Play("Run");
				// _animatedSprite.Animation = "run";
			}		
			else 
			{
				_velocity.x = 0;
				_animationPlayer.Play("Idle");
				// _animatedSprite.Animation = "idle";
			}
			if(_velocity.x > 0)		
			{
//				ApplyScale(new Vector2(1,1));
			}
			else if(_velocity.x < 0 )
			{
				// ApplyScale(new Vector2(-1,1));
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

		
		_velocity.y += _gravity;

		_velocity = MoveAndSlide(_velocity,Vector2.Up);
		
	}

	public void _on_JoyStickCanvas_MoveVectorSignal(Vector2 moveVector)
	{
		_velocity = MoveAndSlide(moveVector*_speed,Vector2.Up);
	}
}
