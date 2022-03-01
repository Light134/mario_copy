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
        print("����!");
        animator.SetBool("IsCrushed", true);
    }

    //��ũ��Ʈ�� ��Ȱ��ȭ�Ǹ� �ڵ����� ����Ǵ� �̺�Ʈ�Դϴ�
    public void OnDisable()
    {
        Destroy(gameObject);
    }

}
