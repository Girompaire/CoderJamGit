using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;

    public float speed = 5f;

    [Range(1, 100)]
    public float jumpVelocity;
    public float myGravity;
    private Vector2 velocityStock;
    private bool isGravity = true;

    private bool isJumping;
    public float countJump = 5.0f;
    private float initialCount;


    private bool isGrounded;
    private List<GameObject> GroundColliders;

    public GameObject roue;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        initialCount = countJump;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGravity)
        {
            float moveDirX = Input.GetAxis("Horizontal");
            Vector2 moveDir = new Vector2(moveDirX, 0);

            _rigidbody.velocity = new Vector2(moveDir.normalized.x * speed, _rigidbody.velocity.y);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    Jump();
                }
            }

            if (isJumping)
            {
                countJump -= Time.deltaTime;
                if (countJump < 0)
                {
                    isJumping = false;
                    countJump = initialCount;
                }
            }
            else
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y - (myGravity * Time.deltaTime));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Choose();
        }
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            Release();
        }
    }

    private void Jump() // Jump de Van Halen
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpVelocity);
        isJumping = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            GroundColliders.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GroundColliders.Remove(collision.gameObject);
            if (GroundColliders.Count == 0)
            {
                isGrounded = false;
            }
        }
    }

    private void Choose() //Choose de Why Don't We
    {
        velocityStock = _rigidbody.velocity;
        _rigidbody.velocity *=0;
        isGravity = false;
        roue.SetActive(true);
    }

    private void Release()
    {
        _rigidbody.velocity = velocityStock;
        isGravity = true;
        roue.SetActive(false);
    }


}
