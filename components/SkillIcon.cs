using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SkillIcon : TextureProgressBar
{
    //Variables and constants---------------------------------------------}

    [Export] float waitTime;
    static int controlMapIndex = 0;
    String activationKey;
    bool isUsed = false;
    int skillIndex;
    List<String> keysInAction = new List<string>();
        
    //Node references-----------------------------------------------------
    Label activationLabel;
    Timer cdTimer;
    
    //Overrided functions-------------------------------------------------
    public override void _Ready()
    {

        
        skillIndex = GetIndex() + 1;
        activationKey = "Skill" + skillIndex.ToString();

        getActionList();

        activationLabel = GetNode<Label>("activationLabel");
        activationLabel.Text = keysInAction[controlMapIndex];

        cdTimer = GetNode<Timer>("cdTimer");
        cdTimer.WaitTime = waitTime;
        cdTimer.Timeout += OnCdTimerTimeout;
        MaxValue = waitTime;

        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(isUsed){
            Value = cdTimer.TimeLeft;
        }
        
        base._PhysicsProcess(delta);
    }


    public override void _ExitTree()
    {
        cdTimer.Timeout -= OnCdTimerTimeout;
        base._ExitTree();
    }
    //Signal functions----------------------------------------------------

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed(activationKey)){
            Value = waitTime;
            cdTimer.Start();
            isUsed = true;
        }
        base._Input(@event);
    }

    public void OnCdTimerTimeout(){
        Value = 0;
        isUsed = false;
    }

    //Custom functions----------------------------------------------------
    public void getActionList(){
        foreach(InputEventKey actionEvent in InputMap.ActionGetEvents(activationKey)){
            GD.Print(activationKey + ": " +actionEvent.PhysicalKeycode);
            keysInAction.Add(OS.GetKeycodeString(actionEvent.PhysicalKeycode));
        }
    }
}
