// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;

namespace osu.Game.Rulesets.Typer.UI
{
    [Cached]
    public partial class TyperPlayfield : ScrollingPlayfield
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            Height = 80;

            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;

            Padding = new MarginPadding { Left = 100 };

            AddRangeInternal(new Drawable[]
            {
                HitObjectContainer,
                new HitBox
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                },
            });
        }
    }

    internal partial class HitBox : CompositeDrawable
    {
        private readonly OneBox rotatingBox;

        public HitBox()
        {
            InternalChildren = new Drawable[]
            {
                new OneBox(),
                rotatingBox = new OneBox
                {
                    Scale = new Vector2(2f),
                    Alpha = 0.3f,
                }
            };
        }

        protected override void Update()
        {
            base.Update();

            rotatingBox.Rotation += (float)Clock.ElapsedFrameTime * 0.1f;
        }

        public partial class OneBox : CompositeDrawable
        {
            public OneBox()
            {
                Masking = true;
                CornerRadius = 15;
                CornerExponent = 2.5f;

                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                Blending = BlendingParameters.Additive;

                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Glow,
                    Radius = 5,
                    Colour = Color4Extensions.FromHex("483D8B"),
                    Hollow = true,
                };

                Size = new Vector2(80);

                AddRangeInternal(new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4Extensions.FromHex("483D8B").Opacity(0.5f),
                    },
                });
            }
        }
    }
}
