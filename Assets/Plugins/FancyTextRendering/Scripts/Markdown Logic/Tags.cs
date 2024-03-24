using UnityEngine;

namespace LogicUI.FancyTextRendering.MarkdownLogic
{
    class Bold : SimpleMarkdownTag
    {
        protected override string MarkdownIndicator => "$$";
        protected override string RichTextOpenTag => "<b>";
        protected override string RichTextCloseTag => "</b>";

        protected override char? IgnoreContents => '$';

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.Bold.RenderBold;
    }

    class PinkColor : SimpleMarkdownTag
    {
        protected override string MarkdownIndicator => "@@@@@@";
        protected override string RichTextOpenTag => "<color=#ff00ffff>";
        protected override string RichTextCloseTag => "</color>";

        protected override char? IgnoreContents => '@';

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.PinkColor.RenderPinkColor;
    }
    
    class OrangeColor : SimpleMarkdownTag
    {
        protected override string MarkdownIndicator => "@@@@@";
        protected override string RichTextOpenTag => "<color=orange>";
        protected override string RichTextCloseTag => "</color>";

        protected override char? IgnoreContents => '@';

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.OrangeColor.RenderOrangeColor;
    }

    class LimeColor : SimpleMarkdownTag
    {
        protected override string MarkdownIndicator => "@@@@";
        protected override string RichTextOpenTag => "<color=#00ff00ff>";
        protected override string RichTextCloseTag => "</color>";

        protected override char? IgnoreContents => '@';

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.LimeColor.RenderLimeColor;
    }
    
    class YellowColor : SimpleMarkdownTag
    {
        protected override string MarkdownIndicator => "@@@";
        protected override string RichTextOpenTag => "<color=#ffff00ff>";
        protected override string RichTextCloseTag => "</color>";

        protected override char? IgnoreContents => '@';

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.YellowColor.RenderYellowColor;
    }
    
    class CyanColor : SimpleMarkdownTag
    {
        protected override string MarkdownIndicator => "@@";
        protected override string RichTextOpenTag => "<color=#00ffffff>";
        protected override string RichTextCloseTag => "</color>";

        protected override char? IgnoreContents => '@';

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.CyanColor.RenderCyanColor;
    }
    
    class Italics : SimpleMarkdownTag
    {
        protected override string MarkdownIndicator => "$";
        protected override string RichTextOpenTag => "<i>";
        protected override string RichTextCloseTag => "</i>";

        protected override char? IgnoreContents => '$';

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.Italics.RenderItalics;
    }
    
    class RedColor : SimpleMarkdownTag
    {
        protected override string MarkdownIndicator => "@";
        protected override string RichTextOpenTag => "<color=red>";
        protected override string RichTextCloseTag => "</color>";

        protected override char? IgnoreContents => '@';

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.RedColor.RenderRedColor;
    }
    
    class Witch : SimpleMarkdownTag
    {
        protected override string MarkdownIndicator => "°°";
        protected override string RichTextOpenTag => "<font=Witch of Thebes SDF>";
        protected override string RichTextCloseTag => "</font>";

        protected override char? IgnoreContents => '°';

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.Witch.RenderWitch;
    }

    class Strikethrough : SimpleMarkdownTag
    {
        protected override string MarkdownIndicator => "~~";
        protected override string RichTextOpenTag => "<s>";
        protected override string RichTextCloseTag => "</s>";

        protected override char? IgnoreContents => '~';

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.Strikethrough.RenderStrikethrough;
    }

    class Monospace : MarkdownTag
    {
        public override string GetMarkdownOpenTag(MarkdownRenderingSettings settings) => "`";
        public override string GetMarkdownCloseTag(MarkdownRenderingSettings settings) => "`";

        // Optimization not critical, as this is only called once per markdown render
        protected override string GetRichTextOpenTag(MarkdownRenderingSettings settings)
        {
            string tag = string.Empty;

            if (settings.Monospace.AddSeparationSpacing)
                tag += $"<space={settings.Monospace.SeparationSpacing}em>";

            /*
            if (settings.Monospace.UseCustomFont)
                tag += $"<font=\"{settings.Monospace.FontAssetPathRelativeToResources}\">";
            */

            if (settings.Monospace.DrawOverlay)
            {
                var padding = settings.Monospace.OverlayPaddingPixels;
                tag += $"<mark=#{ColorUtility.ToHtmlStringRGBA(settings.Monospace.OverlayColor)} padding=\"{padding},{padding},0,0\">";
            }

            if (settings.Monospace.ManuallySetCharacterSpacing)
                tag += $"<mspace={settings.Monospace.CharacterSpacing}em>";

            return tag;
        }

        protected override string GetRichTextCloseTag(MarkdownRenderingSettings settings)
        {
            string tag = string.Empty;

            if (settings.Monospace.UseCustomFont)
                tag += "</font>";

            if (settings.Monospace.DrawOverlay)
                tag += "</mark>";

            if (settings.Monospace.ManuallySetCharacterSpacing)
                tag += "</mspace>";

            if (settings.Monospace.AddSeparationSpacing)
                tag += $"<space={settings.Monospace.SeparationSpacing}em>";

            return tag;
        }


        protected override char? IgnoreContents => '`';

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.Monospace.RenderMonospace;
    }
    

    class SuperscriptSingle : MarkdownTag
    {
        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.Superscript.RenderSuperscript;


        public override string GetMarkdownOpenTag(MarkdownRenderingSettings settings) => "^";
        public override string GetMarkdownCloseTag(MarkdownRenderingSettings settings) => " ";

        protected override string GetRichTextOpenTag(MarkdownRenderingSettings settings) => "<sup>";
        protected override string GetRichTextCloseTag(MarkdownRenderingSettings settings) => "</sup> ";


        protected override char? IgnoreContents => '^';
        protected override bool TreatLineEndAsCloseTag => true;
    }
    
    class SuperscriptChain : MarkdownTag
    {
        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.Superscript.RenderSuperscript && settings.Superscript.RenderChainSuperscript;


        public override string GetMarkdownOpenTag(MarkdownRenderingSettings settings) => "^(";
        public override string GetMarkdownCloseTag(MarkdownRenderingSettings settings) => ")";

        protected override string GetRichTextOpenTag(MarkdownRenderingSettings settings) => "<sup>";
        protected override string GetRichTextCloseTag(MarkdownRenderingSettings settings) => "</sup>";
    }
}