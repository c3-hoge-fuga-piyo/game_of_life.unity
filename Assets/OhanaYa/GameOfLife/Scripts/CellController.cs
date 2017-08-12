using UnityEngine;
using UnityEngine.Assertions;

using System.Collections.Generic;

namespace OhanaYa.GameOfLife
{
    [DisallowMultipleComponent]
    public sealed class CellController : MonoBehaviour
    {
        #region Serialized Properties
        [SerializeField]
        float interval = 1;

        [SerializeField]
        Cell[] front;

        [SerializeField]
        Cell[] back;

        [SerializeField]
        Cell[] left;

        [SerializeField]
        Cell[] right;

        [SerializeField]
        Cell[] top;

        [SerializeField]
        Cell[] bottom;
        #endregion

        #region Unity Events
        void Start()
        {
            Application.targetFrameRate = 60;

            this.world = new Cell[][]{
                this.front,
                this.back,
                this.left,
                this.right,
                this.top,
                this.bottom,
            };

            foreach (var cells in this.world)
            {
                this.Connect(cells, 9, 9);
            }
        }

        void Update()
        {
            var canUpdate = this.elapsedTime >= this.interval;
            this.elapsedTime += Time.deltaTime;

            if (!canUpdate)
            {
                return;
            }

            this.elapsedTime -= this.interval;

            foreach (var cells in this.world)
            {
                foreach (var cell in cells)
                {
                    cell.Evaluate();
                }
            }

            foreach (var cells in this.world)
            {
                foreach (var cell in cells)
                {
                    cell.Apply();
                }
            }
        }
        #endregion

        #region Caches
        Cell[][] world;
        #endregion

        float elapsedTime;

        static readonly List<Cell> neighborsBuffer = new List<Cell>();

        void Connect(Cell[] board, int width, int height)
        {
            for (var i = 0; i < height; ++i)
            {
                for (var j = 0; j < width; ++j)
                {
                    neighborsBuffer.Clear();

                    var index = (i * width) + j;
                    var cell = board[index];

                    var hasUp = 0 < i;
                    var hasDown = i < height - 1;
                    var hasLeft = 0 < j;
                    var hasRight = j < width - 1;

                    if (hasUp)
                    {
                        var at = ((i - 1) * width) + j;
                        neighborsBuffer.Add(board[at]);
                    }

                    if (hasDown)
                    {
                        var at = ((i + 1) * width) + j;
                        neighborsBuffer.Add(board[at]);
                    }

                    if (hasLeft)
                    {
                        var at = (i * width) + j - 1;
                        neighborsBuffer.Add(board[at]);
                    }

                    if (hasRight)
                    {
                        var at = (i * width) + j + 1;
                        neighborsBuffer.Add(board[at]);
                    }

                    if (hasLeft && hasUp)
                    {
                        var at = ((i - 1) * width) + j - 1;
                        neighborsBuffer.Add(board[at]);
                    }

                    if (hasLeft && hasDown)
                    {
                        var at = ((i + 1) * width) + j - 1;
                        neighborsBuffer.Add(board[at]);
                    }

                    if (hasRight && hasUp)
                    {
                        var at = ((i - 1) * width) + j + 1;
                        neighborsBuffer.Add(board[at]);
                    }

                    if (hasRight && hasDown)
                    {
                        var at = ((i + 1) * width) + j + 1;
                        neighborsBuffer.Add(board[at]);
                    }

                    cell.AddNeighbors(neighborsBuffer);
                }
            }
        }
    }
}