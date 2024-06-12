// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private readonly Random beatmapSeed;

        public DrawableTyperRuleset(TyperRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
            Direction.Value = ScrollingDirection.Left;
            TimeRange.Value = 10000;

            beatmapSeed = acquireBeatmapSeed(beatmap.BeatmapInfo.MD5Hash);
        }

        protected override Playfield CreatePlayfield() => new TyperPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new TyperFramedReplayInputHandler(replay);

        public override DrawableHitObject<TyperHitObject> CreateDrawableRepresentation(TyperHitObject h) => new DrawableTyperHitObject(h, beatmapSeed);

        protected override PassThroughInputManager CreateInputManager() => new TyperInputManager(Ruleset?.RulesetInfo);

        private static Random acquireBeatmapSeed(string beatmapHash)
        {
            byte[] bytes = Convert.FromHexString(beatmapHash);

            Debug.Assert(bytes.Length == 16);

            int[] chunks = new int[4];

            for (int i = 0; i < 4; i++)
            {
                chunks[i] = BitConverter.ToInt32(bytes, i * 4);
            }

            int seed = chunks.Aggregate(0, (a, e) => a ^ e);

            return new Random(seed);
        }
    }
}
