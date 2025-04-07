using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node2D
{
    //Variables and constants---------------------------------------------

    private int currentPoints = -1;
    private int baseClickDamage = 90;
    //private float baseProjectileDamage = 50;
    private float fireSpeedActual = 2;
    private int currentLevel = 0;
    private int currentHorde = 0;
    private int clicksPerClick = 1;
    private int clickDamageMultiplier = 1;
    private int criticalClickMultiplier = 2;
    private Vector2 playerBasePosition = Vector2.Zero;
    private int enemyCount = 0;
    private int projectileCount = 0;
    private static int skipFrames = 0;
    private uint tickOffset;

    //Node references-----------------------------------------------------

    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        queueFreeManager();
        if((((long)Engine.GetPhysicsFrames() + tickOffset) % (skipFrames + 1)) == 0){
            
        }
        base._PhysicsProcess(delta);
    }


    public override void _ExitTree()
    {
        base._ExitTree();
    }
    //Signal functions----------------------------------------------------

    //Custom functions----------------------------------------------------
    public int CurrentPoints            {get{return currentPoints;}             set{currentPoints = value;}}
    public int BaseClickDamage          {get{return baseClickDamage;}           set{baseClickDamage = value;}}
    //public float BaseProjectileDamage   {get{return baseProjectileDamage;}      set{baseProjectileDamage = value;}}
    public int CurrentLevel             {get{return currentLevel;}              set{currentLevel = value;}}
    public int CurrentHorde             {get{return currentHorde;}              set{currentHorde = value;}}
    public int ClicksPerClick           {get{return clicksPerClick;}            set{clicksPerClick = value;}}
    public int ClickDamageMultiplier    {get{return clickDamageMultiplier;}     set{clickDamageMultiplier = value;}}
    public int CriticalClickMultiplier  {get{return criticalClickMultiplier;}   set{criticalClickMultiplier = value;}}
    public float FireSpeedActual        {get{return fireSpeedActual;}           set{fireSpeedActual = value;}}
    public Vector2 PlayerBasePosition   {get{return playerBasePosition;}        set{playerBasePosition = value;}}
    public int EnemyCount               {get{return enemyCount;}                set{enemyCount = value;}}
    public int ProjectileCount          {get{return projectileCount;}           set{projectileCount = value;}}
    public int SkipFrames               {get{return skipFrames;}                set{skipFrames = value;}}

    public int ClickDamage              {get{return baseClickDamage*clickDamageMultiplier;}}

    //Handle queue free
    public List<Node2D> queueFreeList = new List<Node2D>();
    private void queueFreeManager(){
        if(queueFreeList.Any()){
            for (int i = 0; i<20; i++){
                if(queueFreeList.Any()){
                    queueFreeList[0].QueueFree();
                    queueFreeList.RemoveAt(0);
                }
                else{
                    break;
                }
            }
        }
        else{
            queueFreeList.Clear();
        }
    }

}

