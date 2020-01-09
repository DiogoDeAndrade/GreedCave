using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TestMove : MonoBehaviour
{
    public GameObject      gfxObject;
    public bool            enableMove = false;
    [ShowIf("enableMove")]
    public float           moveSpeed = 1.0f;
    [ShowIf("enableMove"), Range(0,1)]
    public float           drag = 0.1f;

    Animator            anim;
    SpriteRenderer      spriteRenderer;
    CharacterController charController;

    Vector2             lastMoveDir = new Vector2(0.0f, -1.0f);
    Material            spriteMaterial;
    Coroutine           flashCR;

    void Start()
    {
        anim = gfxObject.GetComponent<Animator>();
        spriteRenderer = gfxObject.GetComponent<SpriteRenderer>();
        spriteMaterial = spriteRenderer.material;
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 mv = Vector2.zero;
        mv.x = Input.GetAxis("Horizontal");
        mv.y = Input.GetAxis("Vertical");

        if (mv.magnitude > 0.1f)
        {
            lastMoveDir = mv.normalized;
        }

        anim.SetFloat("DirX", lastMoveDir.x);
        anim.SetFloat("DirY", lastMoveDir.y);

        anim.SetFloat("Speed", mv.magnitude);

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Slash");
        }
        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Thrust");
        }

        anim.SetBool("Shoot", Input.GetButton("Fire3"));

        if (Input.GetButtonDown("Jump"))
        {
            if (flashCR != null)
            {
                StopCoroutine(flashCR);
            }
            flashCR = StartCoroutine(FlashCR());
        }

        if (enableMove)
        {
            charController.Move(new Vector3(mv.x * moveSpeed * Time.deltaTime, 0.0f, mv.y * moveSpeed * Time.deltaTime));
        }
    }

    IEnumerator FlashCR()
    {
        Color c = Color.red;

        while (c.a > 0.0f)
        {
            spriteMaterial.SetColor("_Color", c);

            c.a -= Time.deltaTime * 2.0f;

            yield return null;
        }

        c.a = 0;
        spriteMaterial.SetColor("_Color", c);

        flashCR = null;
    }
}
