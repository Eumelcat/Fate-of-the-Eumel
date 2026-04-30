using UnityEngine;
using UnityEngine.PlayerLoop;

[SelectionBase] //allows selecting sprites

public class NewMonoBehaviourScript : MonoBehaviour
{
   #region Enums

   private enum Directions { up, down, left, right }
   #endregion
   
   
   #region Editor Data
   [Header("Movement Attributes")]
   [SerializeField] float _moveSpeed = 50f;
   
   [Header("Dependencies")] 
   [SerializeField] Rigidbody2D _rb;
   [SerializeField] Animator _animator;
   [SerializeField] SpriteRenderer _spriteRenderer;
   #endregion
   
   
   #region Internal Data
   private Vector2 _moveDirection = Vector2.zero;
   private Directions _facingDirecction = Directions.down;

   private readonly int _animationMoveDown = Animator.StringToHash("Animation_YLMCT_Move_Down");
   private readonly int _animationIdle = Animator.StringToHash("Animation_YMLCT_idle");
   
   #endregion

   #region tick

   private void Update()
   {
      GetInput();
      CalcFacingDirection();
   }

   private void FixedUpdate()
   {
      MovementUpdate();
   }
   
   #endregion
   
   #region Input Logic
   private void GetInput()
   {
      _moveDirection.x = Input.GetAxisRaw("Horizontal");
      _moveDirection.y = Input.GetAxisRaw("Vertical");
   }
   #endregion

   #region Movement Logic

   private void MovementUpdate()
   {
      _rb.linearVelocity = _moveDirection.normalized * (_moveSpeed * Time.fixedDeltaTime);
   }
   #endregion

   #region  Animation Logic

   private void CalcFacingDirection()
   {
      if (_moveDirection.y != 0)
      {
         if (_moveDirection.y > 0) //Moving up
         {
            _facingDirecction = Directions.up;
         }
         else if (_moveDirection.y < 0)
         {
            _facingDirecction = Directions.down;
         }
      }
      if (_moveDirection.magnitude > 0) // bewegung
      {
         _animator.CrossFade(_animationMoveDown, 0);
      }
      else
      {
         _animator.CrossFade(_animationIdle, 0);
      }
   }
   #endregion
   
}
