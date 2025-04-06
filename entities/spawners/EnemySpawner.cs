using Godot;
using System;

public partial class EnemySpawner : Area2D
{
    //Variables and constants---------------------------------------------
    static PackedScene [] scenes = [GD.Load<PackedScene>("res://entities/enemy_unit_base.tscn")];
    //Node references-----------------------------------------------------
    Timer spawnTimer;
    
    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        spawnTimer = GetNode<Timer>("spawnTimer");
        spawnTimer.Timeout += OnSpawnTimerTimeout;
        base._Ready();
    }
    

    public override void _ExitTree()
    {
        spawnTimer.Timeout -= OnSpawnTimerTimeout;
        base._ExitTree();
    }
    //Signal functions----------------------------------------------------
    private void OnSpawnTimerTimeout(){
        spawn();
    }

    //Custom functions----------------------------------------------------
    public void spawn(){
        EnemyUnitBase instance = (EnemyUnitBase)scenes[0].Instantiate();
        instance.GlobalPosition = this.GlobalPosition;
        AddSibling(instance);
    }
}
