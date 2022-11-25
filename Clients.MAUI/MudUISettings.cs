﻿using Colors = MudBlazor.Colors;

namespace Clients.MAUI;

public static class MudUISettings
{
    private static Typography DefaultTypography = new Typography()
    {
        Default = new Default()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = ".875rem",
            FontWeight = 400,
            LineHeight = 1.43,
            LetterSpacing = ".01071em"
        },
        H1 = new H1()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = "6rem",
            FontWeight = 300,
            LineHeight = 1.167,
            LetterSpacing = "-.01562em"
        },
        H2 = new H2()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = "3.75rem",
            FontWeight = 300,
            LineHeight = 1.2,
            LetterSpacing = "-.00833em"
        },
        H3 = new H3()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = "3rem",
            FontWeight = 400,
            LineHeight = 1.167,
            LetterSpacing = "0"
        },
        H4 = new H4()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = "2.125rem",
            FontWeight = 400,
            LineHeight = 1.235,
            LetterSpacing = ".00735em"
        },
        H5 = new H5()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = "1.5rem",
            FontWeight = 400,
            LineHeight = 1.334,
            LetterSpacing = "0"
        },
        H6 = new H6()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = "1.15rem",
            FontWeight = 400,
            LineHeight = 1.2,
            LetterSpacing = ".0035em"
        },
        Button = new MudBlazor.Button()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = ".875rem",
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em"
        },
        Body1 = new Body1()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = "1rem",
            FontWeight = 400,
            LineHeight = 1.5,
            LetterSpacing = ".00938em"
        },
        Body2 = new Body2()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = ".875rem",
            FontWeight = 400,
            LineHeight = 1.43,
            LetterSpacing = ".01071em"
        },
        Caption = new Caption()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = ".75rem",
            FontWeight = 400,
            LineHeight = 1.66,
            LetterSpacing = ".03333em"
        },
        Subtitle2 = new Subtitle2()
        {
            FontFamily = new[] { "Jura", "Helvetica", "Arial", "sans-serif" },
            FontSize = ".875rem",
            FontWeight = 500,
            LineHeight = 1.57,
            LetterSpacing = ".00714em"
        }
    };
    private static LayoutProperties DefaultLayoutProperties = new LayoutProperties()
    {
        DefaultBorderRadius = "5px"
    };

    public static MudTheme DefaultTheme = new MudTheme()
    {
        Palette = new Palette()
        {
            Primary = "#009BFF",
            AppbarBackground = "#F3F5F8",
            Background = Colors.Grey.Lighten5,
            DrawerBackground = "#DADFE0",
            DrawerText = "rgba(0,0,0, 0.7)",
            Success = "#007E33",

        },
        Typography = DefaultTypography,
        LayoutProperties = DefaultLayoutProperties
    };
}
