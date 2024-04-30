// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Typer.Objects;
using osu.Game.Rulesets.Typer.Objects.Drawables;
using osu.Game.Rulesets.Typer.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Typer.UI
{
    [Cached]
    public partial class DrawableTyperRuleset : DrawableScrollingRuleset<TyperHitObject>
    {
        public DrawableTyperRuleset(TyperRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
            Direction.Value = ScrollingDirection.Left;
            TimeRange.Value = 10000;
        }

        protected override Playfield CreatePlayfield() => new TyperPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new TyperFramedReplayInputHandler(replay);

        public override DrawableHitObject<TyperHitObject> CreateDrawableRepresentation(TyperHitObject h) => new DrawableTyperHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new TyperInputManager(Ruleset?.RulesetInfo);
    }
}
