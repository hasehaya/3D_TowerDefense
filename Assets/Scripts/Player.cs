using System.Collections;

using UnityEngine;

public class Player :MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    [SerializeField] Transform cameraTransform;
    [SerializeField] private float moveSpeed = 5f;       // 移動速度
    [SerializeField] private float rotationSpeed = 720f; // 回転速度
    [SerializeField] private float jumpForce = 5f;       // ジャンプ力

    Rigidbody rb;
    Animator animator;
    Quaternion targetRotation = Quaternion.identity;
    bool canMove = true;

    private bool isGrounded = true; // 現在の地面との接地状態
    private bool isJumping = false; // ジャンプ中かどうかのフラグ

    // Pause state
    private bool isPaused = false;

    private void Awake()
    {
        // StageManagerのPauseとResumeイベントに登録
        StageManager.OnPause += Pause;
        StageManager.OnResume += Resume;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isPaused)
            return; // Pause中はUpdateを停止

        // ジャンプ入力
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            Jump();
        }

        // アニメーションの更新
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (isPaused)
            return; // Pause中はFixedUpdateを停止

        Move();
        Rotate();
        GroundCheck();
        Gravity();
    }

    void Move()
    {
        if(!canMove)
        {
            return;
        }
        // 入力取得
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // カメラの向きに合わせた移動方向
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical);

        // 入力がある場合のみ処理を行う
        if (inputDirection.sqrMagnitude > 0.01f)
        {
            inputDirection = inputDirection.normalized;

            Quaternion cameraRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            Vector3 moveDirection = cameraRotation * inputDirection;

            // 移動速度の計算
            float speedModifier = Input.GetKey(KeyCode.LeftShift) ? 1f : 0.5f;
            Vector3 velocity = moveDirection * moveSpeed * speedModifier;

            // Rigidbodyに速度を適用（垂直方向の速度は維持）
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
        else
        {
            // 入力がない場合、水平方向の速度をゼロにする
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void Rotate()
    {
        // 入力取得
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0, vertical);

        // 入力がある場合のみ回転処理を行う
        if (inputDirection.sqrMagnitude > 0.01f)
        {
            inputDirection = inputDirection.normalized;

            // カメラの向きに合わせた移動方向
            Quaternion cameraRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            Vector3 moveDirection = cameraRotation * inputDirection;

            // ターゲットの回転方向
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // スムーズに回転
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.MoveRotation(newRotation);
        }
        else
        {
            // 入力がない場合、回転速度をゼロにする
            rb.angularVelocity = Vector3.zero;
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetBool("isJump", true);
        isJumping = true;
    }

    void GroundCheck()
    {
        // プレイヤーの足元にレイを飛ばして地面をチェック
        RaycastHit hit;
        int layerToTarget = LayerMask.NameToLayer("Ground"); // レイヤー名を使用
        LayerMask layerMask = 1 << layerToTarget;
        var position = transform.position + Vector3.up * 2;

        if (Physics.Raycast(position, Vector3.down, out hit, 2.01f, layerMask))
        {
            if (isJumping)
            {
                isGrounded = true;
                isJumping = false;

                animator.SetBool("isJump", false);
            }
        }
        else
        {
            isGrounded = false;
            isJumping = true;
        }
    }

    void Gravity()
    {
        rb.AddForce(Vector3.down * 20, ForceMode.Acceleration);
    }

    void UpdateAnimator()
    {
        // 入力取得
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // スピードの計算
        float speed = new Vector2(horizontal, vertical).magnitude;
        float speedModifier = Input.GetKey(KeyCode.LeftShift) ? 1f : 0.5f;

        // アニメーションの速度を設定
        animator.SetFloat("Speed", speed * speedModifier, 0.1f, Time.deltaTime);
    }

    public void WarpTo(Vector3 position)
    {
        transform.position = position;
        rb.velocity = Vector3.zero; // 速度をリセット
    }

    // 保存用変数
    private Vector3 savedRbVelocity;
    private bool wasRbKinematic;
    private float savedAnimSpeed;

    /// <summary>
    /// ゲームの一時停止時に呼ばれるメソッド
    /// </summary>
    void Pause()
    {
        if (isPaused)
            return; // 既にPause中の場合は何もしない

        isPaused = true;

        // Rigidbodyの状態を保存
        savedRbVelocity = rb.velocity;
        wasRbKinematic = rb.isKinematic;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        // Animatorの状態を保存
        savedAnimSpeed = animator.speed;
        animator.speed = 0;

        // その他のコンポーネントや状態異常があれば必要に応じて保存・停止
    }

    /// <summary>
    /// ゲームの再開時に呼ばれるメソッド
    /// </summary>
    void Resume()
    {
        if (!isPaused)
            return; // Pauseされていない場合は何もしない

        isPaused = false;

        // Rigidbodyの状態を復元
        rb.isKinematic = wasRbKinematic;
        rb.velocity = savedRbVelocity;

        // Animatorの状態を復元
        animator.speed = savedAnimSpeed;

        // その他のコンポーネントや状態異常があれば必要に応じて復元
    }

    public void setCanMove(bool b)
    {
        canMove = b;
    }
}
