﻿using UnityEngine;

public class HexagonPiece : MonoBehaviour
{
	public enum Corner { BottomLeft, Left, TopLeft, TopRight, Right, BottomRight };

#pragma warning disable 0649
	[SerializeField]
	private SpriteRenderer spriteRenderer;
#pragma warning restore 0649

	public int X { get; private set; }
	public int Y { get; private set; }
	public int ColorIndex { get; private set; }

	public void SetPosition( int x, int y )
	{
		X = x;
		Y = y;
	}

	public void SetColor( int colorIndex, Color color )
	{
		ColorIndex = colorIndex;
		spriteRenderer.color = color;
	}

	public void SetSelected( bool isSelected )
	{
		spriteRenderer.sortingOrder = isSelected ? 1 : 0;
	}

	public Corner GetClosestCorner( Vector2 localPoint )
	{
		bool leftSide = localPoint.x < 0f;
		Vector2 p1 = new Vector2( GridManager.PIECE_WIDTH * ( leftSide ? -0.25f : 0.25f ), GridManager.PIECE_HEIGHT * -0.5f );
		Vector2 p2 = new Vector2( GridManager.PIECE_WIDTH * ( leftSide ? -0.5f : 0.5f ), 0f );
		Vector2 p3 = new Vector2( GridManager.PIECE_WIDTH * ( leftSide ? -0.25f : 0.25f ), GridManager.PIECE_HEIGHT * 0.5f );

		if( ( p1 - localPoint ).sqrMagnitude < ( p2 - localPoint ).sqrMagnitude )
		{
			if( ( p1 - localPoint ).sqrMagnitude < ( p3 - localPoint ).sqrMagnitude )
				return leftSide ? Corner.BottomLeft : Corner.BottomRight;

			return leftSide ? Corner.TopLeft : Corner.TopRight;
		}

		if( ( p2 - localPoint ).sqrMagnitude < ( p3 - localPoint ).sqrMagnitude )
			return leftSide ? Corner.Left : Corner.Right;

		return leftSide ? Corner.TopLeft : Corner.TopRight;
	}

	public Corner GetPickableCorner( Corner corner )
	{
		if( X == 0 )
		{
			if( corner == Corner.BottomLeft )
				corner = Corner.BottomRight;
			else if( corner == Corner.Left )
				corner = Corner.Right;
			else if( corner == Corner.TopLeft )
				corner = Corner.TopRight;
		}
		else if( X == GridManager.Instance.Width - 1 )
		{
			if( corner == Corner.BottomRight )
				corner = Corner.BottomLeft;
			else if( corner == Corner.Right )
				corner = Corner.Left;
			else if( corner == Corner.TopRight )
				corner = Corner.TopLeft;
		}

		if( Y == 0 )
		{
			if( corner == Corner.BottomLeft )
				corner = Corner.Left;
			else if( corner == Corner.BottomRight )
				corner = Corner.Right;

			if( X % 2 == 0 )
			{
				if( corner == Corner.Left )
					corner = Corner.TopLeft;
				else if( corner == Corner.Right )
					corner = Corner.TopRight;
			}
		}
		else if( Y == GridManager.Instance.Height - 1 )
		{
			if( corner == Corner.TopLeft )
				corner = Corner.Left;
			else if( corner == Corner.TopRight )
				corner = Corner.Right;

			if( X % 2 == 1 )
			{
				if( corner == Corner.Left )
					corner = Corner.BottomLeft;
				else if( corner == Corner.Right )
					corner = Corner.BottomRight;
			}
		}

		return corner;
	}
}