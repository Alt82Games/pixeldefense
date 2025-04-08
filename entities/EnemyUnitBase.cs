using Godot;
using System;

public partial class EnemyUnitBase : Area2D
{
    //Variables and constants---------------------------------------------
    protected float maxHealt = 100;
    protected float currentHealt = 100;
    protected int speed = 200;
    protected bool isOnFloor = false;

    protected bool direction = true;
    
    protected int level = 1;
    protected int basePointsGiven = 7;

    protected bool isDead = false;


    protected Vector2 gravity = new Vector2(0,98f);
    protected static Vector2 gravityDead = new Vector2(0,98f);
	protected Vector2 velocity; //TODO: Make it for the two directions
    //Vector2 targetDirection = new Vector2(1,0);
    protected Vector2 dashJump = Vector2.Zero;
    protected Vector2 directionToObjective = Vector2.Zero;
    protected Vector2 target,currentPosition,lastPosition;
    protected Vector2 initialVelocityDead = new Vector2(-200,-980);
    protected double lastDelta = 0;

    uint tickOffset;
        
    //Node references-----------------------------------------------------
    protected GameManager gameManager;
    protected RayCast2D floorDetector;
    protected Timer dashJumpTimer,deadTimer;
    protected HealtBar healtBar;
    protected VisibleOnScreenNotifier2D onScreen;
    protected Control clickArea;
    protected Sprite2D sprite;    
    
    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {

        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");

        floorDetector = GetNode<RayCast2D>("floorDetector");
        isOnFloor = floorDetector.IsColliding();

        dashJumpTimer = GetNode<Timer>("dashJumpTimer");
        dashJumpTimer.Timeout += OnDashJumpTimerTimeout;

        deadTimer = GetNode<Timer>("deadTimer");
        deadTimer.Timeout += OnDeadTimerTimeout;

        healtBar = GetNode<HealtBar>("healtBar");
        healtBar.initializeHealthBar(maxHealt);

        onScreen = GetNode<VisibleOnScreenNotifier2D>("onScreen");
        onScreen.ScreenEntered += OnScreenNotifierEntered;
        onScreen.ScreenExited  += OnScreenNotifierExited;

        clickArea = GetNode<Control>("clickArea");
        clickArea.GuiInput += OnClickAreaGuiInput;

        sprite = GetNode<Sprite2D>("sprite");

        tickOffset = GD.Randi() % 60;

        this.speed = updateToChildSpeed();
        if(GlobalPosition.X>target.X){
            initialVelocityDead.X *= -1;
            velocity = new Vector2(-100,0);
            direction = false;
            sprite.FlipH = direction;

        }
        else{
            velocity = new Vector2(100,0);
        }
        
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        if((((long)Engine.GetPhysicsFrames() + tickOffset) % (gameManager.SkipFrames + 1)) == 0){
            move((float)delta);

        }
        
        base._PhysicsProcess(delta);
    }

    

    public override void _ExitTree()
    {
        dashJumpTimer.Timeout   -= OnDashJumpTimerTimeout;
        deadTimer.Timeout       -= OnDeadTimerTimeout;
        onScreen.ScreenEntered  -= OnScreenNotifierEntered;
        onScreen.ScreenExited   -= OnScreenNotifierExited;
        clickArea.GuiInput      -= OnClickAreaGuiInput;
        base._ExitTree();
    }

    //Signal functions----------------------------------------------------

    private void OnClickAreaGuiInput(InputEvent @event)
    {
        if(@event.IsActionPressed("MouseClickLeft")){
            //TODO: Make a variable on game Manager to control the % of critical clicks
            if(GD.RandRange(0,100)<80){
                receiveClickDamage(false);
            }
            else{
                receiveClickDamage(true);
            }
            
        }
    }

    protected void OnDashJumpTimerTimeout()
    {
        UpdateDashJump();
    }
    private void OnScreenNotifierEntered()
    {
        Visible = true;
    }
    private void OnScreenNotifierExited()
    {
        Visible = false;
    }

    private void OnDeadTimerTimeout()
    {
        gameManager.EnemyCount -= 1;
        gameManager.queueFreeList.Add(this);
    }

    //Custom functions----------------------------------------------------
    public virtual void initialize(int level){
        this.level = level;
        speed = speed*level;
        speed += GD.RandRange(-20,20); 
        currentHealt = maxHealt*level;
        healtBar.initializeHealthBar(maxHealt*level);
        target = gameManager.PlayerBasePosition;
        directionToObjective = GlobalPosition.DirectionTo(target);
    }

    public virtual void move(float delta){
        moveWithSpeed(delta,speed);
    }
    public void moveWithSpeed(float delta, int speed)
    {
        if(!isDead){
            isOnFloor = floorDetector.IsColliding();
            if(!isOnFloor){
                //TODO: Set terminal velocity for objects falling
                dashJump = Vector2.Zero;
                this.GlobalPosition += velocity*delta-gravity*delta;
                if(velocity.Y < gravity.Y*2){
                    velocity     += gravity*delta;
                }
                
            }
            else{
                velocity = directionToObjective.Normalized().Round() * speed + dashJump;
                this.GlobalPosition += velocity*delta;
            }
        }
        else{
            this.GlobalPosition += initialVelocityDead*delta-gravityDead*delta;
		    initialVelocityDead += gravityDead;
            if(Modulate.A > 0.1f){
                Modulate -= new Color (0,0,0,1f*delta);
            }
            if(RotationDegrees < 350){
                RotationDegrees += 90*delta;
            }
            else{
                RotationDegrees = 0;
            }
        }
    }

    public void receiveClickDamage(bool critical){
        if(!isDead){
            float clickDamage = gameManager.ClickDamage;
            int numberOfClicks = gameManager.ClicksPerClick;
            if(critical){
                clickDamage *= gameManager.CriticalClickMultiplier;
            }
            for(int i = 0; i < numberOfClicks; i++){
                currentHealt -= clickDamage;
                healtBar.receiveDamage(currentHealt);
                //showDamageNumber(clickDamage);
                if(currentHealt <= 0){
                dead();             
                }
            }
        }
        //TODO: Add function that convert some of the normal clicks to critical clicks
        
        
    }

    public void reciveDamage(int damage){
        if(!isDead){
            currentHealt -= damage;
            healtBar.receiveDamage(currentHealt);
            //showDamageNumber(clickDamage);
            if(currentHealt <= 0){
                dead();                
            }
        }
    }

    public void dead(){
        isDead = true;
        deadTimer.Start();
        SetCollisionLayerValue(2,false);
        RemoveFromGroup("Enemy");
        clickArea.MouseFilter = (Control.MouseFilterEnum)2;
        velocity = Vector2.Zero;
        //CallDeferred("spawnPointCristal",[basePointsGiven, level]);
        gameManager.CurrentPoints += basePointsGiven*level;
         //TODO: Make it drop a coin or cristal to click or hover to pick up and gain the points instead
        
    }


    public virtual int updateToChildSpeed()
    {
        return speed;
    }
    
    public virtual void UpdateDashJump()
    {
        dashJump = new Vector2(0,0);
    }
    public Vector2 Target{get{return target;} set{target = value;}}
    public Vector2 CurrentPosition{get{return currentPosition;}}
    public Vector2 LastPosition{get{return lastPosition;}}
    public Vector2 DirectionToObjective{get{return directionToObjective;}}
    public int Speed{get{return speed;} set{speed = value;}}

}
