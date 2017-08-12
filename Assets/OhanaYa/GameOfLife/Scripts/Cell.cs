using UnityEngine;
using UnityEngine.Assertions;

using System.Collections.Generic;

namespace OhanaYa.GameOfLife
{
    [DisallowMultipleComponent]
    public sealed class Cell : MonoBehaviour
    {
        #region Serialized Properties
        #endregion

        #region Unity Events
        void Awake()
        {
            this.cachedRenderer = this.GetComponentInChildren<Renderer>();
            Assert.IsNotNull(this.cachedRenderer);

            this.colorId = Shader.PropertyToID("_EmissionColor");

            this.colorBlock = new MaterialPropertyBlock();

            this.Color = Color.black;

            this.cachedHeartbeat = this.GetComponent<Heartbeat>();
            Assert.IsNotNull(this.cachedHeartbeat);
            this.initialHeartbeatScale = this.cachedHeartbeat.To;
        }
        #endregion

        #region Caches
        Renderer cachedRenderer;

        int colorId;

        Color cachedColor;

        Heartbeat cachedHeartbeat;
        #endregion

        Vector3 initialHeartbeatScale;

        MaterialPropertyBlock colorBlock;

        public Color Color
        {
            get
            {
                return this.cachedColor;
            }

            set
            {
                if (value == this.cachedColor)
                {
                    return;
                }

                this.cachedColor = value;
                this.colorBlock.SetColor(this.colorId, value);
                this.cachedRenderer.SetPropertyBlock(this.colorBlock);
            }
        }

        public bool IsAlive { get; set; }

        readonly List<Cell> neighbors = new List<Cell>();

        public void AddNeighbors(IEnumerable<Cell> cells)
        {
            foreach (var cell in cells)
            {
                if (!this.neighbors.Contains(cell))
                {
                    this.neighbors.Add(cell);
                }
            }
        }

        bool isEvaluated;
        bool nextState;
        Color nextColor;

        public void Evaluate()
        {
            if (this.isEvaluated)
            {
                return;
            }

            this.isEvaluated = true;

            var aliveNeighbours = 0;
            var totalColor = new Color(0, 0, 0, 0);
            foreach (var n in this.neighbors)
            {
                if (n.IsAlive)
                {
                    ++aliveNeighbours;

                    totalColor += n.Color;
                }
            }

            if (this.IsAlive)
            {
                this.nextState = 2 <= aliveNeighbours && aliveNeighbours <= 3;
                this.nextColor = this.nextState ? this.Color : Color.black;
            }
            else
            {
                this.nextState = aliveNeighbours == 3;
                if (this.nextState)
                {
                    var averageColor = totalColor / aliveNeighbours;

                    averageColor[Random.Range(0, 3)] = 0;
                    averageColor[Random.Range(0, 3)] = 1;

                    this.nextColor = averageColor;
                }
                else
                {
                    if (aliveNeighbours == 0 && Random.Range(0, 30) == 0)
                    {
                        this.nextState = true;
                        this.nextColor = Random.ColorHSV(0.25f, 0.75f);
                    }
                    else
                    {
                        this.nextColor = Color.black;
                    }
                }
            }
        }

        public void Apply()
        {
            this.Color = this.nextColor;
            this.IsAlive = this.nextState;
            this.isEvaluated = false;

            this.cachedHeartbeat.To = this.IsAlive
                ? this.initialHeartbeatScale * 2
                : this.initialHeartbeatScale;
        }
    }
}