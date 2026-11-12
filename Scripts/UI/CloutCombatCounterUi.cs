using System.Linq;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Rooms;
using StsModBloodywolf.Scripts.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace StsModBloodywolf.Scripts.UI;

public partial class CloutCombatCounterUi : Control
{
    private const double CombatTransitionDuration = 0.6;
    private static readonly Vector2 CombatTransitionOffset = new(-480f, 128f);

    [Export]
    public Vector2 IndicatorOffset { get; set; } = new(128, 0);

    private Label _amountLabel = null!;
    private Control _backRotationLayers = null!;
    private Control _frontRotationLayers = null!;
    private SecondaryResourceHoverTipBinder? _hoverTipBinder;
    private TextureRect _icon = null!;
    private int _lastAmount = int.MinValue;
    private Player? _player;
    private Tween? _combatTransitionTween;
    private Vector2 _combatTransitionStartPosition;
    private Vector2 _combatTransitionTargetPosition;
    private bool _shouldShowInCombat;
    private bool _hasPlayedCombatIntro;
    private bool _isPlayingCombatIntro;
    private bool _isLeavingCombat;

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Ignore;
        _amountLabel = GetNode<Label>("%AmountLabel");
        _backRotationLayers = GetNode<Control>("%BackRotationLayers");
        _frontRotationLayers = GetNode<Control>("%FrontRotationLayers");
        _icon = GetNode<TextureRect>("%Icon");
        var hoverArea = GetNode<Control>("%HoverArea");
        _amountLabel.ApplyLocaleFontSubstitution(
            FontType.Regular,
            ThemeConstants.Label.Font);

        _hoverTipBinder = SecondaryResourceHoverTipBinder.Bind(
            hoverArea,
            BloodywolfResources.CloutDefinition,
            GetDisplayedAmount,
            GetMaximumAmount);
        CombatManager.Instance.CombatSetUp += OnCombatSetUp;
        CombatManager.Instance.CombatEnded += OnCombatEnded;
    }

    public override void _ExitTree()
    {
        CombatManager.Instance.CombatSetUp -= OnCombatSetUp;
        CombatManager.Instance.CombatEnded -= OnCombatEnded;
        _combatTransitionTween?.Kill();
        _combatTransitionTween = null;
        _hoverTipBinder?.Hide();
    }

    public override void _Process(double delta)
    {
        if (!Visible || _player == null)
            return;

        int amount = GetDisplayedAmount();
        UpdateAmountVisuals(amount);

        float speed = amount == 0 ? 5f : 30f;
        RotateLayers(_backRotationLayers, (float)delta * speed);
        RotateLayers(_frontRotationLayers, (float)delta * speed);
    }

    public void Refresh(
        SecondaryResourceCombatUiContext<NCombatUi, CloutCombatCounterUi> context)
    {
        _player = context.Player;
        _shouldShowInCombat =
            context.Player != null &&
            context.VisibleDefinitions.Any(definition =>
                definition.Id == BloodywolfResources.CloutId);

        if (_isLeavingCombat)
            return;

        Visible = _shouldShowInCombat;

        if (Visible)
            UpdateAmountVisuals(GetDisplayedAmount());
        else
            _hoverTipBinder?.Hide();

        _hoverTipBinder?.Refresh();

        var targetPosition = Position;
        var energyCounter = context.Parent.GetNodeOrNull<Control>(
            "%EnergyCounterContainer");
        if (energyCounter != null)
            targetPosition = energyCounter.Position + IndicatorOffset;

        if (Visible && !_hasPlayedCombatIntro)
        {
            PlayCombatIntroAnimation(targetPosition);
        }
        else if (!_isPlayingCombatIntro)
        {
            Position = targetPosition;
        }
    }

    private void OnCombatSetUp(CombatState _)
    {
        _combatTransitionTween?.Kill();
        _combatTransitionTween = null;
        _hasPlayedCombatIntro = false;
        _isPlayingCombatIntro = false;
        _isLeavingCombat = false;
    }

    private void OnCombatEnded(CombatRoom _)
    {
        if (_player == null || !_shouldShowInCombat)
            return;

        _isLeavingCombat = true;
        _hoverTipBinder?.Hide();
        Callable.From(PlayCombatExitAnimation).CallDeferred();
    }

    private void PlayCombatIntroAnimation(Vector2 targetPosition)
    {
        _combatTransitionTween?.Kill();
        _hasPlayedCombatIntro = true;
        _isPlayingCombatIntro = true;
        _combatTransitionTargetPosition = targetPosition;
        Position = targetPosition + CombatTransitionOffset;
        Visible = true;

        _combatTransitionTween = CreateTween();
        _combatTransitionTween
            .TweenProperty(
                this,
                "position",
                targetPosition,
                CombatTransitionDuration)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Expo);
        _combatTransitionTween.Finished += OnCombatIntroFinished;
    }

    private void OnCombatIntroFinished()
    {
        _combatTransitionTween = null;
        _isPlayingCombatIntro = false;
        Position = _combatTransitionTargetPosition;
    }

    private void PlayCombatExitAnimation()
    {
        if (!IsInsideTree() || !_isLeavingCombat || _player == null)
            return;

        _combatTransitionTween?.Kill();
        _isPlayingCombatIntro = false;
        _combatTransitionStartPosition = Position;
        Visible = true;

        _combatTransitionTween = CreateTween();
        _combatTransitionTween
            .TweenProperty(
                this,
                "position",
                _combatTransitionStartPosition + CombatTransitionOffset,
                CombatTransitionDuration)
            .SetEase(Tween.EaseType.In)
            .SetTrans(Tween.TransitionType.Back);
        _combatTransitionTween.Finished += OnCombatExitFinished;
    }

    private void OnCombatExitFinished()
    {
        _combatTransitionTween = null;
        Visible = false;
        Position = _combatTransitionStartPosition;
        _isLeavingCombat = false;
    }

    private int GetDisplayedAmount()
    {
        return _player == null ? 0 : BloodywolfResources.GetClout(_player);
    }

    private int? GetMaximumAmount()
    {
        return _player == null
            ? null
            : SecondaryResourceCmd.GetMax(_player, BloodywolfResources.CloutId);
    }

    private void UpdateAmountVisuals(int amount)
    {
        if (_lastAmount == amount)
            return;

        _lastAmount = amount;
        _amountLabel.Text = amount.ToString();
        _amountLabel.AddThemeColorOverride(
            "font_color",
            amount == 0 ? StsColors.red : StsColors.cream);
        _icon.Modulate = amount == 0
            ? new Color(0.58f, 0.58f, 0.58f, 1f)
            : Colors.White;
        _hoverTipBinder?.Refresh();
    }

    private static void RotateLayers(Control container, float baseSpeed)
    {
        for (int i = 0; i < container.GetChildCount(); i++)
        {
            var layer = container.GetChild<Control>(i);
            layer.RotationDegrees += baseSpeed * (i + 1);
        }
    }
}
