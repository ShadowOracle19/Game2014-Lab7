using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpposumBehaviour : MonoBehaviour
{
    public float runForce;
    public Rigidbody2D rigidbody2D;
    public bool isGroundAhead;
    public LayerMask CollisionGroundLayer;
    public LayerMask CollisioWallLayer;
    public Transform lookaheadPoint;
    public Transform lookInfronPoint;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
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
        if(Physics2D.Linecast(transform.position, lookInfronPoint.position, CollisioWallLayer))
        {
            _FlipX();
        }
        Debug.DrawLine(transform.position, lookInfronPoint.position, Color.red);
    }

    private void _LookAhead()
    {
        isGroundAhead = Physics2D.Linecast(transform.position, lookaheadPoint.position, CollisionGroundLayer);

        Debug.DrawLine(transform.position, lookaheadPoint.position, Color.green);
    }
    
    

    private void _Move()
    {
        if(isGroundAhead)
        {
            rigidbody2D.AddForce(Vector2.left * runForce * Time.deltaTime * transform.localScale.x);
            rigidbody2D.velocity *= 0.90f;
            
        }
        else
        {
            _FlipX();
        }
        
    }

    private void _FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);

    }

}
