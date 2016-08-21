using RimWorld;
using Verse;
using UnityEngine;

namespace CraftingHysteresis
{
	public static class BillUtility_Detour
	{
		public static Bill MakeNewBill(this RecipeDef recipe)
		{
			if (recipe.UsesUnfinishedThing)
			{
				return new Bill_ProductionWithUft_Hysteresis(recipe);
			}
			
			return new Bill_Production_Hysteresis(recipe);
		}
	}
}
