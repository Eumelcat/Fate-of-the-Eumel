using UnityEngine;
using UnityEngine.PlayerLoop;

[SelectionBase] //allows selecting sprites

public class Player_Controller : MonoBehaviour
{
   #region Editor Data

   [Header("Movement Attributes")]
   [SerializeField] float _moveSpeed = 50f;
   
   [Header("Dependencies")] 
   [SerializeField] Rigidbody2D _rb;
   [SerializeField] Animator _animator;
   [SerializeField] SpriteRenderer _spriteRenderer; 
  
   #endregion

   #region Internal Data
   private Vector2 _moveDir = Vector2.zero;

   #endregion

   #region tick

   private void Update()
   {
      GatherInput();
   }

   private void FixedUpdate()
   {
      MovementUpdate();
   }
   
   #endregion
   
   #region Input Logic
   private void GatherInput()
   {
      _moveDir.x = Input.GetAxisRaw("Horizontal");
      _moveDir.y = Input.GetAxisRaw("Vertical");
   }
   #endregion

   #region Movement Logic

   private void MovementUpdate()
   {
      _rb.Velocity = _moveDir.normalized * _moveSpeed * Time.fixedDeltaTime;_
   }
   #endregion
   
}
