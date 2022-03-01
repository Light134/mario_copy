using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    sbyte moveDirection = 0;
    Rigidbody2D GetRigidbody2D;
    float jumpPower = 8f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        GetRigidbody2D = GetComponent<Rigidbody2D>();
    }
    public void OnSpawn()
    {
        print("½ºÆù!");
        gameObject.SetActive(true);
    }

    void onSpawnComplete()
    {
        moveDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, moveDirection == 0 ? Time.deltaTime : 0, 0);
        transform.Translate(moveDirection != 0 ? moveDirection * Time.deltaTime * 2.5f : 0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 0)
            moveDirection = (sbyte)(moveDirection * -1);
        if (collision.gameObject.layer == 3)
            GetRigidbody2D.AddForce(Vector2.up * jumpPower);
    }
}
