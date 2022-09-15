// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Typer.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Typer.Replays
{
    public class TyperAutoGenerator : AutoGenerator<TyperReplayFrame>
    {
        public new Beatmap<TyperHitObject> Beatmap => (Beatmap<TyperHitObject>)base.Beatmap;

        public TyperAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        protected override void GenerateFrames()
        {
            Frames.Add(new TyperReplayFrame());

            foreach (TyperHitObject hitObject in Beatmap.HitObjects)
            {
                Frames.Add(new TyperReplayFrame
                {
                    Time = hitObject.StartTime
                    // todo: add required inputs and extra frames.
                });
            }
        }
    }
}
