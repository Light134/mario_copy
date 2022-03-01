using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExperimentalPlayer : MonoBehaviour
{
    [SerializeField]
    float jumpPower = 8f;
    [SerializeField] //SerializeField�� ����Ƽ �ν����Ϳ� private ��������� �������ݴϴ�.
    float moveSpeed = 5f;

    Collider2D getCollider2D;
    Rigidbody2D getRigidbody = null;
    Animator getAnimator;
    //�������� ���� ���Ż��¸� ��Ÿ���� �ִϸ��̼�. ����, ��, ���� �Ծ����� ���ϰ� �ٽ� ������� �۾����⵵ �Ѵ�.
    Form currentAnimform = Form.Small;
    bool isEatMushroom = false;
    bool isEatFlower = false;
    bool isEatStar = false;
    bool isCanJump = false;

    // Start is called before the first frame update
    void Start()
    {
        //�ݶ��̴��� �޾ƿ´�. �������� 2D�̹Ƿ� Collider2D�� �޾ƿ´�.
        getCollider2D = GetComponent<Collider2D>();
        //������ �ٵ� �޾ƿɴϴ�. �������� �ݶ��̴��� 2D�̹Ƿ� Rigidbody2D�� �޾ƿɴϴ�
        getRigidbody = GetComponent<Rigidbody2D>();
        getAnimator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        ChangeAnimatorState(currentAnimform);

        OnJump();
        OnMove();

        var dir = MoveDirection.x; // �÷��̾��� MoveDirection.x�� dir ���������� ������
        if (dir == 0) return; // dir�� 0�̸� �Լ��� �����Ѵ�.
        var scale = transform.localScale; // �÷��̾��� ũ�⸦ scale ���������� ������
        scale.x = Mathf.Sign(dir); // scale�� xũ��� Mathf.Sign(�̵�����)
        transform.localScale = scale; // �÷��̾��� ũ�⸦ scale�� �缳���Ѵ�.
    }

    //���⼭ �ϰ��� �����մϴ�
    void OnDetected(Collider2D key, VerticalCode verticalCheck) //Ʃ�÷ε� �����丵 ����..����
    {
        if (key && Detector.Dictionary.ContainsKey(key))
        {
            //Dictionary���� �浹�ѳ��� ȣ���� OnDetected() �Լ��� ã���ݴϴ�.
            //������ ���� ���� �ֵ� �Ʒ��� �ֵ� ������� ȣ��ɰ���
            Detector.Dictionary[key].OnDetected(verticalCheck);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //������ �ݶ��̴��� �浹�� �ݶ��̴��� �浹�� ��ġ
        Vector2 detectionPoint = collision.contacts[0].normal;

        print(detectionPoint);

        //�浹�� ������ �÷��̾�� �Ʒ��� �ִ� ���
        bool isDown = detectionPoint.y > 0f;
        VerticalCode verticalCheck = isDown ? VerticalCode.DOWN : VerticalCode.UP;
        isCanJump = collision.gameObject.layer == 3;

        if (isDown)
        {
            isCanJump = true;
            if (collision.gameObject.layer == 9)
            {
                getRigidbody.AddForce(Vector2.up * jumpPower * 0.75f, ForceMode2D.Impulse);
            }
        }
        OnDetected(collision.collider, verticalCheck);

        if (collision.gameObject.layer == 7)
        {
            var itemState = collision.gameObject.GetComponent<Animator>();
            switch (itemState.GetInteger("itemState"))
                {
                case 1:
                    isEatMushroom = true;
                    break;
                case 2:
                    if (isEatMushroom)
                        isEatFlower = true;
                    break;
                case 3:
                    if (isEatMushroom || isEatFlower)
                        isEatStar = true;
                    break;
                }
        }
    }

    //moveDirection ������ MoveDirection�� ������Ʈ���ݴϴ�. ��ҹ��ڰ� �ٸ��ϴ�!! ����!!
    Vector2 MoveDirection
    {
        get
        {
            // MoveDirection�� Vector2.right�� ��ǲ���� ���� ��� �Ǵ� ������ ���� ���̴�.
            return (Vector2.right * Input.GetAxis("MoveX"));
        }
    }

    void OnJump()
    {

        isCanJump = getRigidbody.velocity.y != 0 ? false : true;

        if (isCanJump && Input.GetButtonDown("Jump")) //���� isCanJump�� ���̰� �����̽��ٸ� ���� ���¶��
        {
            isCanJump = false; //isCanJump�� false�� �ٲپ� ����� ����
            //Vector2.up * jumpPower�� Vector2.up�� ũ�⸦ jumpPower��ŭ Ű���༭ �������� ������ ���Դϴ�. (������ ũ�� * �Ǽ� = ���� ũ��)
            getRigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); //�÷��̾��� ���� �������� jumpPower��ŭ ���� ����(�� ������)
            print($"����! {getRigidbody.velocity}");
            return;
        }

        if (Input.GetButtonUp("Jump") && getRigidbody.velocity.y > 0) // �÷��̾��� y�� �̵��ӵ��� ������ �����̽��ٸ� �ôٸ�
        {
            //����Ƽ �������⿡ (1, 0)�� ���ؼ� y * 0�� �Ǿ������ ��������߿� y�� �������� 0�� �Ǽ� ���߰Ե˴ϴ�.
            getRigidbody.velocity *= Vector2.right; //�������� �������� ���� ����� ���´�.
        }
    }

    void OnMove()
    {
        //ũ�Ⱑ1�� ������ ���� ���� * n�� ũ�� = ũ�Ⱑ n�̰� ������ ���� ����
        getRigidbody.velocity = new Vector2(MoveDirection.x * moveSpeed, getRigidbody.velocity.y);
        print("�ȴ´�!");
    }

    void ChangeAnimatorState(Form form)
    {
        AnimCode animationState = AnimCode.IDLE_ANIMATION;
        var velocity = getRigidbody.velocity;
        switch (form)
        {
            case Form.Small:
                {
                    if (isEatMushroom)
                    {
                        animationState = AnimCode.SMALL_TO_BIG_TRANSITION;
                        break;
                    }
                    setState(
                        AnimCode.IDLE_ANIMATION,
                        AnimCode.JUMP_ANIMATION,
                        AnimCode.RUN_ANIMATION
                    );
                    break;
                }
            case Form.Big:
                {
                    if (isEatFlower)
                    {
                        animationState = AnimCode.BIG_TO_FIRE_TRANSITION;
                        break;
                    }
                    setState(
                        AnimCode.IDLE_ANIMATION_MUSHROOM,
                        AnimCode.JUMP_ANIMATION_MUSHROOM,
                        AnimCode.RUN_ANIMATION_MUSHROOM
                    );
                    break;
                }
            case Form.Fire:
                {
                    setState(
                        AnimCode.IDLE_ANIMATION_FLOWER,
                        AnimCode.JUMP_ANIMATION_FLOWER,
                        AnimCode.RUN_ANIMATION_FLOWER
                        );
                }
                break;
            case Form.SmallInvincibility:
                break;
            case Form.BigInvincibility:
                break;
        }

        print(animationState.ToString());
        getAnimator.Play(animationState.ToString());


        //�����ӿ� ���� �ִϸ��̼� ������Ʈ���� ���Ѵ�.
        void setState(AnimCode stop, AnimCode jump, AnimCode run)
        {
            animationState =
                //�ٹ������� �����̴��� �˻�
                velocity == Vector2.zero ? stop :
                //�ϴÿ� �ִ��� �˻�
                velocity.y != 0 ? jump :
                //�¿�� �����̴��� �˻�
                velocity.x != 0 ? run :
                animationState;
        }
    }

    //�̰� ������ �ִϸ��̼� ������ �����ӿ��� �����Ű�� ���� �������?
    public void ChangingForm()
    {
        print("���� �ִϸ��̼� ����");
        currentAnimform++; //Small�� 0�̰� Big�� 1�̱⶧���� Small���� ++�ϸ� Big�ǿ�
    }

    enum AnimCode 
    {
        IDLE_ANIMATION,
        JUMP_ANIMATION,
        RUN_ANIMATION,
        SMALL_TO_BIG_TRANSITION,
        IDLE_ANIMATION_MUSHROOM,
        JUMP_ANIMATION_MUSHROOM,
        RUN_ANIMATION_MUSHROOM,
        BIG_TO_FIRE_TRANSITION,
        IDLE_ANIMATION_FLOWER,
        JUMP_ANIMATION_FLOWER,
        RUN_ANIMATION_FLOWER
    }

    //�̷��� ���� �ȳ����� ���������� 0, 1, 2, 3 ...���� ���� ���ϴ�.
    enum Form
    {
        Small, //���� ������
        Big, //ū ������
        Fire, //ȭ���� ���� ����
        SmallInvincibility,//���� �������� ���� ����
        BigInvincibility //ū�������� ���� ����
    }
}
