// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace osu.Game.Rulesets.Typer.Objects.Drawables
{
    public partial class DrawableTyperHitObject : DrawableHitObject<TyperHitObject>
    {
        private const double allowable_error = 150;

        private bool wasCorrectKey;

        private readonly Container keyContent;

        private readonly char keyToHit;

        public DrawableTyperHitObject(TyperHitObject hitObject)
            : base(hitObject)
        {
            Size = new Vector2(80);

            Origin = Anchor.CentreLeft;
            Anchor = Anchor.CentreLeft;

            keyToHit = (char)RNG.Next('a', 'z' + 1);

            AddRangeInternal(new Drawable[]
            {
                keyContent = new Container
                {
                    Masking = true,
                    CornerRadius = 15,
                    CornerExponent = 2.5f,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Radius = 8,
                        Colour = Color4Extensions.FromHex("483D8B"),
                        Type = EdgeEffectType.Shadow,
                    },
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = ColourInfo.GradientVertical(
                                Color4Extensions.FromHex("5F6A6A"),
                                Color4Extensions.FromHex("D8BFD8")
                            )
                        },
                        new OsuSpriteText
                        {
                            Font = OsuFont.Default.With(size: 52, weight: FontWeight.Bold),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = keyToHit.ToString().ToUpper(),
                        }
                    }
                },
            });
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (userTriggered)
            {
                if (Math.Abs(timeOffset) >= allowable_error)
                    return;

                if (!wasCorrectKey)
                    ApplyResult(r => r.Type = HitResult.Miss);
                else
                    ApplyResult(r => r.Type = HitResult.Perfect);
            }
            else if (timeOffset >= allowable_error)
            {
                ApplyResult(r => r.Type = HitResult.Miss);
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            bool correctKey = e.Key - Key.A == keyToHit - 'a';

            if (!Result.HasResult)
            {
                wasCorrectKey = correctKey;

                if (wasCorrectKey)
                {
                    keyContent.ScaleTo(0.9f, 200, Easing.OutElastic);
                    keyContent.RotateTo(RNG.NextSingle(30) - 15, 500, Easing.OutElastic);
                }

                UpdateResult(true);

                return wasCorrectKey;
            }

            return base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            bool correctKey = e.Key - Key.A == keyToHit - 'a';

            if (State.Value != ArmedState.Hit && correctKey)
            {
                keyContent.ScaleTo(1, 300, Easing.OutQuint);
                keyContent.RotateTo(0, 300, Easing.OutQuint);
            }

            base.OnKeyUp(e);
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            const double duration = 800;

            switch (state)
            {
                case ArmedState.Hit:
                    keyContent.ScaleTo(5f, duration, Easing.OutQuint);

                    this.FadeOut(duration, Easing.OutQuint).Expire();
                    break;

                case ArmedState.Miss:
                    keyContent.FadeColour(Color4.Red, 100);
                    keyContent.MoveToY(100, 1000, Easing.In);
                    keyContent.RotateTo(RNG.NextSingle(30) - 15, 1000, Easing.In);

                    this.FadeOut(500, Easing.InQuint).Expire();
                    break;
            }
        }
    }
}
