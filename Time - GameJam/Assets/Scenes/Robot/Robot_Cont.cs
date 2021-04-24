using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Robot_Cont : MonoBehaviour
{
    public float walkSpeed = 1;
    public float jumpSpeed = 1;

    Rigidbody2D rigid;
    public Animator animBody;
    public Animator animWheel;

    public AudioSource WalkSound;
    public AudioSource JumpSound;


    public GameObject dust;
    [SerializeField]
    bool isOnGround;
    [SerializeField]
    bool isCantMove;
    bool isJumping;

    [SerializeField]
    float jumpTimerCounter = 2;
    public float jumpTimer = 0.3f;
    void Start()
    {
        smokeVFX.SetActive(false);
        rigid = this.GetComponent<Rigidbody2D>();
        isCantMove = false;
        changeColor(1);
        startSFX();
        GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineVirtualCamera>().Follow = this.gameObject.transform;
    }

    public bool DoFacingLeft() {
        return isFacingLeft;
    }

    private void Update()
    {
        if (transform.localScale.x > 0)
        {
            isFacingLeft = true;
        }
        else {
            isFacingLeft = false;
        }

        if (Mathf.Abs(rigid.velocity.y) < 0.001f && IsGrounded())
        {
            //anim.SetBool("OnGround", true);
            jumpTimerCounter = jumpTimer;
            isOnGround = true;
        }
        else
        {
            //anim.SetBool("OnGround", false);
            isOnGround = false;
        }
    }

    public GameObject smokeVFX;
    public void setCantMove(bool trigger)
    {
        isCantMove = trigger;
       
    }

    public void showSmoke() {
        smokeVFX.SetActive(true);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isCantMove)
        {
            Movement();

            if (Input.GetButton("Jump") && isOnGround)
            {
                isJumping = true;
                Jump();
                dust.SetActive(true);
            }

            if (Input.GetButton("Jump") && isJumping == true)
            {
                if (jumpTimerCounter > 0)
                {
                    if (isOnGround)
                    {
                        dust.SetActive(true);
                        Jump();

                    }
                    rigid.velocity = Vector2.up * jumpSpeed;
                    jumpTimerCounter -= Time.deltaTime;
                }
                else
                {
                    //dust.SetActive(false);
                    isJumping = false;
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
               // dust.SetActive(false);
                isJumping = false;
            }

        }

    }

    public LayerMask groundLayer;
    [SerializeField]
    float distance = 0.15f;
    [SerializeField]
    float RayOffsetxL = 0.7f;
    [SerializeField]
    float RayOffsetxR = 0.7f;
    [SerializeField]
    float RayOffsetxM = 0.0f;

    float RayOffsety = -0.59f;

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        //float distance = 1.0f;

        Debug.DrawRay(position + new Vector2(-RayOffsetxL, RayOffsety), direction * distance, Color.green);
        Debug.DrawRay(position + new Vector2(RayOffsetxR, RayOffsety), direction * distance, Color.red);
        Debug.DrawRay(position + new Vector2(RayOffsetxM, RayOffsety), direction * distance, Color.yellow);

        RaycastHit2D hit = Physics2D.Raycast(position + new Vector2(-RayOffsetxL, RayOffsety), direction, distance, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(position + new Vector2(RayOffsetxR, RayOffsety), direction, distance, groundLayer);
        RaycastHit2D hit3 = Physics2D.Raycast(position + new Vector2(RayOffsetxM, RayOffsety), direction, distance, groundLayer);

        if (hit.collider != null || hit2.collider != null || hit3.collider != null)
        {
            return true;
        }



        return false;
    }

    bool isFacingLeft = true;

    void Movement()
    {

        float horizontalAxis = Input.GetAxis("Horizontal");

        if (horizontalAxis != 0)
        {
            if (WalkSound.enabled == true) {
                if (WalkSound.isPlaying == false)
                {

                    WalkSound.Play();
                }
            }
            
        }
        else {
            WalkSound.Stop();
        }

        if (horizontalAxis < 0)
        {
            if (isFacingLeft == true)
            {
                Vector3 newScale = this.transform.localScale;
                newScale.x *= -1;
                this.transform.localScale = newScale;
                isFacingLeft = false;
            }

        }
        else if (horizontalAxis > 0)
        {
            if (isFacingLeft == false)
            {
                Vector3 newScale = this.transform.localScale;
                newScale.x *= -1;
                this.transform.localScale = newScale;
                isFacingLeft = true;
            }
        }



        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Time.deltaTime * walkSpeed;
        animWheel.SetFloat("Movement", Mathf.Abs(horizontalAxis));
        animBody.SetFloat("Movement", Mathf.Abs(horizontalAxis));

        
      

    }

    void Jump()
    {

        //DustPlay();
       
        //anim.SetTrigger("Jump");
        JumpSound.Play();

        rigid.velocity = Vector2.up * jumpSpeed;



        //rigid.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);

    }

    public SpriteRenderer headSprite;
    public SpriteRenderer antennaSprite;
    public SpriteRenderer antenna2Sprite;

    public SpriteRenderer bodySprite;
    public SpriteRenderer wheel1Sprite;
    public SpriteRenderer wheel2Sprite;

    public void changeColor(float id)
    {
        float colorNum = id;
        Color clr= new Color(colorNum, colorNum, colorNum, 1);

        headSprite.color = clr;
        antennaSprite.color = clr;
        antenna2Sprite.color = clr;

        bodySprite.color = clr;
        wheel1Sprite.color = clr;
        wheel2Sprite.color = clr;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            CircleCollider2D[] wheelcollider = GetComponents<CircleCollider2D>();

            CircleCollider2D[] Objectwheelcollider = collision.gameObject.GetComponents<CircleCollider2D>();


            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<PolygonCollider2D>(), GetComponent<PolygonCollider2D>());
            Physics2D.IgnoreCollision(Objectwheelcollider[0], GetComponent<PolygonCollider2D>());
            Physics2D.IgnoreCollision(Objectwheelcollider[1], GetComponent<PolygonCollider2D>());

            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<PolygonCollider2D>(), wheelcollider[0]);
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<PolygonCollider2D>(), wheelcollider[1]);

            Physics2D.IgnoreCollision(Objectwheelcollider[0], wheelcollider[0]);
            Physics2D.IgnoreCollision(Objectwheelcollider[0], wheelcollider[1]);

            Physics2D.IgnoreCollision(Objectwheelcollider[1], wheelcollider[0]);
            Physics2D.IgnoreCollision(Objectwheelcollider[1], wheelcollider[1]);

            //Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(), GetComponent<PolygonCollider2D>());
            //Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
            antenna2Sprite.sortingOrder = 4;
            antennaSprite.sortingOrder = 4;
        }

        if (collision.gameObject.tag == "Platform")
        {
            this.transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            this.transform.parent = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            this.transform.parent = null;
        }
    }

    public void stopSFX() {
        JumpSound.enabled = false;
        WalkSound.enabled = false;
    }

    public void startSFX()
    {
        JumpSound.enabled = true;
        WalkSound.enabled = true;
    }
}
