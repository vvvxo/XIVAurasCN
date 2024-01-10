﻿using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using Newtonsoft.Json;
using XIVAuras.Config;
using XIVAuras.Helpers;

namespace XIVAuras.Auras
{
    public class AuraLabel : AuraListItem
    {
        [JsonIgnore] private DataSource[]? _data;
        [JsonIgnore] private int _dataIndex;

        public override AuraType Type => AuraType.Label;

        [JsonConverter(typeof(LabelConverter))]
        public LabelStyleConfig LabelStyleConfig { get; set; }
        public StyleConditions<LabelStyleConfig> StyleConditions { get; set; }
        public VisibilityConfig VisibilityConfig { get; set; }

        // Constuctor for deserialization
        public AuraLabel() : this(string.Empty) { }

        public AuraLabel(string name, string textFormat = "") : base(name)
        {
            this.Name = name;
            this.LabelStyleConfig = new LabelStyleConfig(textFormat);
            this.StyleConditions = new StyleConditions<LabelStyleConfig>();
            this.VisibilityConfig = new VisibilityConfig();
        }

        public override IEnumerable<IConfigPage> GetConfigPages()
        {
            yield return this.LabelStyleConfig;
            yield return this.StyleConditions;
            yield return this.VisibilityConfig;
        }

        public override void ImportPage(IConfigPage page)
        {
            switch (page)
            {
                case LabelStyleConfig newPage:
                    this.LabelStyleConfig = newPage;
                    break;
                case StyleConditions<LabelStyleConfig> newPage:
                    this.StyleConditions = newPage;
                    break;
                case VisibilityConfig newPage:
                    this.VisibilityConfig = newPage;
                    break;
            }
        }

        public override void Draw(Vector2 pos, Vector2? parentSize = null, bool parentVisible = true)
        {
            if (!this.VisibilityConfig.IsVisible(parentVisible) && !this.Preview)
            {
                return;
            }

            Vector2 size = parentSize.HasValue ? parentSize.Value : ImGui.GetMainViewport().Size;
            pos = parentSize.HasValue ? pos : Vector2.Zero;

            LabelStyleConfig style = this.StyleConditions.GetStyle(_data, _dataIndex) ?? this.LabelStyleConfig;

            string text = _data is not null && _dataIndex < _data.Length && _data[_dataIndex] is not null
                ? _data[_dataIndex].GetFormattedString(style.TextFormat, "N", style.Rounding)
                : style.TextFormat;

            using (FontsManager.PushFont(style.FontKey))
            {
                Vector2 textSize = ImGui.CalcTextSize(text);
                Vector2 textPos = Utils.GetAnchoredPosition(pos + style.Position, -size, style.ParentAnchor);
                textPos = Utils.GetAnchoredPosition(textPos, textSize, style.TextAlign);
                DrawHelpers.DrawText(
                    ImGui.GetWindowDrawList(),
                    text,
                    textPos,
                    style.TextColor.Base,
                    style.ShowOutline,
                    style.OutlineColor.Base);
            }
        }

        public void SetData(DataSource[] data, int index)
        {
            _data = data;
            _dataIndex = index;
            this.StyleConditions.UpdateTriggerCount(data.Length);
        }
    }
}