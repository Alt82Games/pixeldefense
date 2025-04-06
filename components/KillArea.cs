using Godot;
using System;

public partial class KillArea : Area2D
{
    //Variables and constants---------------------------------------------
        
    //Node references-----------------------------------------------------
    GameManager gameManager;
    
    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");
        this.AreaEntered += OnAreaEntered;
        base._Ready();
    }
    

    public override void _ExitTree()
    {
        this.AreaEntered -= OnAreaEntered;
        base._ExitTree();
    }

    
    //Signal functions----------------------------------------------------

    private void OnAreaEntered(Area2D area)
    {
        gameManager.queueFreeList.Add(area);
    }

    //Custom functions----------------------------------------------------

}
