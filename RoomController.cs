using Godot;
using System;

public partial class RoomController : Node
{
	public Node2D TopLeft { get; private set; }
	public Node2D BottomRight { get; private set; }
	public override void _Ready()
	{
		TopLeft = GetNode<Node2D>("Area2DLevel/TopLeft");
		BottomRight = GetNode<Node2D>("Area2DLevel/BottomRight");
	}
}
