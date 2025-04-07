using Godot;
using System;

public partial class GeneralButton : Button
{
    [Export] int skillUpgradeNumber;
    GameManager gameManager;
    public override void _Ready()
    {
        gameManager = GetTree().Root.GetChild(0).GetNode<GameManager>("gameManager");
        base._Ready();
    }

    public override void _Pressed()
    {
        switch (skillUpgradeNumber)
        {
            case 1:
                gameManager.BaseClickDamage = gameManager.BaseClickDamage*2;
                GD.Print("Upgrade click a "+ gameManager.BaseClickDamage*2 + " de daño por click");
            break;
            case 2:
                //gameManager.BaseProjectileDamage = gameManager.BaseProjectileDamage*2.0f;
                //GD.Print("Upgrade projectil a "+ gameManager.BaseProjectileDamage*2.0f + " de daño");
            break;
            case 3:
                float firespeed = gameManager.FireSpeedActual*0.9f;
                if(firespeed < 0.01f){firespeed = 0.01f;}
                GD.Print("Upgrade turret a "+ firespeed + " segundos de enfriamiento");
                gameManager.FireSpeedActual = firespeed;
            break;
            case 4:
            break;
            case 5:
            break;
            case 6:
            break;
            case 7:
            break;
            case 8:
            break;
            case 9:
            break;
            case 10:
            break;
            default:
            break;
        }
        base._Pressed();
    }

}
