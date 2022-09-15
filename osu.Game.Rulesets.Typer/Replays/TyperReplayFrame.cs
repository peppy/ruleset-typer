// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Typer.Replays
{
    public class TyperReplayFrame : ReplayFrame
    {
        public List<TyperAction> Actions = new List<TyperAction>();

        public TyperReplayFrame(TyperAction? button = null)
        {
            if (button.HasValue)
                Actions.Add(button.Value);
        }
    }
}
