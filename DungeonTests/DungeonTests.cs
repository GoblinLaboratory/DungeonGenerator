using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DungeonGenerator;

namespace DungeonTests
{
	[TestClass]
	public class DungeonTests
	{
		[TestMethod]
		public void MinimumSizeLogic()
		{
			Piece[][] Map = CreateDungeon.DungeonSetup(20, 15);
			Map = CreateDungeon.DungeonFill(Map);
			int MinDungSize = 20 * 15 / 2;
			int DungSize = 0;
			Piece Edge = new Piece(false, false, false, false);

			for (int i = 0; i < Map[0].Length; i++)
			{
				for (int j = 0; j < Map.Length; j++)
				{
					if (!Map[j][i].Equals(new Piece()) && !Map[j][i].Equals(Edge))
					{
						DungSize++;
					}
				}
			}
			Assert.IsTrue(DungSize >= MinDungSize);
		}

		[TestMethod]
		public void DungeonHasNoOpenPoints()
		{
			Piece[][] Map = CreateDungeon.DungeonSetup(20, 10);
			Map = CreateDungeon.DungeonFill(Map);
			bool NoOpenPoint = true;
			for (int i = 1; i < Map[0].Length - 2; i++)
			{
				for (int j = 1; j < Map.Length - 2 ; j++)
				{
					Piece Above = Map[j][i - 1];
					Piece Below = Map[j][i + 1];
					Piece Left = Map[j - 1][i];
					Piece Right = Map[j + 1][i];
					Piece cPiece = Map[j][i];

					if ((cPiece.ConnectBottom == true && (Below.Equals(new Piece()) || Below.ConnectTop == false))
						|| (cPiece.ConnectLeft == true && (Left.Equals(new Piece()) || Left.ConnectRight == false))
						|| (cPiece.ConnectRight == true && (Right.Equals(new Piece()) || Right.ConnectLeft == false))
						|| (cPiece.ConnectTop == true && (Above.Equals(new Piece()) || Above.ConnectBottom == false)))
					{
						NoOpenPoint = false;
					}
				}
			}
			Assert.IsTrue(NoOpenPoint);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException), "Inputs must be greater than or equal to 1")]
		public void InputValidationForLessThanOne()
		{
			Piece[][] Map = CreateDungeon.DungeonSetup(-2, 10);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException), "Width and Height of Dungeon cannot both be 1")]
		public void InputValidationForOne()
		{
			Piece[][] Map = CreateDungeon.DungeonSetup(1, 1);
		}

	}
}
