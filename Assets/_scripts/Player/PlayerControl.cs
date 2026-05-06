using System;
using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Characteristics")]
    [SerializeField]
    private float jumpingForce;

    [SerializeField]
    private float skatingForce;

    [SerializeField]
    private float maxSpeed = 2f;

    [SerializeField]
    private float heightToConsiderBeingOffTheGround = 0f;

    [SerializeField]
    private float waitForUnJumpTime = 0.15f;

    [SerializeField]
    private static float maxJumpHeight;
    public static float MaxJumpHeight { get => maxJumpHeight; }

    [SerializeField]
    private float inAirForceReduceMultiplier = 0.5f;

    [Header("References")]
    [SerializeField]
    private Rigidbody2D mainRigidbody;

    [SerializeField]
    private ConstantForce2D constantForce2D;

    [SerializeField]
    private Transform centerOfMass;

    [SerializeField]
    private Transform spriteTransform;

    [Header("Debug")]
    [SerializeField]
    private bool useFixedUpdate;

    // Private

    private float distanceBetweenRaycastAndBaseOfPlayer;

    // Public

    public float PlayerHeight => GetPlayerCurrentHeight();

    public static Action PlayerJumped;
    public static Action PlayerLanded;

    protected bool currentlyFlying = false;
    public bool CurrentlyFlying { get => currentlyFlying; set => currentlyFlying = value; }

    protected CharacterFacingDirection characterCurrentFacingDirection = CharacterFacingDirection.Right;
    public CharacterFacingDirection CharacterCurrentFacingDirection { get => characterCurrentFacingDirection; private set => characterCurrentFacingDirection = value; }

    // Methods

    private void Start()
    {
        mainRigidbody.centerOfMass = centerOfMass.localPosition;
        maxJumpHeight = CalculateJumpHeight();
        distanceBetweenRaycastAndBaseOfPlayer = Vector2.Distance(PlayerReferences.Instance.ConstantRayCasting.RaycastOrigin.transform.position, centerOfMass.transform.position);
    }

    private void FixedUpdate()
    {
        if (!useFixedUpdate)
            return;

        CheckForJumpOrFall();
        SetConstantForce();
    }

    private void Update()
    {
        if (useFixedUpdate)
            return;

        CheckForJumpOrFall();
        SetConstantForce();
    }

    private float rayDistance;
    public float GetPlayerCurrentHeight()
    {
        // Return positive infinity if both colliders are null
        if (PlayerReferences.Instance.ConstantRayCasting.Hit.collider == null && PlayerReferences.Instance.ConstantRayCasting.Hit2.collider == null)
        {
            return float.PositiveInfinity;
        }

        if (PlayerReferences.Instance.ConstantRayCasting.Hit.collider != null)
        {
            rayDistance = PlayerReferences.Instance.ConstantRayCasting.Hit.distance;
        }
        else if (PlayerReferences.Instance.ConstantRayCasting.Hit2.collider != null)
        {
            rayDistance = PlayerReferences.Instance.ConstantRayCasting.Hit2.distance;
        }
        else
        {
            rayDistance = PlayerReferences.Instance.ConstantRayCasting.Hit.distance < PlayerReferences.Instance.ConstantRayCasting.Hit2.distance ?
            PlayerReferences.Instance.ConstantRayCasting.Hit.distance : PlayerReferences.Instance.ConstantRayCasting.Hit2.distance;
        }

        if (rayDistance < distanceBetweenRaycastAndBaseOfPlayer)
            return 0;
        else
            return rayDistance - distanceBetweenRaycastAndBaseOfPlayer;
    }

    public void SetConstantForce()
    {
        if (Mathf.Abs(mainRigidbody.velocityX) >= maxSpeed)
        {
            SetForce(0);
        }
        else
        {
            SetForce(forceSetByController * (CurrentlyFlying ? inAirForceReduceMultiplier : 1));
        }

    }

    public void CheckForJumpOrFall()
    {
        // If the ray hits nothing, or the player is off the ground, player is flying. Set true if not true and invoke event
        if (PlayerHeight > heightToConsiderBeingOffTheGround)
        {
            if (!CurrentlyFlying)
            {
                //Debug.Log("Setting Currently Flying to true (No collider)");
                CurrentlyFlying = true;
                PlayerJumped?.Invoke();
            }

            return;
        }

        // If we get here we're on the ground, make sure set to not flying and invoke event if changing state        

        if (CurrentlyFlying)
        {
            CurrentlyFlying = false;
            PlayerLanded?.Invoke();
        }

        //// If the rigidbody velocity is positive rate of climb, we need to set to true and invoke the jump event
        //if (PlayerReferences.Instance.PlayerControl.mainRigidbody.velocityY > 0)
        //{
        //    if (!CurrentlyFlying)
        //    {
        //        //Debug.Log("Setting Currently Flying to true");
        //        CurrentlyFlying = true;
        //        PlayerJumped?.Invoke();
        //    }
        //}
        //else
        //{
        //    if (CurrentlyFlying)
        //    {
        //        //Debug.Log("Setting Currently Flying to false");
        //        CurrentlyFlying = false;
        //        PlayerLanded?.Invoke();
        //    }
        //}
    }


    private float forceSetByController;
    public void PowerLeft()
    {
        //Debug.Log("Power left");

        TurnCharacter(CharacterFacingDirection.Left);
        forceSetByController = -skatingForce;
    }

    public void PowerRight()
    {
        //Debug.Log("Power right");

        TurnCharacter(CharacterFacingDirection.Right);
        forceSetByController = skatingForce;

    }

    private Vector2 forceVector;
    public void SetForce(float xForce, float yForce = 0)
    {
        //Debug.Log($"SetForce: {xForce}");

        forceVector.x = xForce;
        forceVector.y = yForce;

        constantForce2D.relativeForce = forceVector;
        //constantForce2D.force = forceVector;
    }

    public void TurnCharacter(CharacterFacingDirection characterFacingDirection)
    {
        if (CharacterCurrentFacingDirection == characterFacingDirection)
        {
            return;
        }

        CharacterCurrentFacingDirection = characterFacingDirection;
        spriteTransform.Rotate(0f, 180f, 0f);
    }

    public void UnPower()
    {
        //Debug.Log("Unpower");

        forceSetByController = 0;

        SetForce(forceSetByController);
    }

    public void Jump()
    {
        if (CurrentlyFlying)
            return;

        mainRigidbody.AddForce(transform.up * jumpingForce, ForceMode2D.Impulse);
    }

    Vector2 lineStart;
    Vector2 lineEnd;
    float CalculateJumpHeight()
    {
        float g = mainRigidbody.gravityScale * Physics2D.gravity.magnitude;
        float v0 = jumpingForce / mainRigidbody.mass; // converts the jumpForce to an initial velocity
        return (v0 * v0) / (2 * g);
    }

    public void UnJump()
    {
        if (!CurrentlyFlying)
            return;

        if (unJumpCoroutine == null && mainRigidbody.velocityY > 0)
        {
            unJumpCoroutine = StartCoroutine(UnJumpCoroutine());
        }
    }

    private Coroutine unJumpCoroutine = null;

    public IEnumerator UnJumpCoroutine()
    {
        yield return new WaitForSeconds(waitForUnJumpTime);

        if (mainRigidbody.velocityY > 0)
            mainRigidbody.velocityY = 0;
        unJumpCoroutine = null;
    }

    public void ZeroAllForcesAndSpeed()
    {
        constantForce2D.force = new Vector2(0, 0);
        mainRigidbody.velocity = new Vector2(0, 0);
    }
}

public enum CharacterFacingDirection
{
    Left,
    Right
}
