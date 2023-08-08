using Godot;
using System;

public partial class GameController : Node
{
	[Export()] private CharacterBody2D _player;
	[Export()] private Camera2D _camera;
	
	private RoomController _currentRoom;
	private Node _rooms;
	private bool _isPaused;
	
	
	public override void _Ready()
	{
		base._Ready();
		_rooms = GetNode<Node>("Rooms");
		_currentRoom = GetCurrentRoom();
	}

	public override void _Process(double delta)
	{
		if (_isPaused)
		{
			if (IsCameraInsideRect(_currentRoom.TopLeft.GlobalPosition, _currentRoom.BottomRight.GlobalPosition, 10))
			{
				PauseLevel(false);
			}
		}
		else
		{
			RoomController playerRoom = GetCurrentRoom();
			if (playerRoom != null && playerRoom != _currentRoom)
			{
				_currentRoom = playerRoom;
				SetCameraLimitsToRoom(_currentRoom);
				PauseLevel(true);
			}
		}
	}
	private bool IsCameraInsideRect(Vector2 topLeft, Vector2 bottomRight, int margin)
	{
		Vector2 currentTopLeft = (_camera.GetScreenCenterPosition() - (_camera.GetViewportRect().Size / 2) / _camera.Zoom);
		Vector2 currentBottomRight = (_camera.GetScreenCenterPosition() + (_camera.GetViewportRect().Size / 2) / _camera.Zoom);

		bool topLeftIsOk = (currentTopLeft.X + margin) >= topLeft.X && (currentTopLeft.Y + margin) >= topLeft.Y;
		bool bottomRightIsOk = currentBottomRight.X <= (bottomRight.X + margin) && currentBottomRight.Y <= (bottomRight.Y + margin);

		return topLeftIsOk && bottomRightIsOk;
	}
	private void SetCameraLimitsToRoom(RoomController currentRoom)
	{
		Vector2 topLeft = currentRoom.TopLeft.GlobalPosition;
		Vector2 bottomRight = currentRoom.BottomRight.GlobalPosition;

		_camera.LimitTop = (int)topLeft.Y;
		_camera.LimitLeft = (int)topLeft.X;
		_camera.LimitBottom = (int)bottomRight.Y;
		_camera.LimitRight = (int)bottomRight.X;
	}
	private void PauseLevel(bool paused)
	{
		_isPaused = paused;
		foreach (Node child in GetChildren())
		{
			child.ProcessMode = paused ? ProcessModeEnum.Disabled : ProcessModeEnum.Inherit;
		}
	}
	private RoomController GetCurrentRoom()
	{
		foreach (Node node in _rooms.GetChildren())
		{
			var room = (RoomController)node;
			if (IsPlayerInRoom(room))
			{
				return room;
			}
		}
		return null;
	}
	private bool IsPlayerInRoom(RoomController room)
	{
		Area2D roomArea = room.GetNode<Area2D>("Area2DLevel");
		return roomArea.OverlapsBody(_player);
	}
	
	
}
