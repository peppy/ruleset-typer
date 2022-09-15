// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Typer.UI
{
    [Cached]
    public class TyperPlayfield : ScrollingPlayfield
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
                new HitBox
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                },
                HitObjectContainer,
            });
        }
    }

    internal class HitBox : CompositeDrawable
    {
        public HitBox()
        {
            Masking = true;
            CornerRadius = 15;
            CornerExponent = 2.5f;

            BorderThickness = 4;
            BorderColour = Color4.White;

            Size = new Vector2(80);

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = OsuColour.Gray(0.1f),
                },
            });
        }
    }
}
