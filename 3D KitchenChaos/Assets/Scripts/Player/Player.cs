using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    #region Events & Properties

    [HideInInspector] public static Player Instance { get; private set; }


    public event EventHandler OnPickedSomething;
    [HideInInspector] public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    [HideInInspector] public class OnSelectedCounterChangedEventArgs : EventArgs { public BaseCounter baseCounter; }

    #endregion

    #region Input, Move & Visuals

    [SerializeField] private float moveSpeed = 7f;
    private bool isWalking;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    #endregion

    #region Counters & KitchenObjects

    private Vector3 lastInteractDir;
    private BaseCounter baseCounter;

    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;

    #endregion

    private void Update()
    {
        HandeleMovement();
        HandleOnInteractions();
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternativeAction += GameInput_OnInteractAlternativeAction;
    }

    private void GameInput_OnInteractAlternativeAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (baseCounter != null)
            baseCounter.InteractAlternative(this);
    }
    
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (baseCounter != null)
            baseCounter.Interact(this);
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player Instance!!!");
            Destroy(this.gameObject);
        }
        else
            Instance = this;
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandeleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        if (isWalking = inputVector != Vector2.zero)
        {
            Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

            float playerRadius = .7f;
            float playerHeight = 2f;
            float moveDistance = moveSpeed * Time.deltaTime;

            bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
            if (!canMove)
            {
                Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
                canMove = (moveDir.x < -.5f || moveDirX.x > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                    canMove = (moveDir.z < -.5f || moveDirX.z > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                    if (canMove)
                    {
                        moveDir = moveDirZ;
                    }
                }
            }

            if (canMove)
            {
                transform.position += moveDir * moveDistance;
            }

            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }

    private void HandleOnInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero)
            lastInteractDir = moveDir;

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != this.baseCounter)
                    SetSelectedCounter(baseCounter);
                return;
            }
        SetSelectedCounter(null);
    }

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        this.baseCounter = baseCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            baseCounter = this.baseCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
