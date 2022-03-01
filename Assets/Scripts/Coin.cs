using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ISpawnableObject
{
    [SerializeField]
    float jumpPower = 4;
    [SerializeField]
    float sec = 1f;
    Rigidbody2D Rigidbody2D { get; set; } // 음 이건 뭘까

    Vector2 startPosition;

    //offset.position은 기준점 위치
    public void OnSpawn()
    {
        print("스폰!");
        gameObject.SetActive(true);
        Rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        
        StartCoroutine(OnRemove());
    }

    IEnumerator OnRemove()
    {
        yield return new WaitForSeconds(sec);
        transform.position = startPosition;
        gameObject.SetActive(false);
    }

    void Start()
    {
        startPosition = transform.position;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }
}
