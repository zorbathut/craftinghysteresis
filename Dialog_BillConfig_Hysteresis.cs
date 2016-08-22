using RimWorld;
using Verse;
using Verse.Sound;
using UnityEngine;

namespace CraftingHysteresis
{
	public class Dialog_BillConfig_Hysteresis : Dialog_BillConfig
	{
		private Bill_Production bill;
		
		public Dialog_BillConfig_Hysteresis(Bill_Production bill, IntVec3 billGiverPos) : base(bill, billGiverPos)
		{
			this.bill = bill;
		}
		
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			
			if (bill is Bill_Production_Hysteresis)
			{
				Bill_Production_Hysteresis bph = bill as Bill_Production_Hysteresis;
			
				Rect rect2 = new Rect(0f, 400f, 180f, inRect.height - 400f);
				Listing_Standard listing_Standard = new Listing_Standard(rect2);
				
				listing_Standard.CheckboxLabeled("Pause on completion", ref bph.pauseOnCompletion);
				listing_Standard.CheckboxLabeled("Resume on low stock", ref bph.unpauseOnExhaustion);
				
				if (bph.unpauseOnExhaustion)
				{
					listing_Standard.Gap(12f);
					listing_Standard.Label("Low stock threshold: " + bph.unpauseThreshold);
					listing_Standard.IntSetter(ref bph.unpauseThreshold, 1, "1", 42f);
					listing_Standard.IntAdjuster(ref bph.unpauseThreshold, 1, 1);
					listing_Standard.IntAdjuster(ref bph.unpauseThreshold, 25, 1);
					listing_Standard.IntAdjuster(ref bph.unpauseThreshold, 250, 1);
				}

				listing_Standard.End();
			}
			else if (bill is Bill_ProductionWithUft_Hysteresis)
			{
				Bill_ProductionWithUft_Hysteresis bph = bill as Bill_ProductionWithUft_Hysteresis;
			
				Rect rect2 = new Rect(0f, 400f, 180f, inRect.height - 400f);
				Listing_Standard listing_Standard = new Listing_Standard(rect2);
				
				listing_Standard.CheckboxLabeled("Pause on completion", ref bph.pauseOnCompletion);
				listing_Standard.CheckboxLabeled("Resume on low stock", ref bph.unpauseOnExhaustion);
				
				if (bph.unpauseOnExhaustion)
				{
					listing_Standard.Gap(12f);
					listing_Standard.Label("Low stock threshold: " + bph.unpauseThreshold);
					listing_Standard.IntSetter(ref bph.unpauseThreshold, 1, "1", 42f);
					listing_Standard.IntAdjuster(ref bph.unpauseThreshold, 1, 1);
					listing_Standard.IntAdjuster(ref bph.unpauseThreshold, 25, 1);
					listing_Standard.IntAdjuster(ref bph.unpauseThreshold, 250, 1);
				}

				listing_Standard.End();
			}
		}
	}
}
