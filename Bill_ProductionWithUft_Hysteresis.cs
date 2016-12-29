using RimWorld;
using Verse;
using Verse.Sound;
using UnityEngine;

namespace CraftingHysteresis
{
	public class Bill_ProductionWithUft_Hysteresis : Bill_ProductionWithUft
	{
		protected override string StatusString
		{
			get
			{
				if (this.BoundWorker == null)
				{
					if (paused)
					{
						return "Paused";
					}
					else
					{
						return null;
					}
				}
				
				return "BoundWorkerIs".Translate(new object[]
				{
					this.BoundWorker.NameStringShort
				}) + (paused ? " - " + "CraftingHysteresis.Paused".Translate() : "");
			}
		}
		
		// Everything below this line is straight-up copypasted from Bill_Production_Hysteresis
		// (except for a single clause of BoundUft)
		// Yeah, I know. Eww.
		
		public bool paused = false;
		
		public bool pauseOnCompletion = false;
		public bool unpauseOnExhaustion = false;
		public int unpauseThreshold = 5;
		
		public Bill_ProductionWithUft_Hysteresis()
		{
			
		}
		
		public Bill_ProductionWithUft_Hysteresis(RecipeDef recipe) : base(recipe)
		{
			
		}
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.LookValue<bool>(ref paused, "paused", false, false);
			Scribe_Values.LookValue<bool>(ref pauseOnCompletion, "pauseOnCompletion", false, false);
			Scribe_Values.LookValue<bool>(ref unpauseOnExhaustion, "unpauseOnExhaustion", false, false);
			Scribe_Values.LookValue<int>(ref unpauseThreshold, "unpauseThreshold", 0, false);
		}
		
		public override bool ShouldDoNow()
		{
			bool baseShouldDoNow = base.ShouldDoNow();
			
			if (pauseOnCompletion && !suspended && !baseShouldDoNow)
			{
				paused = true;
			}
			
			if (unpauseOnExhaustion && this.recipe.WorkerCounter.CountProducts(this) < unpauseThreshold)
			{
				paused = false;
			}
			
			if (paused)
			{
				return false;
			}
			
			return baseShouldDoNow;
		}
		
		protected override void DoConfigInterface(Rect baseRect, Color baseColor)
		{
			Rect rect = new Rect(28f, 32f, 100f, 30f);
			GUI.color = new Color(1f, 1f, 1f, 0.65f);
			Widgets.Label(rect, this.RepeatInfoText);
			GUI.color = baseColor;
			WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Details".Translate() + "...", null, true, false))
			{
				Find.WindowStack.Add(new Dialog_BillConfig_Hysteresis(this, ((Thing)this.billStack.billGiver).Position));
			}
            Bill_Production_Hysteresis.DrawStandardControls(this, widgetRow);
		}
	}
}
