using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour, ISpawnableObject
{
    sbyte moveDirection = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
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
        transform.Translate(moveDirection != 0? moveDirection* Time.deltaTime * 2.5f : 0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 0)
            moveDirection = (sbyte)(moveDirection * -1);
    }
}
