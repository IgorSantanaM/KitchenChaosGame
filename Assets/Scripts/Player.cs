using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f;

    private Keyboard keyboard;
    private bool isWalking;

    private void Awake()
    {
        keyboard = Keyboard.current;
    }

    private void Update()
    {
        Vector2 inputVector = new Vector2(0, 0);
        if (keyboard.wKey.isPressed)
        {
            inputVector.y = +1;
        }

        if (keyboard.sKey.isPressed)
        {
            inputVector.y = -1;
        }

        if (keyboard.aKey.isPressed)
        {
            inputVector.x = -1;
        }

        if (keyboard.dKey.isPressed)
        {
            inputVector.x = +1;
        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y); 
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;
        float rotatetionSpeed = 10f;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotatetionSpeed);

        Debug.Log(inputVector);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
