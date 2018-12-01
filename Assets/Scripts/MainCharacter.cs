using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MainCharacter : MonoBehaviour
{
    // Variables
    // --------------------------------------------------------------------------------------------------------------------------------
    // Layermasks
    public LayerMask playerMask, punchMask, activateMask;
    // Character body
    Transform myTransform, tagGround;
    Rigidbody2D myBody;
    // Character characteristics
    public Sprite[] walkL, walkR, jump, land, stand;
    public float walkFPS, jumpFPS, normaljumpFPS, airjumpFPS, landFPS, standFPS, punchLength;
    public float speed, jumpVelocity, airJumpVelocity;
    public float ajRespawnTime, adRespawnTime;
    public float airdashShort = 1;
    public float airdashLong = 2;
    public float move = 0;
    int AnimationNum = 0, walkLNum = 0, walkRNum = 0, jumpNum = 0, landNum = 0, standNum = 0;
    private float lastWalk, lastJump, lastLand, lastStand = 0;
    SpriteRenderer sR;
    // Booleans to keep track of character status
    public bool isGrounded = false;
    public bool isLanding = false;
    public bool hasAirdash = true;
    public bool hasAirJump = false;
    public bool isJumping = false;


    // Start settings
    // --------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        sR = GetComponent<SpriteRenderer>();
        isGrounded = true;
        isLanding = false;
        //GameObject[] groundList = GameObject.FindGameObjectsWithTag("Ground");
    }


    // Main Update Function
    // --------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        move = 0;
        // Control in Air
        // For facing the the right direction in the air
        if (!isGrounded)
        {
            if (hasAirJump)
            {
                if (Input.GetKeyDown("w")) // && isGrounded) //Input.GetKeyDown(KeyCode.UpArrow) || )
                {
                    isLanding = false;
                    hasAirJump = false;
                    AnimationNum = 3;
                    jumpNum = 0;
                    jumpFPS = airjumpFPS;
                    AirJump();
                }
            }
            //isLanding = false;
            if (Input.GetKey("d")) // || Input.GetKey(KeyCode.RightArrow))
            {
                move = 1;
                sR.flipX = false;
                // If the air dash is activated
                if (Input.GetKeyDown("i") && hasAirdash || Input.GetKeyDown("o") && hasAirdash)
                {
                    if (Input.GetKeyDown("i"))
                    {
                        Airdash(1, airdashShort);
                    }
                    else
                    {
                        Airdash(1, airdashLong);
                    }
                }
            }
            else if (Input.GetKey("a")) // || Input.GetKey(KeyCode.LeftArrow))
            {
                move = -1;
                sR.flipX = true;
                // If the air dash is activated
                if (Input.GetKeyDown("i") && hasAirdash || Input.GetKeyDown("o") && hasAirdash)
                {
                    if (Input.GetKeyDown("i"))
                    {
                        Airdash(-1, airdashShort);
                    }
                    else
                    {
                        Airdash(-1, airdashLong);
                    }
                }
            }
            AnimationNum = 3;
        }
        else
        {
            jumpNum = 0;
            // Jumping
            if (Input.GetKeyDown("w")) // && isGrounded) //Input.GetKeyDown(KeyCode.UpArrow) || )
            {
                isLanding = false;
                isGrounded = false;
                AnimationNum = 3;
                Jump();
            }
            // Move right
            else if (Input.GetKey("d")) // && isGrounded) //|| Input.GetKey(KeyCode.RightArrow) )
            {
                move = 1;
                AnimationNum = 1;
                sR.flipX = false;
            }
            // Move left
            else if (Input.GetKey("a")) // && isGrounded) // || Input.GetKey(KeyCode.LeftArrow))
            {
                move = -1;
                AnimationNum = 2;
                sR.flipX = true;
            }       
        }
        // If it's landing set AnimationNum to 4
        if (isLanding)
        {
            AnimationNum = 4;
        }
        Animate();
    }

    void FixedUpdate()
    {
        myBody.velocity = new Vector2(move * speed, myBody.velocity.y);
        //transform.position += (new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Time.deltaTime * speed);
    }

    // Movement Functions
    // --------------------------------------------------------------------------------------------------------------------------------
    // Function for Jumping
    public void Jump()
    {
        myBody.AddForce(50 * jumpVelocity * Vector2.up);
        isGrounded = false;
        isJumping = true;
    }
    // Function for AirJumping
    public void AirJump()
    {
        myBody.velocity = new Vector2(myBody.velocity.x, 0);
        myBody.AddForce(50 * airJumpVelocity * Vector2.up);
    }
    // Function for Airdashing
    public void Airdash(float a, float b)
    {
        //transform.position += new Vector3(a, myBody.velocity.y * .1F, 0) * b;
        //transform.position += new Vector3(a, 0, 0) * b;
        myBody.AddForce(500 * Vector2.right * a * b);
        hasAirdash = false;
        myBody.velocity = new Vector2(myBody.velocity.x, 0);
        jumpNum = 8;
    }


    // Collision Functions
    // --------------------------------------------------------------------------------------------------------------------------------
    // Checks when one collider is touching another collider
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isGrounded = true;
            //isJumping = false;
            //hasAirJump = false;
        }
    }
    // Checks when character stops touching a collider
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            if (!isJumping)
            {
                hasAirdash = true;
                jumpNum = 8;
            }
            isGrounded = false;
        }    
    }
    // Checks when character starts touching a collider
    void OnCollisionEnter2D(Collision2D other)
    {
        // Checks if the character is on the ground so it can jump
        if (other.collider.tag == "Ground")
        {
            if (!isGrounded)
            {
                isLanding = true;
                hasAirdash = true;
                jumpFPS = normaljumpFPS;
            }
            isGrounded = true;
            isJumping = false;
            hasAirJump = false;
        }
        //if (gameObject.GetComponentsInChildren<Collider>(). "JumpThroughPlatform")
        //{
        //    if (myBody.velocity.y <= 0)
        //    {
        //        other.rigidbody.WakeUp();
        //        isLanding = true;
        //        hasAirdash = true;
        //        jumpFPS = normaljumpFPS;
        //        isGrounded = true;
        //        isJumping = false;
        //        hasAirJump = false;
        //        myBody.velocity.Set(myBody.velocity.x, 0);
        //    }
        //    else
        //    {
        //        other.rigidbody.Sleep();
        //    }
        //}
    }


    // Trigger functions
    // --------------------------------------------------------------------------------------------------------------------------------
    // Checks when character touches a trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        // Checks if the character has touched a trigger
        if (other.gameObject.CompareTag("AirJumpItem"))
        {
            hasAirJump = true;
            other.gameObject.SetActive(false);
            StartCoroutine(RespawnAfterSeconds(ajRespawnTime, other.gameObject));
        }
        if (other.gameObject.CompareTag("AirDashItem"))
        {
            hasAirdash = true;
            other.gameObject.SetActive(false);
            StartCoroutine(RespawnAfterSeconds(adRespawnTime, other.gameObject));
        }
        if (other.gameObject.CompareTag("JumpThroughPlatform"))
        {
            if (myBody.velocity.y <= 0)
            {
                isLanding = true;
                hasAirdash = true;
                jumpFPS = normaljumpFPS;
                isGrounded = true;
                isJumping = false;
                hasAirJump = false;
                myBody.velocity.Set(myBody.velocity.x, 0);
            }
        }
    }
    // Checks if the character has stopped touching a trigger
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "JumpThroughPlatform")
        {
            if (!isJumping)
            {
                hasAirdash = true;
                jumpNum = 8;
            }
            isGrounded = false;
        }
    }


    // Coroutines
    // --------------------------------------------------------------------------------------------------------------------------------
    // Respawns the airjump item
    IEnumerator RespawnAfterSeconds(float a, GameObject b)
    {
        yield return new WaitForSeconds(a);
        b.SetActive(true);
    }

    /*
	void OnCollisionEnter2D(Collision2D other){

		//when the player hits a spike they die and the level starts over
		if (other.collider.tag == "spike") {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		//the player is no longer standing on black/white/wall when they are no longer colliding with them
		if (other.collider.tag == "black") {
			standingOnBlack = false;
		} else if (other.collider.tag == "white") {
			standingOnWhite = false;
		} else if (other.collider.tag == "wall") {
			standingOnWall = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		//determine if the player has collected the coin to unlock the door
		if (other.gameObject.CompareTag ("coin")) {
			other.gameObject.SetActive (false);
			collected = true;
		}
	}

	void OnTriggerStay2D(Collider2D other) {

		//load the appropriate next level when the player goes through the door and the door has been unlocked with the coin
		if (Input.GetKeyDown (KeyCode.UpArrow) && other.tag == "door" && collected) {
			string scenetoload = "Level 1";
			string currentlevel = SceneManager.GetActiveScene ().name;

			if (currentlevel == "Level 1") {
				scenetoload = "Level 2";
			} else if (currentlevel == "Level 2") {
				scenetoload = "Level 3";
			} else if (currentlevel == "Level 3") {
				scenetoload = "Level 4";
			} else if (currentlevel == "Level 4") {
				scenetoload = "Level 5";
			} else if (currentlevel == "Level 5")  {
                scenetoload = "Level 6";
            } else if (currentlevel == "Level 6") {
                scenetoload = "Level 7";
            } else if (currentlevel == "Level 7") {
                scenetoload = "Level 8";
            } else if (currentlevel == "Level 8") {
                scenetoload = "Level 9";
            } else if (currentlevel == "Level 9") {
				scenetoload = "Level 10";
			} else if (currentlevel == "Level 10") {
				scenetoload = "Game Over";
			}


			//load next scene and reset the gravity/flipped/gravflipped variables for the next level
			SceneManager.LoadScene(scenetoload);

	}*/

    // Animation
    // --------------------------------------------------------------------------------------------------------------------------------
    public void Animate()
    {
        // Walk Right
        if (AnimationNum == 1)
        {
            walkLNum = 0;
            jumpNum = 0;
            landNum = 0;
            if ((lastWalk + 1 / walkFPS) < Time.time)
            {
                lastWalk = Time.time;
                sR.sprite = walkR[walkRNum];
                //sR.flipX = false;
                walkRNum++;
                if (walkRNum >= walkR.Length)
                {
                    walkRNum = 0;
                }

            }
        }
        // Walk Left
        else if (AnimationNum == 2)
        {
            walkRNum = 0;
            jumpNum = 0;
            landNum = 0;
            if ((lastWalk + 1 / walkFPS) < Time.time)
            {
                lastWalk = Time.time;
                sR.sprite = walkL[walkLNum];
                //sR.flipX = false;
                walkLNum++;
                if (walkLNum >= walkL.Length)
                {
                    walkLNum = 0;
                }

            }
        }
        // Jumping
        else if (AnimationNum == 3)
        {
            walkRNum = 0;
            walkLNum = 0;
            landNum = 0;
            if ((lastJump + 1 / jumpFPS) < Time.time)
            {
                //Debug.Log("jumping");
                lastJump = Time.time;
                sR.sprite = jump[jumpNum];
                //sR.flipX = false;
                jumpNum++;
                if (jumpNum >= jump.Length)
                {
                    //jumpNum = jump.Length - 1;
                    jumpNum = 12;
                }

            }
            // sR.sprite = jump[4];
        }
        // Landing on Ground
        else if (AnimationNum == 4)
        {
            walkRNum = 0;
            walkLNum = 0;
            jumpNum = 0;
            if ((lastLand + 1 / landFPS) < Time.time)
            {
                //Debug.Log("going");
                lastLand = Time.time;
                sR.sprite = land[landNum];
                landNum++;
                if (landNum == land.Length)
                {
                    isLanding = false;
                }
            }
        }
        // Standing still
        else
        {
            walkLNum = 0;
            walkRNum = 0;
            jumpNum = 0;
            landNum = 0;
            if ((lastStand + 1 / standFPS) < Time.time)
            {
                lastStand = Time.time;
                sR.sprite = stand[standNum];
                //sR.flipX = false;
                standNum++;
                if (standNum >= stand.Length)
                {
                    standNum = 0;
                }

            }
            /*if ((lastStand + 1 / standFPS) < Time.time)
            {
                lastStand = Time.time;
                sR.sprite = stand[standNum];
                //sR.flipX = false;
                standNum++;
                if (standNum >= stand.Length)
                {
                    standNum = 0;
                }

            }*/
        }
        AnimationNum = 0;
    }
}