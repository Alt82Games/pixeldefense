using Godot;
using System;

public partial class EnemySpawner : Area2D
{
    //Variables and constants---------------------------------------------
    static int MAX_ENEMY_COUNT = 200;
    static PackedScene [] scenes = [GD.Load<PackedScene>("res://entities/enemy_unit_base.tscn")];
    //Node references-----------------------------------------------------
    GameManager gameManager;
    Timer spawnTimer;
    
    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");
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
        if(gameManager.EnemyCount <= MAX_ENEMY_COUNT){
            spawn();
        }
        
    }

    //Custom functions----------------------------------------------------
    public void spawn(){
        EnemyUnitBase instance = (EnemyUnitBase)scenes[0].Instantiate();
        instance.GlobalPosition = this.GlobalPosition + new Vector2(0,GD.RandRange(-50,50));
        AddSibling(instance);
        gameManager.EnemyCount += 1;
        instance.initialize(1);
    }
}
