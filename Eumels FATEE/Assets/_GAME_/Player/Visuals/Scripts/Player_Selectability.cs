using UnityEngine;
using UnityEngine.PlayerLoop;

[SelectionBase] //allows selecting sprites

public class NewMonoBehaviourScript : MonoBehaviour
{
   #region Editor Data
   [Header("Movement Attributes")]
   [SerializeField] float _moveSpeed = 50f;
   
   [Header("Dependencies")] [SerializeField]
   private Rigidbody2D _rb;
   #endregion
   
   
   #region Internal Data
   private Vector2 _moveDirection = Vector2.zero;
   #endregion

   #region tick

   private void Update()
   {
      GetInput();
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
      
      print(_moveDirection);
   }
   #endregion

   #region Movement Logic

   private void MovementUpdate()
   {
      _rb.linearVelocity = _moveDirection * (_moveSpeed * Time.fixedDeltaTime);
   }
   #endregion
}
