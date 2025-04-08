using Godot;
using System;

public partial class Slime : EnemyUnitBase
{
    new int speed = 50;
    public override int updateToChildSpeed(){
        return speed;
    }
/*
    public override void initialize(int level){
        this.level = level;
        speed = speed*level;
        speed += GD.RandRange(-20,20); 
        currentHealt = maxHealt*level;
        healtBar.initializeHealthBar(maxHealt*level);
        target = gameManager.PlayerBasePosition;
        directionToObjective = GlobalPosition.DirectionTo(target);
    }
    public override void move(float delta){
        moveWithSpeed(delta,speed);
    }
    */
    public override void UpdateDashJump(){
        if(direction){
            dashJump = new Vector2(speed,-speed);
        }
        else{
            dashJump = new Vector2(-speed,-speed);
        }
        
    }

}
