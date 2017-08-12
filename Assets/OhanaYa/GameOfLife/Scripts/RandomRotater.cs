using UnityEngine;
using UnityEngine.Assertions;

namespace OhanaYa.GameOfLife
{
    public sealed class RandomRotater : MonoBehaviour
    {
        #region Serialized Properties
        [SerializeField]
        float angularVelocity = 360;

        [SerializeField]
        float interval = 1;
        #endregion

        #region Unity Events
        void Start()
        {
            this.axisFrom = this.transform.up;
            this.axisTo = Random.onUnitSphere;
        }

        void Update()
        {
            var dt = Time.deltaTime;

            var t = elapsedTime / this.interval;

            var axis = Vector3.Lerp(this.axisFrom, this.axisTo, t);
            this.transform.Rotate(axis, this.angularVelocity * dt);

            if ((this.elapsedTime += dt) >= this.interval)
            {
                this.axisFrom = axisTo;
                this.axisTo = Random.onUnitSphere;

                this.elapsedTime -= this.interval;
            }
        }
        #endregion

        Vector3 axisFrom;
        Vector3 axisTo;

        float elapsedTime;
    }
}