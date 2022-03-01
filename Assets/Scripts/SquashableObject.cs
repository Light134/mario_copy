using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashableObject : DetectorDown
{
    Animator animator;
    Rigidbody2D getrigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        getrigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDetectedDown()
    {
        print("밟힘!");
        animator.SetBool("IsCrushed", true);
    }

    //스크립트가 비활성화되면 자동으로 실행되는 이벤트입니다
    public void OnDisable()
    {
        Destroy(gameObject);
    }

}
