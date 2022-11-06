using System.IO;

namespace Autonoma.UI.Operation;

public class Settings
{
    public string? Theme { get; set; } = null;
    public FileInfo[]? Scripts { get; set; }
    public FileInfo? Project { get; set; } = new FileInfo("C:\\Users\\peche\\OneDrive\\Документы\\Project1.project");
    public bool Repl { get; set; }
    public bool UseSkia { get; set; }
#if ENABLE_DIRECT2D1
    public bool UseDirect2D1 { get; set; }
#endif
    public bool UseGpu { get; set; } = true;
    public bool AllowEglInitialization { get; set; } = true;
    public bool UseWgl { get; set; }
    public bool UseDeferredRendering { get; set; } = true;
    public bool UseWindowsUIComposition { get; set; } = true;
    public bool UseDirectX11 { get; set; }
    public bool UseManagedSystemDialogs { get; set; }
    public bool UseHeadless { get; set; }
    public bool UseHeadlessDrawing { get; set; }
}
