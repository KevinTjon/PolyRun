using UnityEngine;

public class Triangle : Shape
{
    // Add triangle-specific properties and methods here

    protected override void Update()
    {
        base.Update();
        // Add triangle-specific update logic here
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // Add triangle-specific fixed update logic here
    }

    // Add other triangle-specific methods as needed
}