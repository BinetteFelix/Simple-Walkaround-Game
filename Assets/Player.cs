using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : NetworkBehaviour
{
    public static Player Instance;
    private Animator a_Animator;
    private Rigidbody2D p_Rigidbody;
    private SpriteRenderer s_SpriteRenderer;
    public bool p_IsDead = false;
    private bool _IsGrounded;
    public bool p_IsMoving;
    private float a_Float;
    private Vector3 DistanceFromGround;
    private Vector3 MinHeightforJump = new Vector3(0, -0.39f, 0);

    public NetworkVariable<Vector2> Position = new NetworkVariable<Vector2>();

    private void Awake()
    {
        Instance = this;
        a_Animator = GetComponent<Animator>();
        p_Rigidbody = GetComponent<Rigidbody2D>();
        s_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    public override void OnNetworkSpawn()
    {
        // Add "onStateChanged" to be called every time the value of Position is changed
        Position.OnValueChanged += OnStateChanged;

        if (IsOwner)
        {
            Move();
            AnimatePlayer();
        }
    }

    public override void OnNetworkDespawn()
    {
        // when we remove ourself as a player we dont want updates on the Position
        Position.OnValueChanged -= OnStateChanged;
    }

    public void OnStateChanged(Vector2 previous, Vector2 current)
    {
        // current is the same as Position.Value, this event is triggered
        // to tell you it has been updated
        if (current != previous)
        {
            transform.position = current;
        }
    }

    public void Move()
    {
        SubmitPositionRequestServerRpc();
    }
    public void AnimatePlayer()
    {
        SubmitAnimateRequestServerRpc();
    }

    [Rpc(SendTo.Server)] // attribute, meta data so other functions know what kind of function this is. 
    void SubmitPositionRequestServerRpc(RpcParams rpcParams = default)
    {
        DistanceFromGround = new Vector3(0, transform.position.y, 0);
        p_IsMoving = (p_Rigidbody.GetComponent<Rigidbody2D>().linearVelocityY < -0.15f | p_Rigidbody.GetComponent<Rigidbody2D>().linearVelocityY > 0.15f);
        _IsGrounded = (Vector3.Distance(DistanceFromGround, MinHeightforJump) <= 1f & !p_IsMoving);

        if (Input.GetButton("Horizontal") && !p_IsDead)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                // Make Sprite Face the Left
                s_SpriteRenderer.flipX = true;
                // If player clicks space -> player jumps, move player upwards
                if (Input.GetButtonDown("Jump") && _IsGrounded)
                {
                    p_Rigidbody.GetComponent<Rigidbody2D>().linearVelocityY = 5.0f;
                }
                // If player only wants to move to the Left, move to the Left
                else
                    p_Rigidbody.GetComponent<Rigidbody2D>().linearVelocityX = -1.5f;
                    //gameObject.transform.position += new Vector3(-1.5f, 0, 0) * Time.fixedDeltaTime;
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                // Make Sprite Face the right
                s_SpriteRenderer.flipX = false;

                // If player clicks space -> player jumps, move player upwards
                if (Input.GetButtonDown("Jump") && _IsGrounded)
                {
                    p_Rigidbody.GetComponent<Rigidbody2D>().linearVelocityY = 5.0f;
                }
                // If player only wants to move to the right, move to the right
                else
                    p_Rigidbody.GetComponent<Rigidbody2D>().linearVelocityX = 1.5f;
                //gameObject.transform.position += new Vector3(1.5f, 0, 0) * Time.fixedDeltaTime;
            }  
        }
        if (Input.GetButtonDown("Jump") && _IsGrounded)
        {
            p_Rigidbody.GetComponent<Rigidbody2D>().linearVelocityY = 5.0f;
            //gameObject.transform.position += new Vector3(0, 4f, 0) * Time.fixedDeltaTime;
        }
    }
    [Rpc(SendTo.Server)] // attribute, meta data so other functions know what kind of function this is. 
    void SubmitAnimateRequestServerRpc(RpcParams rpcParams = default)
    {
        if (Input.GetButton("Horizontal") && !p_IsDead)
        {
            a_Float = 0.6f;
            a_Animator.SetFloat("Speed", a_Float);

            if (Input.GetButtonDown("Jump") && !p_IsDead && _IsGrounded)
            {
                a_Animator.SetTrigger("Jump");
            }
        }
        else if (Input.GetButtonDown("Jump") && !p_IsDead && _IsGrounded)
        {
            a_Float = 0.4f;
            a_Animator.SetTrigger("Jump");
            a_Animator.SetFloat("Speed", a_Float);
        }
        else if (p_IsDead)
        {
            a_Animator.SetTrigger("Death");
            p_IsDead = true;
        }
        else
        {
            a_Float = 0.4f;
            a_Animator.SetFloat("Speed", a_Float);
        }
    }

    static Vector2 GetRandomPosition()
    {
        return new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
    }
}