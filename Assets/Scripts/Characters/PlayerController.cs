using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 4.5f;

    //public float timeInvincible = 2.0f;

    public Rigidbody2D rigidbBody2D;
    Vector2 moveDelta;
    float rotateAngle;

    private int sotringOrderBase = 1000;
    private SortingGroup mySortingGroup;

    private float positionRendererTimer;
    private float positionRendererTimerMax = .1f;


    private AudioSource source;

    private bool isWalking;


    //private int henchmanLine;
    private void Awake()
    {
        mySortingGroup = gameObject.GetComponent<SortingGroup>();
        rigidbBody2D = GetComponent<Rigidbody2D>();
        source = gameObject.GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        isWalking = false;
    }

    protected virtual void Update()
    {

    }

    public void HandleMovement()
    {
        float x = Input.GetAxisRaw("Vertical");// Input.GetAxisRaw("Horizontal");
        float y = - Input.GetAxisRaw("Horizontal"); //Input.GetAxisRaw("Vertical");

        if (x != 0 || y != 0) isWalking = true;
        else isWalking = false;

        moveDelta = new Vector2(x, y).normalized * speed;
        transform.Translate(moveDelta.x * Time.deltaTime, moveDelta.y * Time.deltaTime, 0);
    }

    public void HandleRotation()
    {
        var mouseScreenPos = Input.mousePosition;
        var startingScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        mouseScreenPos.x -= startingScreenPos.x;
        mouseScreenPos.y -= startingScreenPos.y;
        transform.right = mouseScreenPos - transform.position;
    }

    protected virtual void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
        PlayWalkSound();
    }

    private void LateUpdate()
    {
        SetSortingLayer();
    }

    private void SetSortingLayer()
    {
        positionRendererTimer -= Time.deltaTime;
        if (positionRendererTimer <= 0f)
        {
            positionRendererTimer = positionRendererTimerMax;
            mySortingGroup.sortingOrder = (int)(sotringOrderBase - transform.position.y * 10);
        }
    }

    private void PlayWalkSound()
    {
        if (isWalking )
        {

        }
    }
}
