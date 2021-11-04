﻿using System.Numerics;
using ImGuiNET;

namespace XIVAuras.Config
{
    public enum ActorType
    {
        Player,
        Target,
        TargetOfTarget,
        Focus,
    }

    public enum TriggerType
    {
        Buff,
        Debuff,
        Cooldown
    }

    public class TriggerConfig : IConfigPage
    {
        public string Name => "Trigger";

        public ActorType ActorType { get; set; }

        public uint StatusId;

        public bool IsTriggered()
        {
            return false;
        }

        public void DrawConfig(Vector2 size, float padX, float padY)
        {
            if (ImGui.BeginChild("##TriggerConfig", new Vector2(size.X, size.Y), true))
            {
                ImGui.Text("TODO");

                ImGui.EndChild();
            }
        }
    }
}