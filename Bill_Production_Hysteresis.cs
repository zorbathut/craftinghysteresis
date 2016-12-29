using RimWorld;
using Verse;
using Verse.Sound;
using UnityEngine;

namespace CraftingHysteresis
{
	public class Bill_Production_Hysteresis : Bill_Production
	{
		protected override string StatusString
		{
			get
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
		}
		
		public bool paused = false;
		
		public bool pauseOnCompletion = false;
		public bool unpauseOnExhaustion = false;
		public int unpauseThreshold = 5;
		
		public Bill_Production_Hysteresis()
		{
			
		}
		
		public Bill_Production_Hysteresis(RecipeDef recipe) : base(recipe)
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
			
			if (unpauseOnExhaustion && recipe.WorkerCounter.CanCountProducts(this) && recipe.WorkerCounter.CountProducts(this) < unpauseThreshold)
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
			if (widgetRow.ButtonText(this.repeatMode.GetLabel().PadRight(20), null, true, false))
			{
				BillRepeatModeUtility.MakeConfigFloatMenu(this);
			}
			if (widgetRow.ButtonIcon((Texture2D)CraftingHysteresis.Bootstrap.ButtonPlus.GetValue(null), null))
			{
				if (this.repeatMode == BillRepeatMode.Forever)
				{
					this.repeatMode = BillRepeatMode.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatMode.TargetCount)
				{
					this.targetCount += this.recipe.targetCountAdjustment;
				}
				else if (this.repeatMode == BillRepeatMode.RepeatCount)
				{
					this.repeatCount++;
				}
				SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                if (TutorSystem.TutorialMode && this.repeatMode == BillRepeatMode.RepeatCount)
                {
                    TutorSystem.Notify_Event(this.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount);
                }
            }
			if (widgetRow.ButtonIcon((Texture2D)CraftingHysteresis.Bootstrap.ButtonMinus.GetValue(null), null))
			{
				if (this.repeatMode == BillRepeatMode.Forever)
				{
					this.repeatMode = BillRepeatMode.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatMode.TargetCount)
				{
					this.targetCount = Mathf.Max(0, this.targetCount - this.recipe.targetCountAdjustment);
				}
				else if (this.repeatMode == BillRepeatMode.RepeatCount)
				{
					this.repeatCount = Mathf.Max(0, this.repeatCount - 1);
				}
				SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                if (TutorSystem.TutorialMode && this.repeatMode == BillRepeatMode.RepeatCount)
                {
                    TutorSystem.Notify_Event(this.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount);
                }
            }
		}
	}
}

