using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour, ISpawnableObject
{
    bool isSpawning = true;

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
        isSpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, isSpawning? Time.deltaTime : 0, 0);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}