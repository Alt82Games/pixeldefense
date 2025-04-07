using Godot;
using System;
using System.Linq;

public partial class Hud : CanvasLayer
{
    Label fps;
    Label points;
    Label projectiles;
    Timer updateLabelsTimer;
    GameManager gameManager;
    public override void _Ready(){
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");
        fps = GetNode<Label>("fps");
        fps.Text = Engine.GetFramesPerSecond().ToString();
        points = GetNode<Label>("points");
        updateLabelsTimer = GetNode<Timer>("updateLabelsTimer");
        updateLabelsTimer.Timeout += OnupdateLabelsTimerTimeout;
        projectiles = GetNode<Label>("projectiles");

    }
    public override void _PhysicsProcess(double delta)
    {
        fps.Text = Engine.GetFramesPerSecond().ToString();
        base._PhysicsProcess(delta);
    }

    public override void _ExitTree()
    {
        updateLabelsTimer.Timeout -= OnupdateLabelsTimerTimeout;
        base._ExitTree();
    }

    private void OnupdateLabelsTimerTimeout()
    {
        setLabels();
    }

    public void setLabels(){
        points.Text = "Enemy Count: " + gameManager.EnemyCount;
        projectiles.Text = "Projectile Count: " + gameManager.ProjectileCount;
    }

}
