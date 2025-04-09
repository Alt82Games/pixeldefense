using Godot;
using System;

public partial class EnemyFlyingUnitBase : EnemyUnitBase
{
    int childSpeed = 100;
    public override int updateToChildSpeed(){
        return childSpeed;
    }

    public override void move(float delta){
        if(!isDead){
            if(GlobalPosition.DistanceTo(target) > 5){
                isOnFloor = floorDetector.IsColliding();
                if(!isOnFloor){
                    velocity = directionToObjective.Normalized() * speed;
                    this.GlobalPosition += velocity*delta;
                }
                else{
                    velocity = directionToObjective.Normalized() * speed ;
                    this.GlobalPosition += velocity*delta;
                }

                directionToObjective = GlobalPosition.DirectionTo(target);
            }
            else{
                dead();
            }
        }
        else{
            dieMovement(delta);
        }
    }
    
}
