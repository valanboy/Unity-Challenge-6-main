using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float speed = 15;
    public bool isMoving;
    private Vector3 travelDirection;
    private Vector3 nextCollisionPosition;

    public int minSwipeRecognition = 500;
    private Vector2 swipePositionLastFrame;
    private Vector2 swipePositionCurrentFrame;
    private Vector2 currentSwipe;

    private Color solveColor;
    private AudioSource playerAudio;
    [SerializeField] AudioClip moveSound;
    private ParticleSystem particle;


    private void Start()
        {
        rb = GetComponent<Rigidbody>();
        solveColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor;
        playerAudio = GetComponent<AudioSource>();
        particle = GetComponent<ParticleSystem>();
        particle.transform.position = transform.position;
        
        }

    private void FixedUpdate()
        {
        if (isMoving)
            {
            rb.velocity = speed * travelDirection;
            }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.1f);
        for (int i = 0; i < hitColliders.Length; i++)
            {
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();

            if(ground && !ground.isColored)
                {
                ground.ChangeColor(solveColor);
                }
            }

        if(nextCollisionPosition != Vector3.zero)
            {
            if(Vector3.Distance(transform.position, nextCollisionPosition) < 1)
                {
                isMoving = false;
                nextCollisionPosition = Vector3.zero;
                travelDirection = Vector3.zero;
                }
            }

        if (isMoving) return;

        if (Input.GetMouseButton(0))
            {
            swipePositionCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            
            if(swipePositionLastFrame != Vector2.zero)
                {
                currentSwipe = swipePositionCurrentFrame - swipePositionLastFrame;
                
                if(currentSwipe.sqrMagnitude < minSwipeRecognition)
                    {
                    return;
                    }
                currentSwipe.Normalize();

                if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                    //up/down
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                   PlayEffects();
                    }
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                    PlayEffects();
                    }
                }
                swipePositionLastFrame = swipePositionCurrentFrame;

            }

        if (Input.GetMouseButtonUp(0))
            {
            swipePositionLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
            }
        }
    
    void SetDestination(Vector3 direction)
        {
        travelDirection = direction;
        RaycastHit hit;

        if(Physics.Raycast(transform.position, direction, out hit, 100f))
            {
            nextCollisionPosition = hit.point;
            }
        isMoving = true;
        }
    void PlayEffects()
        {
        playerAudio.PlayOneShot(moveSound);
        particle.Play();
        }
    
    }
