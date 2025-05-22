using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float bulletSpeed = 10f; // Speed of the bullet
    public float topRotation = 90f;
    public float downRotation = 90f;


    void Start()
    {
        Destroy(gameObject, 5f); // Destroys the bullet after 5 seconds
    }

    void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);
    }    

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object the bullet collided with has the specific tag
        if (collision.gameObject.CompareTag("TopWall"))
        {
            // Change the bullet's rotation
            transform.rotation = Quaternion.Euler(0, 0, topRotation);
            Debug.Log("choqué arriba");
        }
        else if (collision.gameObject.CompareTag("DownWall"))
        {
            // Change the bullet's rotation
            transform.rotation = Quaternion.Euler(0, 0, downRotation);
            Debug.Log("choqué abajo");
        }
        else if (collision.gameObject.CompareTag("SideWall"))
        {
            Destroy(gameObject);
        }        
    }    
}
