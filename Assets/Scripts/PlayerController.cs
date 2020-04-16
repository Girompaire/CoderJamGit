using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class formeRoue
    {
        public GameObject shapeRoue;
        public GameObject prefabShape;
    }

    [Header("Input")]
    public KeyCode left;
    public KeyCode right;
    public KeyCode down;
    public KeyCode up;
    public KeyCode jump;
    public KeyCode menu;

    public KeyCode powerErase;
    public KeyCode powerFreeze;
    public KeyCode Power03;


    [Header("Player Variables")]
    public int id;
    public float speed = 5f;
    [Range(1, 100)]
    public float jumpVelocity;
    public float myGravity;
    public float countJump = 5.0f;

    [Header("menu")]
    public GameObject roue;
    public GameObject cible;
    public formeRoue[] roueShape;

    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;
    private Vector2 velocityStock;
    private Vector3 firstPosition;
    private Vector3 firstScale;
    private Color colorPlayer;


    private bool isGravity = true;
    private bool isJumping;
    private bool isDone;
    private bool isGrounded;
    public bool canFreeze;

    private List<GameObject> GroundColliders;
    private float initialCount;
    private int currentShape = 1;
    private Dictionary<int, KeyCode[]> dicoKeys;
    private float freezeCooldown = 10f;
    private float freezeDuration= 2f;

    private RaycastHit2D hit;

    [HideInInspector]
    public bool stop;


    private void Start()
    {

    }

    private void Awake()
    {
        GroundColliders = new List<GameObject>();
        _rigidbody = GetComponent<Rigidbody2D>();
        initialCount = countJump;
        firstPosition = transform.position;
        firstScale = transform.localScale;
        dicoKeys = new Dictionary<int, KeyCode[]>();
        dicoKeys.Add(0,new KeyCode[] { right, down });
        dicoKeys.Add(1,new KeyCode[] { right, left });
        dicoKeys.Add(2,new KeyCode[] { down, left });
        dicoKeys.Add(3,new KeyCode[] { down, up });
        dicoKeys.Add(4,new KeyCode[] { left, up });
        dicoKeys.Add(5,new KeyCode[] { left, right });
        dicoKeys.Add(6,new KeyCode[] { up, right });
        dicoKeys.Add(7,new KeyCode[] { up, down });

        colorPlayer = roueShape[0].shapeRoue.GetComponent<SpriteRenderer>().color;
        cible.GetComponent<SpriteRenderer>().color = colorPlayer;
        roueShape[currentShape].shapeRoue.transform.localScale *= 1.2f;
        roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color= new Color(roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.r, roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.g, roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.b,1);

        canFreeze = true;
    }


    void Update()
    {
        if (!stop)
        {
            if (isGravity)
            {
                Move();

                if (Input.GetKeyDown(jump))
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
            else
            {
                cible.GetComponent<SpriteRenderer>().sprite = roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().sprite;
                CheckInput();
            }

            if (Input.GetKeyDown(menu))
            {
                Choose();
            }
            if (Input.GetKeyUp(menu))
            {
                Release();
            }
            if (Input.GetKeyDown(powerErase))
            {
                //hit = Physics2D.Raycast(transform.position + new Vector3(1, 0, 0), transform.right);
                erase();
            }
            if (Input.GetKeyDown(powerFreeze) & canFreeze)
            {
                canFreeze = false;
                freezeOtherPlayer();
            }
            if (transform.position.y < -5)
            {
                Death();
            }
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }

    }


    private void erase()
    {
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Ground" && hit.collider.gameObject.layer != 5) 
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
    private void freezeOtherPlayer()
    {
        GameObject[] otherPlayer;
        otherPlayer = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject item in otherPlayer)
        {
            if(item.GetComponent<PlayerController>().id != this.gameObject.GetComponent<PlayerController>().id)
            {
                StartCoroutine(freezeActionCooldown());
                StartCoroutine(freezeTimer(item));
            }
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
        cible.SetActive(true);
        roue.SetActive(true);
    }

    private void Release()
    {
        _rigidbody.velocity = velocityStock;
        isGravity = true;
        cible.SetActive(false);
        roue.SetActive(false);
        GameObject forme = Instantiate(roueShape[currentShape].prefabShape, cible.transform.position, Quaternion.identity);
        forme.GetComponent<SpriteRenderer>().color = colorPlayer;
    }

    private void Death()
    {
        _rigidbody.velocity *= 0;
        isGravity = true;
        cible.SetActive(false);
        roue.SetActive(false);
        transform.position = firstPosition;
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(dicoKeys[currentShape][0]))
        {
            if (isDone)
            {
                isDone = false;
            }
            roueShape[currentShape].shapeRoue.transform.localScale *= 1 / 1.2f;
            roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color = new Color(roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.r, roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.g, roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.b, 199.0f / 255.0f);
            if (currentShape != roueShape.Length - 1)
            {
                currentShape += 1;
            }
            else
            {
                currentShape = 0;
            }
            roueShape[currentShape].shapeRoue.transform.localScale *= 1.2f;
            roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color = new Color(roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.r, roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.g, roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.b, 1);
        }

        if (Input.GetKeyDown(dicoKeys[currentShape][1]))
        {
            if (isDone)
            {
                isDone = false;
            }
            roueShape[currentShape].shapeRoue.transform.localScale *= 1 / 1.2f;
            roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color = new Color(roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.r, roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.g, roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.b, 199.0f / 255.0f);
            if (currentShape != 0)
            {
                currentShape -= 1;
            }
            else
            {
                currentShape = roueShape.Length - 1;
            }
            roueShape[currentShape].shapeRoue.transform.localScale *= 1.2f;
            roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color = new Color(roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.r, roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.g, roueShape[currentShape].shapeRoue.GetComponent<SpriteRenderer>().color.b, 1);
        }
    }

    private void Move()
    {
        float moveDirX = 0;
        if (Input.GetKey(left) && !Input.GetKey(right))
        {
            moveDirX = -1;
            transform.localScale = new Vector3(-firstScale.x, firstScale.y, firstScale.z);
            cible.transform.localScale = new Vector3(-1, 1, 1);
            hit = Physics2D.Raycast(transform.position + new Vector3(-1, 0, 0), transform.right,5);
        }
        else if (Input.GetKey(right) && !Input.GetKey(left))
        {
            moveDirX = 1;
            transform.localScale = firstScale;
            cible.transform.localScale = new Vector3(1, 1, 1);
            hit = Physics2D.Raycast(transform.position + new Vector3(1, 0, 0), transform.right,5);
            
        }
        Vector2 moveDir = new Vector2(moveDirX, 0);

        _rigidbody.velocity = new Vector2(moveDir.normalized.x * speed, _rigidbody.velocity.y);
    }

    private IEnumerator freezeActionCooldown()
    {
       float  elapsed = 0f;
       while(elapsed < freezeCooldown)
       {
           yield return null;
           elapsed += Time.deltaTime;
       }
        canFreeze = true;
    }

    private IEnumerator freezeTimer(GameObject cible)
    {
        cible.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        float elapsed = 0f;
        while (elapsed < freezeDuration)
        {
            yield return null;
            elapsed += Time.deltaTime;
        }
        cible.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }

}
