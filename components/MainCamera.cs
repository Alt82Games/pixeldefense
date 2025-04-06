using Godot;
using System;

public partial class MainCamera : Camera2D
{
    static int cameraSpeed = 5;
    int cameraTargetIndex = 0;
    Vector2 cameraUD = new Vector2(0,cameraSpeed);
    Vector2 cameraLR = new Vector2(cameraSpeed,0);
    Vector2 []zoom = {new Vector2(1,1), new Vector2(2,2)};
    Vector2 zoomStep = new Vector2(0.1f,0.1f);
    Vector2 maxZoom = new Vector2(3,3), minZoom = new Vector2(1f,1f);
    public override void _Ready()
    {
        base._Ready();
    }
    public override void _PhysicsProcess(double delta)
    {
        moveCamera();
        base._PhysicsProcess(delta);
    }

    private void moveCamera()
    {
        if(Input.IsActionPressed ("Up")){
            GlobalPosition -= cameraUD;
        }
        if(Input.IsActionPressed ("Down")){
            GlobalPosition += cameraUD;
        }
        if(Input.IsActionPressed ("Left")){
            GlobalPosition -= cameraLR;
            /*
            if(cameraTargetIndex -1 >= 0){
                cameraTargetIndex -= 1;
            }
            */
        }
        if(Input.IsActionPressed ("Right")){
            GlobalPosition += cameraLR;
            /*
            if(cameraTargetIndex +1 < zoom.Length){
                cameraTargetIndex += 1;
            }
            */
        }
        if(Input.IsActionJustReleased("ZoomIn")){
            GD.Print("x");
            if (Zoom + zoomStep < maxZoom){
                Zoom += zoomStep;
            }
            else{
                Zoom = maxZoom;
            }
        }
        if(Input.IsActionJustReleased("ZoomOut")){
            GD.Print("-x");
            if (Zoom - zoomStep > minZoom){
                Zoom -= zoomStep;
            }
            else{
                Zoom = minZoom;
            }
        }
    }
/*
    public void updateZoom(){
        Vector2 zoomVelocity = new Vector2(0.01f,0.01f);
        if(cameraTargetIndex == 0 && camera.Zoom < zoom[0]){
            camera.Zoom += zoomVelocity;
        }
        if(cameraTargetIndex == 1 && camera.Zoom > zoom[1]){
            camera.Zoom -= zoomVelocity;
        }
    }
*/
}
