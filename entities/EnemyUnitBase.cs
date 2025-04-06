using Godot;
using System;

public partial class EnemyUnitBase : Area2D
{
    //Variables and constants---------------------------------------------
    int speed = 100;
    bool isOnFloor = false;
    Vector2 gravity = new Vector2(0,98f);
	Vector2 velocity = new Vector2(100,0); //TODO: Make it for the two directions
    Vector2 targetDirection = new Vector2(1,0);
    Vector2 dashJump = Vector2.Zero;
    double lastDelta = 0;


    static int skipFrames = 0;
    uint tickOffset;
        
    //Node references-----------------------------------------------------
    GameManager gameManager;
    RayCast2D floorDetector;
    Timer dashJumpTimer;
    
    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {

        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");

        floorDetector = GetNode<RayCast2D>("floorDetector");
        isOnFloor = floorDetector.IsColliding();

        dashJumpTimer = GetNode<Timer>("dashJumpTimer");
        dashJumpTimer.Timeout += OnDashJumpTimerTimeout;



        tickOffset = GD.Randi() % 60;


        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        lastDelta+=delta;
        //TODO: Add frame skip to stop from too many updates and updates at the same time Engine.get_physics_frames() + tick_offset ) % ( skip_frames + 1 ) == 0
        if((((long)Engine.GetPhysicsFrames() + tickOffset) % (skipFrames + 1)) == 0){
            move((float)lastDelta);
            lastDelta = 0;
        }
        
        base._PhysicsProcess(delta);
    }

    

    public override void _ExitTree()
    {
        dashJumpTimer.Timeout -= OnDashJumpTimerTimeout;
        base._ExitTree();
    }

    //Signal functions----------------------------------------------------

    private void OnDashJumpTimerTimeout()
    {
        dashJump = new Vector2(0,-40);
    }

    //Custom functions----------------------------------------------------

    private void move(float delta)
    {
        isOnFloor = floorDetector.IsColliding();
        if(!isOnFloor){
            //TODO: Set terminal velocity for objects falling
            dashJump = Vector2.Zero;
            this.GlobalPosition += velocity*delta-gravity*delta;
		    velocity     += gravity*delta;
        }
        else{
            velocity = targetDirection * speed + dashJump;
            this.GlobalPosition += velocity*delta;
        }
        
    }
}
