using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExperimentalPlayer : MonoBehaviour
{
    [SerializeField]
    float jumpPower = 8f;
    [SerializeField] //SerializeField는 유니티 인스펙터에 private 멤버변수를 노출해줍니다.
    float moveSpeed = 5f;

    Collider2D getCollider2D;
    Rigidbody2D getRigidbody = null;
    Animator getAnimator;
    //마리오의 현재 변신상태를 나타내는 애니메이션. 버섯, 꽃, 별을 먹었을때 변하고 다시 원래대로 작아지기도 한다.
    Form currentAnimform = Form.Small;
    bool isEatMushroom = false;
    bool isEatFlower = false;
    bool isEatStar = false;
    bool isCanJump = false;

    // Start is called before the first frame update
    void Start()
    {
        //콜라이더를 받아온다. 마리오는 2D이므로 Collider2D로 받아온다.
        getCollider2D = GetComponent<Collider2D>();
        //리지드 바디를 받아옵니다. 마리오의 콜라이더는 2D이므로 Rigidbody2D로 받아옵니다
        getRigidbody = GetComponent<Rigidbody2D>();
        getAnimator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        ChangeAnimatorState(currentAnimform);

        OnJump();
        OnMove();

        var dir = MoveDirection.x; // 플레이어의 MoveDirection.x를 dir 지역변수로 가져옴
        if (dir == 0) return; // dir가 0이면 함수를 종료한다.
        var scale = transform.localScale; // 플레이어의 크기를 scale 지역변수로 가져옴
        scale.x = Mathf.Sign(dir); // scale의 x크기는 Mathf.Sign(이동방향)
        transform.localScale = scale; // 플레이어의 크기를 scale로 재설정한다.
    }

    //여기서 일괄로 실행합니다
    void OnDetected(Collider2D key, VerticalCode verticalCheck) //튜플로도 리팩토링 가능..ㅋㅋ
    {
        if (key && Detector.Dictionary.ContainsKey(key))
        {
            //Dictionary에서 충돌한놈의 호출할 OnDetected() 함수를 찾아줍니다.
            //하지만 아직 위에 있든 아래에 있든 상관없이 호출될것임
            Detector.Dictionary[key].OnDetected(verticalCheck);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //마리오 콜라이더와 충돌한 콜라이더의 충돌한 위치
        Vector2 detectionPoint = collision.contacts[0].normal;

        print(detectionPoint);

        //충돌한 지점이 플레이어보다 아래에 있는 경우
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

    //moveDirection 변수를 MoveDirection가 업데이트해줍니다. 대소문자가 다릅니다!! 주의!!
    Vector2 MoveDirection
    {
        get
        {
            // MoveDirection은 Vector2.right에 인풋값에 따라 양수 또는 음수를 곱한 값이다.
            return (Vector2.right * Input.GetAxis("MoveX"));
        }
    }

    void OnJump()
    {

        isCanJump = getRigidbody.velocity.y != 0 ? false : true;

        if (isCanJump && Input.GetButtonDown("Jump")) //만약 isCanJump가 참이고 스페이스바를 누른 상태라면
        {
            isCanJump = false; //isCanJump를 false로 바꾸어 재실행 방지
            //Vector2.up * jumpPower는 Vector2.up의 크기를 jumpPower만큼 키워줘서 점프력을 구현한 것입니다. (벡터의 크기 * 실수 = 곱한 크기)
            getRigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); //플레이어의 위쪽 방향으로 jumpPower만큼 힘을 가함(즉 점프함)
            print($"점프! {getRigidbody.velocity}");
            return;
        }

        if (Input.GetButtonUp("Jump") && getRigidbody.velocity.y > 0) // 플레이어의 y축 이동속도가 음수고 스페이스바를 뗐다면
        {
            //유니티 물리방향에 (1, 0)을 곱해서 y * 0이 되어버리면 점프상승중에 y축 물리력이 0이 되서 멈추게됩니다.
            getRigidbody.velocity *= Vector2.right; //수직방향 물리력을 없애 상승을 막는다.
        }
    }

    void OnMove()
    {
        //크기가1인 방향을 가진 벡터 * n의 크기 = 크기가 n이고 방향을 가진 벡터
        getRigidbody.velocity = new Vector2(MoveDirection.x * moveSpeed, getRigidbody.velocity.y);
        print("걷는다!");
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


        //움직임에 따라서 애니메이션 스테이트값이 변한다.
        void setState(AnimCode stop, AnimCode jump, AnimCode run)
        {
            animationState =
                //다방향으로 움직이는지 검사
                velocity == Vector2.zero ? stop :
                //하늘에 있는지 검사
                velocity.y != 0 ? jump :
                //좌우로 움직이는지 검사
                velocity.x != 0 ? run :
                animationState;
        }
    }

    //이걸 변신중 애니메이션 마지막 프레임에서 실행시키면 되지 않을까요?
    public void ChangingForm()
    {
        print("변신 애니메이션 실행");
        currentAnimform++; //Small은 0이고 Big은 1이기때문에 Small에서 ++하면 Big되요
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

    //이렇게 값을 안넣으면 순차적으로 0, 1, 2, 3 ...으로 값이 들어갑니다.
    enum Form
    {
        Small, //작은 마리오
        Big, //큰 마리오
        Fire, //화염꽃 먹은 상태
        SmallInvincibility,//작은 마리오의 무적 상태
        BigInvincibility //큰마리오의 무적 상태
    }
}
