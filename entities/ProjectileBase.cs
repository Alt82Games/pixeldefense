using Godot;
using System;

public partial class ProjectileBase : Area2D
{
    //Variables and constants---------------------------------------------
    protected int speed = 250;
    [Export]protected float projectileDamage = 75;

    //TODO: Change pierce when the upgrade system is on progress, get it from GameManager
    protected int enemiesToPierce = 0;
    protected bool isUsed = false;
    float stepsPredicted;

    Vector2 gravity = new Vector2(0,98f);
	Vector2 velocity = new Vector2(100,0); //TODO: Make it for the two directions
    Vector2 targetDirection = new Vector2(1,0);
    Vector2 dashJump = Vector2.Zero;
    protected Vector2 directionToObjective = Vector2.Zero;
    protected EnemyUnitBase target;
    Vector2 targetInitialPosition,lastPosition;
    Vector2 targetPredictedPosition,targetPredictedPositionTuned;
    double lastDelta = 0;
    uint tickOffset;
        
    //Node references-----------------------------------------------------
    GameManager gameManager;
    VisibleOnScreenNotifier2D onScreen;
    Timer despawnTimer;
    
    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {

        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");

        this.AreaEntered += OnAreaEntered;

        despawnTimer = GetNode<Timer>("despawnTimer");
        despawnTimer.Timeout += OnDespawnTimerTimeout;

        onScreen = GetNode<VisibleOnScreenNotifier2D>("onScreen");
        onScreen.ScreenEntered += OnScreenNotifierEntered;
        onScreen.ScreenExited  += OnScreenNotifierExited;

        tickOffset = GD.Randi() % 60;

        updateToChildData();

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
        this.AreaEntered        -= OnAreaEntered;
        onScreen.ScreenEntered  -= OnScreenNotifierEntered;
        onScreen.ScreenExited   -= OnScreenNotifierExited;
        despawnTimer.Timeout    -= OnDespawnTimerTimeout;
        base._ExitTree();
    }



    //Signal functions----------------------------------------------------


    private void OnScreenNotifierEntered()
    {
        Visible = true;
    }
    private void OnScreenNotifierExited()
    {
        Visible = false;
    }
    private void OnDespawnTimerTimeout()
    {
        if(!isUsed){
            clearProjectile();
        }

        
    }
    private void OnAreaEntered(Area2D area)
    {
        if(area.IsInGroup("Enemy")){
            
            hitTarget((EnemyUnitBase)area);
        }
    }

    //Custom functions----------------------------------------------------<
    protected virtual void move(float delta)
    {
     GlobalPosition += directionToObjective * speed*delta;   
    }

    public void clearProjectile(){
        //Visible = false;
        SetCollisionMaskValue(2,false);
        gameManager.ProjectileCount -= 1;
        gameManager.queueFreeList.Add(this);
    }

    public Vector2 calculateInterceptionPoint(float dist){
        float distance1 = dist;
        float steps = distance1/speed;
        
        targetPredictedPosition = targetInitialPosition + target.DirectionToObjective*(target.Speed*steps);
        
        float distance2 = this.GlobalPosition.DistanceTo(targetPredictedPosition);
        float steps2 = distance2/speed;
        
        targetPredictedPositionTuned = targetInitialPosition + target.DirectionToObjective*(target.Speed*steps2);        
        if(targetPredictedPosition.DistanceTo(targetPredictedPositionTuned) > 0.01){
            return calculateInterceptionPoint(distance2);
        }
        else{
            stepsPredicted = steps2;
            return targetPredictedPositionTuned;
        }
    }

    public virtual void initialize(EnemyUnitBase obje,Vector2 GlobalPosition,float projectileDamageMultiplier){
        projectileDamage *= projectileDamageMultiplier;
        this.GlobalPosition = GlobalPosition;
        if(obje != null){
            target = obje;
            targetInitialPosition = target.GlobalPosition;
            Vector2 targetPredictedPositionLocal = calculateInterceptionPoint(this.GlobalPosition.DistanceTo(target.GlobalPosition));

            directionToObjective = GlobalPosition.DirectionTo(targetPredictedPositionLocal);
            
        }
        
    }

    public virtual void updateToChildData(){
        this.speed = speed;
        this.projectileDamage = projectileDamage;
    }

    public virtual void hitTarget(EnemyUnitBase area){
            area.reciveDamage(projectileDamage);
            enemiesToPierce --;
            if(enemiesToPierce < 0 && !isUsed){
                isUsed = true;
                clearProjectile();
            }
    }
    public EnemyUnitBase Target{get{return target;} set{target = value;}}
    public Vector2 DirectionToObjective{get{return directionToObjective;}}
}
