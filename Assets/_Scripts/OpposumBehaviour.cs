using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public enum RampDirection
{
    UP,
    DOWN,
    NONE
}

public class OpposumBehaviour : MonoBehaviour
{
    public float runForce;
    public Rigidbody2D rigidbody2D;
    public bool isGroundAhead;
    public LayerMask CollisionGroundLayer;
    public LayerMask CollisioWallLayer;
    public Transform lookaheadPoint;
    public Transform lookInfronPoint;
    public bool onRamp;
    public RampDirection rampDirection;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rampDirection = RampDirection.NONE;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _LookInFront();
        _LookAhead();
        _Move();
    }
    private void _LookInFront()
    {
        var wallHit = Physics2D.Linecast(transform.position, lookInfronPoint.position, CollisioWallLayer);
        if (wallHit)
        {
            if(!wallHit.collider.CompareTag("Ramps"))
            {
                if(!onRamp && transform.rotation.z == 0.0f)
                {
                    _FlipX();
                }
                
                rampDirection = RampDirection.DOWN;

            }

            else
            {
                rampDirection = RampDirection.UP;
            }
        }
        Debug.DrawLine(transform.position, lookInfronPoint.position, Color.red);
    }

    private void _LookAhead()
    {
        var groundHit = Physics2D.Linecast(transform.position, lookaheadPoint.position, CollisionGroundLayer);
        if (groundHit)
        {
            if(groundHit.collider.CompareTag("Ramps"))
            {
                onRamp = true;
            }

            if (groundHit.collider.CompareTag("platforms"))
            {
                onRamp = false;
            }


            isGroundAhead = true;
        }
        else
        {
            isGroundAhead = false;
        }

        Debug.DrawLine(transform.position, lookaheadPoint.position, Color.green);
    }
    
    

    private void _Move()
    {
        if(isGroundAhead)
        {
            rigidbody2D.AddForce(Vector2.left * runForce * Time.deltaTime * transform.localScale.x);
            if(onRamp)
            {

                if(rampDirection == RampDirection.UP)
                {
                    rigidbody2D.AddForce(Vector2.up * runForce * 0.5f * Time.deltaTime);
                }
                else
                {
                    rigidbody2D.AddForce(Vector2.down * runForce * 0.25f * Time.deltaTime);

                }
                StartCoroutine(Rotate());

            }
            else
            {
                StartCoroutine(Normalize());

            }
            rigidbody2D.velocity *= 0.90f;
            
        }
        else
        {
            _FlipX();
        }
        
    }
    
    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, -26.0f);

    }

    IEnumerator Normalize()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

    }

    private void _FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);

    }

}
