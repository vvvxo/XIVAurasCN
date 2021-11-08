﻿using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using Newtonsoft.Json;
using XIVAuras.Config;
using XIVAuras.Helpers;

namespace XIVAuras.Auras
{
    [JsonObject]
    public class AuraLabel : AuraListItem
    {
        [JsonIgnore] private DataSource? Data { get; set; }

        public override AuraType Type => AuraType.Label;

        public LabelStyleConfig LabelStyleConfig { get; init; }

        // Constuctor for deserialization
        public AuraLabel() : this(string.Empty) { }

        public AuraLabel(string name) : base(name)
        {
            this.Name = name;
            this.LabelStyleConfig = new LabelStyleConfig();
        }

        public override IEnumerator<IConfigPage> GetEnumerator()
        {
            yield return this.LabelStyleConfig;
        }

        public override void Draw(Vector2 pos, Vector2? parentSize = null)
        {
            Vector2 size = parentSize.HasValue ? parentSize.Value : ImGui.GetMainViewport().Size;
            pos = parentSize.HasValue ? pos : Vector2.Zero;

            string text = this.LabelStyleConfig.TextFormat;
            if (this.Data.HasValue)
            {
                DataSource data = this.Data.Value;
                text = text.Replace("[duration]", this.LabelStyleConfig.FormatNumber(data.Duration));
                text = text.Replace("[stacks]", this.LabelStyleConfig.FormatNumber(data.Stacks));
                text = text.Replace("[cooldown]", this.LabelStyleConfig.FormatNumber(data.Cooldown));
            }

            bool fontPushed = Singletons.Get<FontsManager>().PushFont(this.LabelStyleConfig.FontKey);

            Vector2 textSize = ImGui.CalcTextSize(text);
            Vector2 textPos = Utils.GetAnchoredPosition(pos + this.LabelStyleConfig.Position, -size, this.LabelStyleConfig.ParentAnchor);
            textPos = Utils.GetAnchoredPosition(textPos, textSize, this.LabelStyleConfig.TextAlign);

            Vector2 textPad = new Vector2(2, 2); // Add small amount of padding to avoid text getting clipped
            DrawHelpers.DrawInWindow($"##{this.ID}", textPos - textPad, textSize + textPad, false, true, true, (drawList) =>
            {
                uint textColor = this.LabelStyleConfig.TextColor.Base;

                if (this.LabelStyleConfig.ShowOutline)
                {
                    uint outlineColor = this.LabelStyleConfig.OutlineColor.Base;
                    DrawHelpers.DrawOutlinedText(text, textPos, textColor, outlineColor, drawList);
                }
                else
                {
                    drawList.AddText(textPos, textColor, text);
                }
            });

            if (fontPushed)
            {
                ImGui.PopFont();
            }
        }

        public void SetData(DataSource data)
        {
            this.Data = data;
        }
    }
}