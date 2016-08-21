using RimWorld;
using Verse;
using Verse.Sound;
using UnityEngine;

namespace CraftingHysteresis
{
	public class Dialog_BillConfig_Hysteresis : Dialog_BillConfig
	{
		private Bill_Production_Hysteresis bill;
		
		public Dialog_BillConfig_Hysteresis(Bill_Production_Hysteresis bill, IntVec3 billGiverPos) : base(bill, billGiverPos)
		{
			this.bill = bill;
		}
		
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			
			Rect rect2 = new Rect(0f, 400f, 180f, inRect.height - 400f);
			Listing_Standard listing_Standard = new Listing_Standard(rect2);
			
			listing_Standard.CheckboxLabeled("Pause on completion", ref bill.pauseOnCompletion);
			listing_Standard.CheckboxLabeled("Resume on low stock", ref bill.unpauseOnExhaustion);
			
			if (bill.unpauseOnExhaustion)
			{
				listing_Standard.Gap(12f);
				listing_Standard.Label("Low stock threshold: " + this.bill.unpauseThreshold);
				listing_Standard.IntSetter(ref this.bill.unpauseThreshold, 1, "1", 42f);
				listing_Standard.IntAdjuster(ref this.bill.unpauseThreshold, 1, 1);
				listing_Standard.IntAdjuster(ref this.bill.unpauseThreshold, 25, 1);
				listing_Standard.IntAdjuster(ref this.bill.unpauseThreshold, 250, 1);
			}
		}
	}
}
