using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private float movementX;
    private float movementY;
    [SerializeField] float speed = 0;
    bool onGround;
    //[SerializeField] GameObject bullet;
    int count;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    // Start is called before the first frame update.
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    void OnJump()
    {
        if (onGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);
        }
    }

    /*void OnFire()
    {
        Instantiate(bullet, transform, true);
    }*/

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        int collisionCount = collision.contactCount;
        for (int i = 0; i < collisionCount; i++ )
        {
            Vector3 contactNormal = collision.contacts[i].normal;
            bool isGround = Vector3.Dot(contactNormal, Vector3.up) > 0.9;
            if (isGround)
            {
                onGround = true;
                break;
            }    
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        //speed limit
        if (rb.velocity.magnitude > 7)
        {
            rb.velocity = rb.velocity.normalized * 7;
        }
    }
}
