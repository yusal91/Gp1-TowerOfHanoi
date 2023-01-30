using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // codes from Fredrik
    
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private CapsuleCollider _collider;
    [SerializeField]
    private AnimationCurve _jumpPowerCurve;
    [SerializeField]
    private float _speed = 1000f;
    [SerializeField]
    private float _rotationSpeed = 720f;

    private Locomotion _locomotion = new Locomotion();
    private float _minJumpTimer;
    [SerializeField]
    private Animator _anim;

    //float Run;
    //bool  Jump;



    // Testing new codes above 




    //public float speed;                      // veriable for movementspeed
    //public float rotationSpeed;              // for movement rotation and movementspeed
    //public float jumpForce = 2f;            // jump & add force on jump
    //public bool isGrounded;                // ground Check
    //private Vector3 direction;             // Direction for movement

    //Rigidbody rb;


    //Added to make turning smoother
    // public float turnSmoothTime = 0.1f;
    // float turnSmoothVelocity;

    public Transform cam;

    // code from Fredrik
    private void Awake()
    {
        _locomotion.Setup(transform, _collider, _jumpPowerCurve, _rotationSpeed, _speed, _rigidbody);
    }
    // code from Fredrik



    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        //  if (direction.magnitude >= 0.1f)
        //  {
        //      //Turns character in direction of movement
        //      //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //      float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        //      transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //
        //      Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;   //get the input direction
        //      moveDir = moveDir.normalized * _speed * Time.fixedDeltaTime;
        //      moveDir.y = rb.velocity.y;
        //
        //      rb.velocity = moveDir;
        //  }
        //  else
        //  {
        //      rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        //  }

        // Fredriks Code

        Vector3 camforward = cam.forward;
        Vector3 camright = cam.right;
            camright.y = 0f;
            camforward.y = 0f;

            //get the input direction
            Vector3 targetDirection = Vector3.zero;
            targetDirection += Input.GetAxis("Vertical") * camforward;
            targetDirection += Input.GetAxis("Horizontal") * camright;

            _anim.SetFloat("run", targetDirection.magnitude);                /// code for animator

            _locomotion.Move(targetDirection.normalized); //<-- normalize it since we use the debug axis, (wont be needed for touchpad)
        
            

        // tempory testing codes from Fredrik
    }


    private void Update()
    {
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        //Changed name of movementDirection to direction -Olle
        //direction = new Vector3(horizontalInput, 0f, verticalInput);
        //direction.Normalize();



        // Cdoes from Fredrik
        _locomotion.Tick(Time.deltaTime);

        if (_locomotion.IsGrounded && Input.GetKeyDown(KeyCode.Space))     //<-- jump down button check
        {
                              // code for animator  jump
            _locomotion.IsJumping = true;
            _minJumpTimer = 0.1f;
        }
        
        if (_locomotion.IsJumping == false)
        {
            _anim.SetBool("jump", false);   // code for animator Jump
        }
        if (_locomotion.IsJumping == true)
        {
            _anim.SetBool("jump", true);    // code for animator Jump
        }

        if (_minJumpTimer > 0)
            _minJumpTimer -= Time.deltaTime;

        if (_locomotion.IsJumping && (_minJumpTimer > 0f || Input.GetKey(KeyCode.Space)))        //<-- jump is beeing held
        {
            _locomotion.NormalizedJump += Time.deltaTime * 5f;
            
        }
        else
        {
            _locomotion.IsJumping = false;
            _locomotion.NormalizedJump = 0f;
        }

    }


        /*if (_collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }*/
    // code from Fredrik



    // if (Input.GetKeyDown(KeyCode.Space) && isGrounded)     // move on void update
    // {
    //   rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //  isGrounded = false;
    //}

}
    // private void OnCollisionEnter(Collision collision)

    // {
    //if (collision.gameObject.CompareTag("Ground"))
    // {
    // isGrounded = true;
    //}
    // }
    //} 

