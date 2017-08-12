using UnityEngine;
using UnityEngine.Assertions;

namespace OhanaYa.GameOfLife
{
    public sealed class Heartbeat : MonoBehaviour
    {
        #region Serialized Properties
        [SerializeField]
        Vector3 from = Vector3.one;
        public Vector3 From
        {
            get { return this.from; }
            set { this.from = value; }
        }

        [SerializeField]
        Vector3 to = Vector3.one;
        public Vector3 To
        {
            get { return this.to; }
            set { this.to = value; }
        }

        [SerializeField]
        float interval = 1;
        public float Interval
        {
            get { return this.interval; }
            set { this.interval = value; }
        }
        #endregion

        #region Unity Events
        void Update()
        {
            var t = Mathf.Pow((this.elapsedTime / this.interval) - 1, 3) + 1;
            this.transform.localScale = Vector3.LerpUnclamped(this.to, this.from, Mathf.Clamp01(t));

            if ((this.elapsedTime += Time.deltaTime) >= this.interval)
            {
                this.elapsedTime -= this.interval;
            }
        }
        #endregion

        float elapsedTime;
    }
}