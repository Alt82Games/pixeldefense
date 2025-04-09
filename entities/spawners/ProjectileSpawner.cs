using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ProjectileSpawner : Area2D
{
    //Variables and constants---------------------------------------------
    static int MAX_PROJECTILE_COUNT = 200;
    [Export] protected int projectileToSpawn = 2;
    [Export] protected float projectileDamageMultiplier = 1;
    protected int indexToSpawn = 0;
    List<Node2D> targets = new List<Node2D>();
    
    //Node references-----------------------------------------------------
    GameManager gameManager;
    Timer spawnTimer;
    static PackedScene [] scenes = [GD.Load<PackedScene>("res://entities/projectile_base.tscn"),
                                    GD.Load<PackedScene>("res://levels/projectile_thunder.tscn")];
    EnemyUnitBase target = null;
    
    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");
        spawnTimer = GetNode<Timer>("spawnTimer");
        spawnTimer.Timeout += OnSpawnTimerTimeout;
        AreaEntered += OnAreaEntered;
        base._Ready();
    }
    

    public override void _ExitTree()
    {
        spawnTimer.Timeout -= OnSpawnTimerTimeout;
         AreaEntered -= OnAreaEntered;
        base._ExitTree();
    }



    //Signal functions----------------------------------------------------

    private void OnSpawnTimerTimeout(){
        if(gameManager.ProjectileCount <= MAX_PROJECTILE_COUNT){
            spawn();
            target = null;
        }
        
    }
        private void OnAreaEntered(Area2D area)
    {
        /*
        if(area.IsInGroup("Enemy")){
            if(target == null){
                target = (EnemyUnitBase)area;
            }
        }
        */
    }

    //Custom functions----------------------------------------------------
    public void spawn(){
         if(projectileToSpawn == 0 || projectileToSpawn > scenes.Count()){
            indexToSpawn = GD.RandRange(0,scenes.Count()-1);
        }
        else{
            indexToSpawn = projectileToSpawn -1;
        }
        getClosestTarget();
        if(target != null){
            ProjectileBase instance = (ProjectileBase)scenes[indexToSpawn].Instantiate();
            AddSibling(instance);
            instance.GlobalPosition = this.GlobalPosition;
            gameManager.ProjectileCount += 1;
            //instance.initialize(target);
            instance.initialize(target, GlobalPosition, projectileDamageMultiplier);
        }
        
    }

    public void getClosestTarget(){
        Godot.Collections.Array<Area2D> overlap = GetOverlappingAreas();
        targets.Clear();
        int cont = 0;
        foreach (Area2D area in overlap){
                if(area.IsInGroup("Enemy")){
                    targets.Add(area);
                    cont ++;
                    if (cont == 20){
                        break;
                    }
                }
            }
        if(targets.Any()){
            EnemyUnitBase target = null;
            foreach(EnemyUnitBase area in targets){
                if(target == null){
                    target = area;
                }
                else{
                    //TODO: change to a tryCatch for the area in case is deleted
                    if(target == null){
                        break;
                    }
                    else{
                        if(this.GlobalPosition.DistanceTo(target.GlobalPosition) > this.GlobalPosition.DistanceTo(area.GlobalPosition)){
                            target = area;
                        }
                    }
                    
                }
            }
            this.target = target;
        }
    }
}
