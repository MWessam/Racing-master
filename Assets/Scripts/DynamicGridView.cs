using Nova;
using System;
using System.Collections.Generic;
using Game_Manager.Mediator;
using UnityEngine;

/// <summary>
/// Dynamically change the <see cref="GridView.CrossAxisItemCount"/> of a grid based 
/// on the rendered size of the <see cref="Nova.GridView"/> along its cross axis.
/// </summary>
[RequireComponent(typeof(GridView))]
public class DynamicGrid : MonoBehaviour, IUpdateable
{
    [Tooltip("Assign different cross axis item counts based on the rendered size of the GridView.")]
    public List<GridBreakPoint> BreakPoints = new List<GridBreakPoint>()
    {
        new GridBreakPoint() {MinLength = Length.Zero, MaxLength = Length.FixedValue(100), ItemCount = 1 },
        new GridBreakPoint() {MinLength = Length.FixedValue(100), MaxLength = Length.FixedValue(250), ItemCount = 2 },
        new GridBreakPoint() {MinLength = Length.FixedValue(250), MaxLength = Length.FixedValue(500), ItemCount = 4 },
        new GridBreakPoint() {MinLength = Length.FixedValue(500), MaxLength = Length.FixedValue(float.PositiveInfinity), ItemCount = 8 },
    };

    /// <summary>
    /// The <see cref="Nova.GridView"/> attached to this component's Game Object.
    /// </summary>
    [NonSerialized]
    private GridView gridView = null;

    /// <summary>
    /// The <see cref="Nova.GridView"/> attached to this component's Game Object.
    /// </summary>
    public GridView GridView
    {
        get
        {
            if (gridView == null)
            {
                gridView = GetComponent<GridView>();
            }

            return gridView;
        }
    }

    public void Update()
    {
        if (BreakPoints == null)
        {
            // No breakpoints
            return;
        }

        if (!GridView.CrossAxis.TryGetIndex(out int crossAxis))
        {
            // Cross axis not configured.
            return;
        }

        // Get the rendered size of the grid along the cross axis
        Length.Calculated gridSize = GridView.UIBlock.CalculatedSize[crossAxis];

        // Determine the cross axis item count based on the rendered size of the grid
        for (int i = 0; i < BreakPoints.Count; ++i)
        {
            GridBreakPoint bp = BreakPoints[i];

            if (bp.InRange(gridSize))
            {
                // gridSize is with bp.MinLength and bp.MaxLength,
                // so we update the cross axis item count and exit
                GridView.CrossAxisItemCount = bp.ItemCount;
                break;
            }
        }
    }
}

/// <summary>
/// Configure a min/max <see cref="Length"/> for a given cross axis item count.
/// </summary>
[Serializable]
public struct GridBreakPoint
{
    [Tooltip("The minimum length of the grid along the cross axis for the given item count.")]
    public Length MinLength;
    [Tooltip("The maximum length of the grid along the cross axis for the given item count.")]
    public Length MaxLength;
    [Tooltip("The cross axis item count to assign to the attached GridView when the GridView's length along the cross axis is between MinLength and MaxLength")]
    public int ItemCount;

    /// <summary>
    /// Is the calculated length within <see cref="MinLength"/> and <see cref="MaxLength"/>?
    /// </summary>
    /// <param name="length">The calculated length of the grid along the cross axis.</param>
    public bool InRange(Length.Calculated length) => LessThanOrEqual(MinLength, length) && GreaterThanOrEqual(MaxLength, length);

    /// <summary>
    /// Is the given <see cref="Length"/> configuration, <paramref name="lhs"/>,
    /// less than or equal to the currently calculated value?
    /// </summary>
    /// <param name="lhs">The configured <see cref="Length"/> to check.</param>
    /// <param name="rhs">The calculated <see cref="Length.Calculated"/> to compare against.</param>
    private static bool LessThanOrEqual(Length lhs, Length.Calculated rhs)
    {
        float calc = lhs.Type == LengthType.Value ? rhs.Value : rhs.Percent;

        return lhs.Raw <= calc;
    }

    /// <summary>
    /// Is the given <see cref="Length"/> configuration, <paramref name="lhs"/>,
    /// greater than or equal to the currently calculated value?
    /// </summary>
    /// <param name="lhs">The configured <see cref="Length"/> to check.</param>
    /// <param name="rhs">The calculated <see cref="Length.Calculated"/> to compare against.</param>
    private static bool GreaterThanOrEqual(Length lhs, Length.Calculated rhs)
    {
        float calc = lhs.Type == LengthType.Value ? rhs.Value : rhs.Percent;

        return lhs.Raw >= calc;
    }
}