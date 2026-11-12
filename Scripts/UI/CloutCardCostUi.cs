using System.Linq;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Nodes.Cards;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.UI;

public partial class CloutCardCostUi : Control
{
    [Export]
    public Vector2 CardCostOffset { get; set; } = new(-20, 48);

    private Label _costLabel = null!;

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Ignore;
        _costLabel = GetNode<Label>("%CostLabel");
        _costLabel.ApplyLocaleFontSubstitution(
            FontType.Regular,
            ThemeConstants.Label.Font);
    }

    public void Refresh(
        SecondaryResourceCardUiContext<NCard, CloutCardCostUi> context)
    {
        var line = context.Plan.Lines.FirstOrDefault(candidate =>
            candidate.ResourceId == BloodywolfResources.CloutId);

        Visible = line != null;
        if (line == null)
            return;

        _costLabel.Text = line.CostsX ? "X" : line.Cost.ToString();
        var color = SecondaryResourceCardCostHelper.GetCostColor(
            line,
            context.PileType,
            context.PreviewMode);
        if (context.PreviewMode != CardPreviewMode.Upgrade &&
            color is SecondaryResourceCardCostColor.Increased
                or SecondaryResourceCardCostColor.Decreased)
        {
            color = SecondaryResourceCardCostColor.Unmodified;
        }

        ApplyCostColors(color);

        var energyIcon = context.Parent.GetNodeOrNull<TextureRect>("%EnergyIcon");
        if (energyIcon != null)
            Position = energyIcon.Position + CardCostOffset;

        SecondaryResourceCardUiLayout.ReserveVanillaStarCostSlot(context.Parent);
    }

    private void ApplyCostColors(SecondaryResourceCardCostColor color)
    {
        var (fontColor, outlineColor) = color switch
        {
            SecondaryResourceCardCostColor.Unmodified =>
                (StsColors.cream, StsColors.defaultStarCostOutline),
            SecondaryResourceCardCostColor.Increased =>
                (StsColors.energyBlue, StsColors.energyBlueOutline),
            SecondaryResourceCardCostColor.Decreased =>
                (StsColors.green, StsColors.energyGreenOutline),
            SecondaryResourceCardCostColor.InsufficientResources =>
                (StsColors.red, StsColors.unplayableEnergyCostOutline),
            SecondaryResourceCardCostColor.ShortfallPlayable =>
                (StsColors.energyBlue, StsColors.energyBlueOutline),
            SecondaryResourceCardCostColor.OptionalUnavailable =>
                (StsColors.gray, StsColors.defaultStarCostOutline),
            _ => (StsColors.cream, StsColors.defaultStarCostOutline),
        };

        _costLabel.AddThemeColorOverride(
            "font_color",
            fontColor);
        _costLabel.AddThemeColorOverride(
            "font_outline_color",
            outlineColor);
    }
}
