using Godot;
using System;

public partial class MainCamera : Camera2D
{
    static int cameraSpeed = 5;
    int cameraTargetIndex = 0;
    Vector2 cameraUD = new Vector2(0,cameraSpeed);
    Vector2 cameraLR = new Vector2(cameraSpeed,0);
    Vector2 []zoom = {new Vector2(0.5f,0.5f),new Vector2(1,1), new Vector2(2,2)};
    Vector2 zoomStep = new Vector2(0.1f,0.1f);
    Vector2 maxZoom = new Vector2(3,3), minZoom = new Vector2(0.5f,0.5f);
    Vector2 currentZoom = Vector2.Zero;
    public override void _Ready()
    {
        currentZoom = Zoom;
        base._Ready();
    }
    public override void _PhysicsProcess(double delta)
    {
        moveCamera((float)delta);
        base._PhysicsProcess(delta);
    }

    private void moveCamera(float delta)
    {
        if(Input.IsActionPressed ("Up")){
            if(GlobalPosition.Y - GetViewportRect().Size.Y/2 >= LimitTop)
                GlobalPosition -= cameraUD;
        }
        if(Input.IsActionPressed ("Down")){
            if(GlobalPosition.Y + GetViewportRect().Size.Y/2 <= LimitBottom)
            GlobalPosition += cameraUD;
        }
        if(Input.IsActionPressed ("Left")){
            if(GlobalPosition.X - GetViewportRect().Size.X/2 >= LimitLeft)
                GlobalPosition -= cameraLR;
            /*
            if(cameraTargetIndex -1 >= 0){
                cameraTargetIndex -= 1;
            }
            */
        }
        if(Input.IsActionPressed ("Right")){
            if(GlobalPosition.X + GetViewportRect().Size.X/2 <= LimitRight)
            GlobalPosition += cameraLR;
            /*
            if(cameraTargetIndex +1 < zoom.Length){
                cameraTargetIndex += 1;
            }
            */
        }
        if(Input.IsActionJustReleased("ZoomIn")){
            if (Zoom < maxZoom){
                Zoom += zoomStep;
                if(Zoom > maxZoom){
                    Zoom = maxZoom;
                }
            }
        }
            
        
        if(Input.IsActionJustReleased("ZoomOut")){
            if (Zoom > minZoom){
                Zoom -= zoomStep;
                if(Zoom < minZoom){
                    Zoom = minZoom;
                }
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
