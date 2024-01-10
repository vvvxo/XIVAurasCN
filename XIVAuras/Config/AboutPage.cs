﻿using System.Numerics;
using ImGuiNET;
using XIVAuras.Helpers;

namespace XIVAuras.Config
{
    public class AboutPage : IConfigPage
    {
        public string Name => "Changelog";

        public IConfigPage GetDefault() => new AboutPage();

        public void DrawConfig(IConfigurable parent, Vector2 size, float padX, float padY)
        {
            if (ImGui.BeginChild("##AboutPage", new Vector2(size.X, size.Y), true))
            {
                Vector2 headerSize = Vector2.Zero;
                if (Plugin.IconTexture is not null)
                {
                    Vector2 iconSize = new Vector2(Plugin.IconTexture.Width, Plugin.IconTexture.Height);
                    string versionText = $"XIVAuras v{Plugin.Version}";
                    Vector2 textSize = ImGui.CalcTextSize(versionText);
                    headerSize = new Vector2(size.X, iconSize.Y + textSize.Y);

                    if (ImGui.BeginChild("##Icon", headerSize, false))
                    {
                        ImDrawListPtr drawList = ImGui.GetWindowDrawList();
                        Vector2 pos = ImGui.GetWindowPos().AddX(size.X / 2 - iconSize.X / 2);
                        drawList.AddImage(Plugin.IconTexture.ImGuiHandle, pos, pos + iconSize);
                        Vector2 textPos = ImGui.GetWindowPos().AddX(size.X / 2 - textSize.X / 2).AddY(iconSize.Y);
                        drawList.AddText(textPos, 0xFFFFFFFF, versionText);
                        ImGui.End();
                    }
                }

                ImGui.SetCursorPosY(ImGui.GetCursorPosY() + headerSize.Y);
                DrawHelpers.DrawSpacing(1);
                ImGui.Text("Changelog");
                Vector2 changeLogSize = new Vector2(size.X - padX * 2, size.Y - ImGui.GetCursorPosY() - padY - 30);

                if (ImGui.BeginChild("##Changelog", changeLogSize, true))
                {
                    ImGui.Text(Plugin.Changelog);
                    ImGui.EndChild();
                }
                
                ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 0);
                Vector2 buttonSize = new Vector2((size.X - padX * 2 - padX * 2) / 3, 30 - padY * 2);
                if (ImGui.Button("Github", buttonSize))
                {
                    Utils.OpenUrl("https://github.com/lichie567/XIVAuras");
                }

                ImGui.SameLine();
                if (ImGui.Button("Help", buttonSize))
                {
                    Utils.OpenUrl("https://github.com/lichie567/XIVAuras/wiki/FAQ");
                }

                ImGui.SameLine();
                if (ImGui.Button("Discord", buttonSize))
                {
                    Utils.OpenUrl("https://discord.gg/delvui");
                }

                ImGui.PopStyleVar();
            }

            ImGui.EndChild();
        }
    }
}
