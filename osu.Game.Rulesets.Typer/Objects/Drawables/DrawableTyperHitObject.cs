// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
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
    public class DrawableTyperHitObject : DrawableHitObject<TyperHitObject>
    {
        private readonly char keyToHit;

        public DrawableTyperHitObject(TyperHitObject hitObject)
            : base(hitObject)
        {
            Size = new Vector2(80);

            Origin = Anchor.CentreLeft;
            Anchor = Anchor.CentreLeft;

            Masking = true;
            CornerRadius = 15;
            CornerExponent = 2.5f;

            keyToHit = (char)RNG.Next('a', 'z' + 1);

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Gray,
                },
                new OsuSpriteText
                {
                    Font = OsuFont.Default.With(size: 52, weight: FontWeight.Bold),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = keyToHit.ToString().ToUpper(),
                }
            });
        }

        private bool correctKey;

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (userTriggered)
            {
                if (!correctKey || Math.Abs(timeOffset) >= 100)
                    ApplyResult(r => r.Type = HitResult.Miss);
                else
                    ApplyResult(r => r.Type = HitResult.Perfect);
            }
            else if (timeOffset >= 100)
                ApplyResult(r => r.Type = HitResult.Miss);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!Result.HasResult)
            {
                correctKey = (e.Key - Key.A == keyToHit - 'a');
                UpdateResult(true);
                return true;
            }

            return base.OnKeyDown(e);
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            const double duration = 800;

            switch (state)
            {
                case ArmedState.Hit:
                    this.ScaleTo(1.8f, duration, Easing.OutQuint);
                    this.FadeOut(duration, Easing.OutQuint).Expire();
                    break;

                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, 100);
                    this.FadeOut(500, Easing.InQuint).Expire();
                    break;
            }
        }
    }
}
