using Godot;
using System;
using System.Linq;

public partial class Hud : CanvasLayer
{
    Label fps;
    Label points;
    Timer updatePointsTimer;
    GameManager gameManager;
    public override void _Ready(){
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");
        fps = GetNode<Label>("fps");
        fps.Text = Engine.GetFramesPerSecond().ToString();
        points = GetNode<Label>("points");
        updatePointsTimer = GetNode<Timer>("points/updatePointsTimer");
        updatePointsTimer.Timeout += OnUpdatePointsTimerTimeout;

    }
    public override void _PhysicsProcess(double delta)
    {
        fps.Text = Engine.GetFramesPerSecond().ToString();
        base._PhysicsProcess(delta);
    }

    public override void _ExitTree()
    {
        updatePointsTimer.Timeout -= OnUpdatePointsTimerTimeout;
        base._ExitTree();
    }

    private void OnUpdatePointsTimerTimeout()
    {
        setPoints();
    }

    public void setPoints(){
        points.Text = "Points: " + gameManager.CurrentPoints;
    }

}
