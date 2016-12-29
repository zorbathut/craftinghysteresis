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

        public void IntAdjusterPack(ref int val, Listing listing, int min = 0)
        {
            Rect rect = listing.GetRect(24f);
            rect.width = 42f;
            int mult = Event.current.shift ? 10 : 1;
            if (Widgets.ButtonText(rect, "-" + 1, true, false, true))
            {
                SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                val -= 1 * mult;
                if (val < min)
                {
                    val = min;
                }
            }
            rect.x += rect.width + 2f;
            if (Widgets.ButtonText(rect, "+" + 1, true, false, true))
            {
                SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                val += 1 * mult;
                if (val < min)
                {
                    val = min;
                }
            }
            rect.x += rect.width + 2f;
            if (Widgets.ButtonText(rect, "-" + 25, true, false, true))
            {
                SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                val -= 25 * mult;
                if (val < min)
                {
                    val = min;
                }
            }
            rect.x += rect.width + 2f;
            if (Widgets.ButtonText(rect, "+" + 25, true, false, true))
            {
                SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                val += 25 * mult;
                if (val < min)
                {
                    val = min;
                }
            }
            listing.Gap(2f);
        }

        public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			
			if (bill is Bill_Production_Hysteresis)
			{
				Bill_Production_Hysteresis bph = bill as Bill_Production_Hysteresis;

                float vpos = 420f;
				Rect rect2 = new Rect(0f, vpos, 180f, inRect.height - vpos);
				Listing_Standard listing_Standard = new Listing_Standard(rect2);
				
				listing_Standard.CheckboxLabeled("Pause on completion", ref bph.pauseOnCompletion);

                if (bill.recipe.WorkerCounter.CanCountProducts(bill))
                {
                    listing_Standard.CheckboxLabeled("Resume on low stock", ref bph.unpauseOnExhaustion);

                    if (bph.unpauseOnExhaustion)
                    {
                        listing_Standard.Gap(12f);
                        listing_Standard.Label("Low stock threshold: " + bph.unpauseThreshold);
                        listing_Standard.IntSetter(ref bph.unpauseThreshold, 1, "1", 42f);
                        IntAdjusterPack(ref bph.unpauseThreshold, listing_Standard, 1);
                    }
                }

				listing_Standard.End();
			}
			else if (bill is Bill_ProductionWithUft_Hysteresis)
			{
				Bill_ProductionWithUft_Hysteresis bph = bill as Bill_ProductionWithUft_Hysteresis;
			
				Rect rect2 = new Rect(0f, 400f, 180f, inRect.height - 400f);
				Listing_Standard listing_Standard = new Listing_Standard(rect2);
				
				listing_Standard.CheckboxLabeled("Pause on completion", ref bph.pauseOnCompletion);

                if (bill.recipe.WorkerCounter.CanCountProducts(bill))
                {
                    listing_Standard.CheckboxLabeled("Resume on low stock", ref bph.unpauseOnExhaustion);

                    if (bph.unpauseOnExhaustion)
                    {
                        listing_Standard.Gap(12f);
                        listing_Standard.Label("Low stock threshold: " + bph.unpauseThreshold);
                        listing_Standard.IntSetter(ref bph.unpauseThreshold, 1, "1", 42f);
                        IntAdjusterPack(ref bph.unpauseThreshold, listing_Standard, 1);
                    }
                }

				listing_Standard.End();
			}
		}
	}
}
