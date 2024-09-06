using UnityEngine;

public class Square : Shape
{
    [SerializeField] private float flipDuration = 0.2f;
    private bool isFlipping = false;
    private float flipTimer = 0f;
    private int flipDirection = 0;
    private int currentRotation = 0; // 0, 90, 180, or 270 degrees
    private Vector3 flipStartPosition;

    protected override void Update()
    {
        base.Update();

        if (!isFlipping && isGrounded && Mathf.Abs(horizontal) > movementThreshold)
        {
            StartFlip(horizontal > 0 ? 1 : -1);
        }

        if (isFlipping)
        {
            ContinueFlip();
        }
    }

    protected override void FixedUpdate()
    {
        if (!isFlipping)
        {
            base.FixedUpdate();
        }
    }

    private void StartFlip(int direction)
    {
        isFlipping = true;
        flipTimer = 0f;
        flipDirection = direction;
        flipStartPosition = transform.position;
    }

    private void ContinueFlip()
    {
        flipTimer += Time.deltaTime;
        float t = flipTimer / flipDuration;

        if (t >= 1f)
        {
            FinishFlip();
            return;
        }

        float angle = 90f * t;
        Vector3 pivotPoint = flipStartPosition + new Vector3(flipDirection * 0.32f, -0.32f, 0);
        Vector3 newPosition = pivotPoint + Quaternion.Euler(0, 0, -angle * flipDirection) * (flipStartPosition - pivotPoint);
        
        newPosition.y = transform.position.y;
        
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(0, 0, -angle * flipDirection);
    }

    private void FinishFlip()
    {
        isFlipping = false;
        currentRotation = (currentRotation - 90 * flipDirection + 360) % 360;
        float snapRotation = Mathf.Round(currentRotation / 90f) * 90f;
        transform.rotation = Quaternion.Euler(0, 0, snapRotation);
        
        Vector3 finalPosition = flipStartPosition + new Vector3(flipDirection * 0.64f, 0, 0);
        finalPosition.x = Mathf.Round(finalPosition.x / 0.64f) * 0.64f;
        finalPosition.y = transform.position.y;
        transform.position = finalPosition;

        moveDirection = transform.right * horizontal;
    }
}