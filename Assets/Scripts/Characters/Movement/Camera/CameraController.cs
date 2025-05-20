using UnityEngine;

namespace Characters.Movement.Camera
{
    public class CameraController : CustomCharacterController
    {
        [Header("Object References")] 
        [SerializeField] private VisionCone visionCone;

        [Header("Camera Settings")] 
        [SerializeField] private float startingRotation;
        [SerializeField] private float maxAngle = 45f;
        [SerializeField] private float turnTime = 2f;
        [SerializeField] private bool isStatic;

        private float _angle;
        private float _targetAngle;
        private float _angleVelocity;
        private bool _playerDetected;

        public override void Initialize()
        {
            base.Initialize();
            visionCone.Initialize(PlayerFound, PlayerLost);

            _angle = startingRotation;
            _targetAngle = startingRotation + maxAngle;
        }

        public override void UpdateMovement()
        {
            if (!isStatic && !_playerDetected)
            {
                var direction = Mathf.Sign(_targetAngle - _angle);

                _angle += direction * (maxAngle / turnTime) * Time.deltaTime;

                if ((direction > 0 && _angle >= _targetAngle) || (direction < 0 && _angle <= _targetAngle))
                {
                    _angle = _targetAngle;
                    _targetAngle = startingRotation - (_targetAngle - startingRotation);
                }
            }

            visionCone.UpdateVisionCone(_angle);
        }


        private void PlayerFound()
        {
            _playerDetected = true;
            OnPlayerFound();
        }

        private void PlayerLost()
        {
            _playerDetected = false;
            _targetAngle = startingRotation + maxAngle;
        }

        private void OnPlayerFound()
        {
            Debug.Log("Player detected by camera!");
        }
    }
}
