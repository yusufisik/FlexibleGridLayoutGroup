﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YusufISIK.CustomUIComponents
{
    [AddComponentMenu("Layout/Flexible Grid Layout Group")]
    public class FlexibleGridLayoutGroup : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }

        public FitType fitType;

        public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;

        public bool fitX = true;
        public bool fitY = true;
        public bool square = false;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            Logic();

        }

        public override void CalculateLayoutInputVertical()
        {
            Logic();
        }

        public override void SetLayoutHorizontal()
        {

        }

        public override void SetLayoutVertical()
        {

        }

        private void Logic()
        {
            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
            {
                fitX = true;
                fitY = true;

                float sqrRt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }

            if (fitType == FitType.Width || fitType == FitType.FixedColumns)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }

            if (fitType == FitType.Height || fitType == FitType.FixedRows)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }

            if (fitX && fitY) square = false;

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
            float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

            cellSize.x = fitX ? cellWidth : square? (parentHeight / rows) : cellSize.x;
            cellSize.y = fitY ? cellHeight : square? (parentWidth / columns) : cellSize.y;

            if (fitType == FitType.FixedColumns && square) rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, cellSize.y * rows); //content size Y fit
            
            int rowCount = 0;
            int columnCount = 0;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                rowCount = i / columns;
                columnCount = i % columns;

                var item = rectChildren[i];

                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }
    }
}
