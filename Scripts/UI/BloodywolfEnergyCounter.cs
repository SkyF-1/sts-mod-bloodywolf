using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace StsModBloodywolf.Scripts.UI;

public partial class BloodywolfEnergyCounter : NEnergyCounter
{
	private const double GhostLifetime = 0.22;
	private const float GhostScaleMultiplier = 1.7f;
	private const float GhostRotationRange = 6f;
	private const float LightningSizeRatio = 0.375f;

	private Label _label = null!;
	private Control _layers = null!;
	private Control _rotationLayers = null!;
	private Sprite2D _lightning = null!;
	private Sprite2D _lightningGhost = null!;
	private HoverTip? _hoverTip;
	private Player? _player;
	private Tween? _ghostTween;
	private int _energy;

	public override void _EnterTree()
	{
		// NEnergyCounter subscribes to combat events in _EnterTree, but its
		// lifecycle requires scene resources this counter intentionally avoids.
	}

	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
		_layers = GetNode<Control>("%Layers");
		_rotationLayers = GetNode<Control>("%RotationLayers");
		_lightning = GetNode<Sprite2D>("%Lightning");
		_lightningGhost = GetNode<Sprite2D>("%LightningGhost");
		_lightningGhost.Visible = false;

		ConfigureLightningLayout();
		AttachToLocalPlayer();
		CreateHoverTip();
		CombatManager.Instance.StateTracker.CombatStateChanged += OnCombatStateChanged;
		RefreshLabel();
	}

	public override void _Process(double delta)
	{
		float rotationSpeed = _energy == 0 ? 2f : 5f;
		for (int i = 0; i < _rotationLayers.GetChildCount(); i++)
		{
			_rotationLayers.GetChild<Control>(i).RotationDegrees +=
				(float)delta * rotationSpeed * (i + 1);
		}
	}

	public override void _ExitTree()
	{
		CombatManager.Instance.StateTracker.CombatStateChanged -= OnCombatStateChanged;

		if (_player?.PlayerCombatState != null)
			_player.PlayerCombatState.EnergyChanged -= OnEnergyChanged;

		_ghostTween?.Kill();
		_ghostTween = null;
		NHoverTipSet.Remove(this);
	}

	private void AttachToLocalPlayer()
	{
		var combatState = CombatManager.Instance.DebugOnlyGetState();
		_player = LocalContext.GetMe(combatState);

		var combatStateForPlayer = _player?.PlayerCombatState;
		if (combatStateForPlayer == null)
			return;

		_energy = combatStateForPlayer.Energy;
		combatStateForPlayer.EnergyChanged += OnEnergyChanged;
	}

	private void CreateHoverTip()
	{
		if (_player == null)
			return;

		var description = new LocString(
			"static_hover_tips",
			"ENERGY_COUNT.description");
		description.Add(
			"energyPrefix",
			EnergyIconHelper.GetPrefix(_player.Character.CardPool));
		_hoverTip = new HoverTip(
			new LocString("static_hover_tips", "ENERGY_COUNT.title"),
			description);

		Connect(Control.SignalName.MouseEntered, Callable.From(OnHovered));
		Connect(Control.SignalName.MouseExited, Callable.From(OnUnhovered));
	}

	private void OnHovered()
	{
		if (_hoverTip == null)
			return;

		NHoverTipSet.CreateAndShow(this, _hoverTip)
			?.SetGlobalPosition(GlobalPosition + new Vector2(-70f, -200f));
	}

	private void OnUnhovered()
	{
		NHoverTipSet.Remove(this);
	}

	private void OnCombatStateChanged(CombatState _)
	{
		RefreshLabel();
	}

	private void OnEnergyChanged(int oldEnergy, int newEnergy)
	{
		_energy = newEnergy;
		RefreshLabel();

		if (oldEnergy < newEnergy)
			SpawnLightningGhost();
	}

	private void RefreshLabel()
	{
		if (_player is not { } player ||
			player.PlayerCombatState is not { } combatState ||
			player.Creature.CombatState == null)
		{
			return;
		}

		_energy = combatState.Energy;
		bool isEmpty = _energy == 0;

		_label.Text = $"{_energy}/{combatState.MaxEnergy}";
		_label.AddThemeColorOverride(
			"font_color",
			isEmpty
				? new Color(0.92f, 0.25f, 0.22f)
				: new Color(1f, 0.964706f, 0.886275f));
		_label.AddThemeColorOverride(
			"font_outline_color",
			isEmpty
				? new Color(0.3f, 0.0759f, 0.051f)
				: new Color(0.0862749f, 0.242979f, 0.393015f));
		_layers.Modulate = isEmpty
			? new Color(0.43f, 0.43f, 0.43f)
			: Colors.White;
	}

	private void ConfigureLightningLayout()
	{
		float counterExtent = Mathf.Max(1f, Mathf.Min(Size.X, Size.Y));
		float textureExtent = 1f;
		if (_lightning.Texture is { } texture)
		{
			textureExtent = Mathf.Max(
				1f,
				Mathf.Max(texture.GetWidth(), texture.GetHeight()));
		}

		float scale = Mathf.Min(
			1f,
			counterExtent * LightningSizeRatio / textureExtent);
		float cornerOffset = counterExtent * 0.5f / Mathf.Sqrt(2f);

		_lightning.Scale = Vector2.One * scale;
		_lightning.Position = Size * 0.5f + new Vector2(cornerOffset, cornerOffset);
	}

	private void SpawnLightningGhost()
	{
		_ghostTween?.Kill();

		float startRotation = _lightning.GlobalRotationDegrees;
		Vector2 startScale = _lightning.GlobalScale;
		float randomRotation = (float)GD.RandRange(
			-GhostRotationRange,
			GhostRotationRange);

		_lightningGhost.GlobalPosition = _lightning.GlobalPosition;
		_lightningGhost.GlobalRotationDegrees = startRotation;
		_lightningGhost.Scale = startScale;
		_lightningGhost.Modulate = Colors.White;
		_lightningGhost.Visible = true;

		_ghostTween = CreateTween();
		_ghostTween.SetParallel();
		_ghostTween.TweenProperty(
				_lightningGhost,
				"scale",
				startScale * GhostScaleMultiplier,
				GhostLifetime)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Cubic);
		_ghostTween.TweenProperty(
				_lightningGhost,
				"rotation_degrees",
				startRotation + randomRotation,
				GhostLifetime)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Cubic);
		_ghostTween.TweenProperty(
				_lightningGhost,
				"modulate:a",
				0.0f,
				GhostLifetime)
			.SetEase(Tween.EaseType.In)
			.SetTrans(Tween.TransitionType.Quad);
		_ghostTween.Finished += () =>
		{
			_lightningGhost.Visible = false;
			_lightningGhost.Modulate = Colors.White;
		};
	}
}
