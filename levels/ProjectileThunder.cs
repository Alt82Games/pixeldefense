using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

public partial class ProjectileThunder : ProjectileBase
{

    //Variables and constants---------------------------------------------
    [Export]new int speed = 1000;
    static int MAX_LINE_POINTS = 10;
    [Export]new float projectileDamage = 10;
    [Export]int jumps = 10;
    ulong instanceId;
    List<EnemyUnitBase> targetsHit = new List<EnemyUnitBase>();
    List<EnemyUnitBase> potentialTargets = new List<EnemyUnitBase>();
    Vector2 currentPoint, nextPoint, targetPosition, originalTarget;  
    //Node references-----------------------------------------------------
    Timer updateTargetPosition;
    Area2D jumpArea;
    Line2D chain;
    
    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        targetsHit.Clear();
        updateTargetPosition = GetNode<Timer>("updateTargetPosition");
        updateTargetPosition.Timeout += OnupdateTargetPositionTimeout;

        jumpArea = GetNode<Area2D>("jumpArea");

        chain = GetNode<Line2D>("chain");
        chain.ClearPoints();
        //chain.ToLocal(this.GlobalPosition);

        base._Ready();
    }

    public override void _ExitTree()
    {
        updateTargetPosition.Timeout -= OnupdateTargetPositionTimeout;
        base._ExitTree();
    }
    //Signal functions----------------------------------------------------

    public void OnupdateTargetPositionTimeout(){
        if(target.IsDead){
            //updateTargetPosition.Stop();
            //clearProjectile();
            getClosestTarget();
        }
        else{
            //chain.ClearPoints();
            targetPosition = target.GlobalPosition;
            
            directionToObjective = GlobalPosition.DirectionTo(targetPosition);
            
        }
        
    }

    //Custom functions----------------------------------------------------
    public override void updateToChildData(){
        this.speed = speed;
        this.projectileDamage = projectileDamage;
        this.enemiesToPierce = jumps;
    }
    protected override void move(float delta)
    {
     GlobalPosition += directionToObjective * speed*delta;
     chain.AddPoint(Position);
     if(chain.GetPointCount() > MAX_LINE_POINTS){
        chain.RemovePoint(0);
     }   
    }
    public override void initialize(EnemyUnitBase target, Vector2 GlobalPosition, float projectileDamageMultiplier){
        projectileDamage *= projectileDamageMultiplier;
        this.GlobalPosition = GlobalPosition;
        this.target = target;
        directionToObjective = GlobalPosition.DirectionTo(this.target.GlobalPosition);
        //targetsHit.Add(target);
        instanceId = target.GetInstanceId();
        currentPoint = Vector2.Zero;
        nextPoint = target.GlobalPosition-GlobalPosition;
        //chain.AddPoint(currentPoint);
        //drawLine(currentPoint, nextPoint , target);
        
    }

    public void getClosestTarget(){
        Godot.Collections.Array<Area2D> overlap = jumpArea.GetOverlappingAreas();
        potentialTargets.Clear();
        EnemyUnitBase newtarget = null;
        int cont = 0;
        foreach (Area2D area in overlap){
                if(area.IsInGroup("Enemy")){
                    if(targetsHit.Contains(area)){
                        //GD.Print("area already hit");
                    }
                    else{
                        potentialTargets.Add((EnemyUnitBase)area);
                    }
                    cont ++;
                    if (cont == 20){
                        break;
                    }
                }
            }
        if(potentialTargets.Any()){
            newtarget = potentialTargets[0];
            foreach(EnemyUnitBase area in potentialTargets){
                if(target != area){    
                    if(this.GlobalPosition.DistanceTo(newtarget.GlobalPosition) > this.GlobalPosition.DistanceTo(area.GlobalPosition)){
                        newtarget = area;
                    }
                }
            }
        }
        if(newtarget == null){
            //clearProjectile();
        }
        else{
            this.target = newtarget;
            instanceId = this.target.GetInstanceId();
        }
    }
    public override void hitTarget(EnemyUnitBase area){
            targetsHit.Add(area);
            area.reciveDamage(projectileDamage);
            enemiesToPierce --;
            if(enemiesToPierce < 0 && !isUsed){
                isUsed = true;
                clearProjectile();
            }
            getClosestTarget();
            
    }
    /*public void getNewTarget(EnemyUnitBase target){
        EnemyUnitBase curremtTarget = target;
        GD.Print(target);
        GD.Print(jumpArea.GetOverlappingAreas());
        foreach(EnemyUnitBase unit in jumpArea.GetOverlappingAreas()){
            GD.Print(unit.GetGroups());
           if(unit.IsInGroup("Enemy")){ potentialTargets.Add(unit);} 
        } 
        GD.Print(potentialTargets.Count());
        if(potentialTargets.Any()){
            GD.Print("CurrentTarget: " + curremtTarget + " Target: " + target);
            GD.Print("TargetList: " + targetPosition);
            int breakCounter = 0;
            while(curremtTarget == target && !targetsHit.Contains(target)){
                GD.Print(target);
                target = (EnemyUnitBase)potentialTargets[GD.RandRange(0,potentialTargets.Count()-1)];
                GD.Print(target);
                breakCounter++;
                if(breakCounter > 10){
                    target = curremtTarget;
                    break;
                }
            }
            
            jumps--;
            
            potentialTargets.Clear();
        }
    }
    */
}
