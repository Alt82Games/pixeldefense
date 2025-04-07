using Godot;
using System;

public partial class HealtBar : TextureProgressBar
{
    public void initializeHealthBar(float maxHealt){
        this.MaxValue = maxHealt;
        this.Value = maxHealt;
        this.Step = maxHealt/100;
    }
    public void receiveDamage(float currentHealt){
        this.Value = currentHealt;
    }
}
