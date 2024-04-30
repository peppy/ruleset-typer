// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Threading;
using osu.Framework.Utils;
using osu.Game.Audio;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Legacy;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Typer.Objects;

namespace osu.Game.Rulesets.Typer.Beatmaps
{
    public class TyperBeatmapConverter : BeatmapConverter<TyperHitObject>
    {
        public TyperBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        // todo: Check for conversion types that should be supported (ie. Beatmap.HitObjects.Any(h => h is IHasXPosition))
        // https://github.com/ppy/osu/tree/master/osu.Game/Rulesets/Objects/Types
        public override bool CanConvert() => true;

        protected override IEnumerable<TyperHitObject> ConvertHitObject(HitObject obj, IBeatmap beatmap, CancellationToken cancellationToken)
        {
            switch (obj)
            {
                case IHasPath pathData:
                    IList<IList<HitSampleInfo>> allSamples = obj is IHasPathWithRepeats curveData ? curveData.NodeSamples : new List<IList<HitSampleInfo>>(new[] { obj.Samples });

                    int i = 0;

                    int spans = (obj as IHasRepeats)?.SpanCount() ?? 1;

                    TimingControlPoint timingPoint = beatmap.ControlPointInfo.TimingPointAt(obj.StartTime);
                    double beatLength = timingPoint.BeatLength;

                    double tickSpacing = Math.Min(beatLength / beatmap.Difficulty.SliderTickRate, (double)pathData.Duration / spans);

                    for (double j = obj.StartTime; j <= obj.StartTime + pathData.Duration + tickSpacing / 8; j += tickSpacing)
                    {
                        IList<HitSampleInfo> currentSamples = allSamples[i];

                        yield return new TyperHitObject
                        {
                            StartTime = j,
                            Samples = currentSamples,
                        };

                        i = (i + 1) % allSamples.Count;

                        if (Precision.AlmostEquals(0, tickSpacing))
                            break;
                    }

                    break;

                default:
                    yield return new TyperHitObject
                    {
                        Samples = obj.Samples,
                        StartTime = obj.StartTime,
                    };

                    break;
            }
        }
    }
}
