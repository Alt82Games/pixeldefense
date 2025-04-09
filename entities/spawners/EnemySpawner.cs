using Godot;
using System;
using System.Linq;

public partial class EnemySpawner : Area2D
{
    //Variables and constants---------------------------------------------
    [Export] int enemyToSpawn = 0;
    int indexToSpawn = 0;
    static int MAX_ENEMY_COUNT = 250;
    static PackedScene [] scenes = [GD.Load<PackedScene>("res://entities/slime.tscn"),
                                    GD.Load<PackedScene>("res://entities/bat.tscn"),];
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
        if(enemyToSpawn == 0 || enemyToSpawn > scenes.Count()){
            indexToSpawn = GD.RandRange(0,scenes.Count()-1);
        }
        else{
            indexToSpawn = enemyToSpawn -1;
        }
        EnemyUnitBase instance = (EnemyUnitBase)scenes[indexToSpawn].Instantiate();
        instance.GlobalPosition = this.GlobalPosition + new Vector2(0,GD.RandRange(-50,50));
        AddSibling(instance);
        gameManager.EnemyCount += 1;
        instance.initialize(1);
    }
}
