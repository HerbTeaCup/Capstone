using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 7f;
    float gravity = -20f;
    float yVelocity = 0;
    [SerializeField] float jumpPower = 2f;
    [SerializeField] bool isJumping = false;
    [SerializeField] int hp = 150;
    [SerializeField] int weaponPower = 10;

    CharacterController controller;
    public GameObject bulletEffect;
    ParticleSystem ps;

    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();

        ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        dir = Camera.main.transform.TransformDirection(dir);

        if (controller.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                isJumping = false;
                yVelocity = 0;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
        }

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        controller.Move(dir * moveSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo))
            {
                if ((hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.TakeDamage(weaponPower);
                }
                else
                {
                    bulletEffect.transform.position = hitInfo.point;
                    bulletEffect.transform.position = hitInfo.normal;

                    ps.Play();
                }
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        print($"플레이어 hp = {hp}");
    }
}
