using MudBlazor;

namespace DWIS.BlackBoard.Explorer.Components.Layout;

/// <summary>
/// Central MudBlazor theme for the Blackboard Explorer app.
/// Colors are based on the NORCE / DWIS brand blue and purple.
/// </summary>
public static class AppTheme
{
    public static MudTheme Default { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#0B3D6B",
            Secondary = "#7B2D8B",
            Tertiary = "#1B9C85",

            AppbarBackground = "#0B3D6B",
            AppbarText = "#FFFFFF",

            Background = "#F3F5F9",
            BackgroundGray = "#EAEDF3",
            Surface = "#FFFFFF",

            DrawerBackground = "#FFFFFF",
            DrawerText = "#0B3D6B",
            DrawerIcon = "#0B3D6B",

            TextPrimary = "#1C2530",
            TextSecondary = "#5B6472",

            LinesDefault = "#E1E5EC",
            TableLines = "#E9ECF2",
            TableStriped = "#F7F9FC",
            TableHover = "#EEF3FA",
            Divider = "#E1E5EC",

            Success = "#2E7D32",
            Info = "#0288D1",
            Warning = "#ED8B02",
            Error = "#D32F2F",
        },

        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = new[] { "Segoe UI", "Helvetica Neue", "Helvetica", "Arial", "sans-serif" }
            }
        },

        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "10px",
            AppbarHeight = "64px"
        }
    };
}
