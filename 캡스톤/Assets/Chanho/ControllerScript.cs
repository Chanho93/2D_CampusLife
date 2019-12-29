using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public JoyStick joystick;
    public float MoveSpeed;

    private Vector3 _moveVector;
    private Transform _transform;

    Animator _PlayerAnimator;
    Vector2 _PlayerVector;
    // Start is called before the first frame update
    void Start()
    {
        _PlayerAnimator = GetComponent<Animator>();

        _transform = transform;
        _moveVector = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        
    }

    private void FixedUpdate()
    {
        Move();
    }
    public void HandleInput()
    {
        _moveVector = poolInput();
    }

    public Vector3 poolInput()
    {
        float h = joystick.GetHorizontalValue();
        float v = joystick.GetVerticalValue();
        Vector3 moveDir = new Vector3(h, v, 0).normalized;

        return moveDir;
    }

    public void Move()
    {

        _transform.Translate(_moveVector * MoveSpeed * Time.deltaTime);
        _PlayerAnimator.SetFloat("DirX", _PlayerVector.x);
      _PlayerAnimator.SetFloat("DirY", _PlayerVector.y);
      _PlayerAnimator.SetBool("Walking", true);
    }
}
