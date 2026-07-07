using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using CommunityToolkit.Maui.Markup;

namespace MauiSkiaSharpDashboard;

public class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewModel viewModel)
    {
        BindingContext = viewModel;

        Content = new Grid
        {
            RowDefinitions = GridRowsColumns.Rows.Define(GridRowsColumns.Auto, GridRowsColumns.Star),
            Children =
            {
                new Label()
                    .Text("SMART SYSTEM TELEMETRY")
                    .Font(size: 18, bold: true)
                    .CenterHorizontal()
                    .Row(0).Margin(16),

                new SKCanvasView()
                    .Row(1)
                    .Assign<SKCanvasView, SKCanvasView>(out var canvasView)
                    .Invoke(canvas => canvas.PaintSurface += OnPaintCanvas)
            }
        };

        // High-frequency UI tick using System.Reactive or basic messaging
        viewModel.OnDataReceived += (s, e) => canvasView.InvalidateSurface();
    }

    private readonly SKPaint _backgroundPaint = new() { Color = SKColor.Parse("#E0E5EC"), Style = SKPaintStyle.Fill };
    private readonly SKPaint _glowPaint = new() { Color = SKColor.Parse("#FFFFFF"), IsAntialias = true };
    private readonly SKPaint _shadowPaint = new() { Color = SKColor.Parse("#A3B1C6"), IsAntialias = true };

    private void OnPaintCanvas(object? sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        var info = e.Info;

        canvas.Clear(_backgroundPaint.Color);

        // Draw Neumorphic Card Base
        var rect = new SKRect(40, 40, info.Width - 40, 300);

        // Smooth shadows instead of heavy multi-layered borders
        _shadowPaint.ImageFilter = SKImageFilter.CreateDropShadow(6, 6, 10, 10, SKColor.Parse("#A3B1C6"));
        _glowPaint.ImageFilter = SKImageFilter.CreateDropShadow(-6, -6, 10, 10, SKColor.Parse("#FFFFFF"));

        canvas.DrawRoundRect(rect, 20, 20, _shadowPaint);
        canvas.DrawRoundRect(rect, 20, 20, _glowPaint);

        // Draw real-time sensor waves directly from a pre-allocated data buffer
        DrawTelemetryWave(canvas, rect);
        DrawDashboardDecorations(canvas, rect, info);
    }

    private static void DrawDashboardDecorations(SKCanvas canvas, SKRect rect, SKImageInfo info)
    {
        using var titlePaint = new SKPaint();
        titlePaint.IsAntialias = true;
        titlePaint.Color = SKColor.Parse("#1F2A44");

        using var subtitlePaint = new SKPaint();
        subtitlePaint.IsAntialias = true;
        subtitlePaint.Color = SKColor.Parse("#36517A").WithAlpha(180);

        using var footerPaint = new SKPaint();
        footerPaint.IsAntialias = true;
        footerPaint.Color = SKColor.Parse("#1F2A44").WithAlpha(170);

        using var titleTypeface = SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Bold);
        using var subtitleTypeface = SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Normal);
        using var footerTypeface = SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Italic);

        using var titleFont = new SKFont(titleTypeface, 26);
        using var subtitleFont = new SKFont(subtitleTypeface, 16);
        using var footerFont = new SKFont(footerTypeface, 14);

        var title = "MAUI July UI 2026";
        var titleWidth = titleFont.MeasureText(title);
        var titleX = rect.MidX - (titleWidth / 2);
        var titleY = rect.Top + 36;

        canvas.DrawText(title, titleX, titleY, SKTextAlign.Left, titleFont, titlePaint);
        canvas.DrawText("Realtime telemetry dashboard", titleX + 2, titleY + 24, SKTextAlign.Left, subtitleFont, subtitlePaint);

        using var accentPaint = new SKPaint();
        accentPaint.Style = SKPaintStyle.Stroke;
        accentPaint.StrokeWidth = 2;
        accentPaint.IsAntialias = true;
        accentPaint.Color = SKColor.Parse("#4FA3FF").WithAlpha(150);

        using var dotPaint = new SKPaint();
        dotPaint.Style = SKPaintStyle.Fill;
        dotPaint.IsAntialias = true;
        dotPaint.Color = SKColor.Parse("#2F80ED").WithAlpha(180);

        var indicatorY = rect.Bottom + 36;
        var spacing = 28f;
        var startX = rect.MidX - spacing;

        for (var i = 0; i < 3; i++)
        {
            var x = startX + (spacing * i);
            canvas.DrawCircle(x, indicatorY, 6, dotPaint);
            canvas.DrawCircle(x, indicatorY, 10, accentPaint);
        }

        var footer = $"Canvas {info.Width} x {info.Height}";
        canvas.DrawText(footer, rect.Left + 4, indicatorY + 34, SKTextAlign.Left, footerFont, footerPaint);
    }

    private readonly float[] _waveBuffer = new float[120];
    private int _waveOffset;

    private void DrawTelemetryWave(SKCanvas canvas, SKRect rect)
    {
        var padding = 24f;
        var waveRect = new SKRect(rect.Left + padding, rect.Top + padding, rect.Right - padding, rect.Bottom - padding);

        if (waveRect.Width <= 1 || waveRect.Height <= 1)
        {
            return;
        }

        var centerY = waveRect.MidY;
        var amplitude = waveRect.Height * 0.32f;
        var pointCount = _waveBuffer.Length;

        for (var i = 0; i < pointCount; i++)
        {
            var sampleIndex = i + _waveOffset;
            _waveBuffer[i] =
                MathF.Sin(sampleIndex * 0.18f) * 0.55f +
                MathF.Sin(sampleIndex * 0.05f + 1.6f) * 0.25f +
                MathF.Sin(sampleIndex * 0.32f + 0.8f) * 0.2f;
        }

        _waveOffset++;

        using var strokePaint = new SKPaint();
        strokePaint.Style = SKPaintStyle.Stroke;
        strokePaint.StrokeWidth = 3;
        strokePaint.StrokeCap = SKStrokeCap.Round;
        strokePaint.IsAntialias = true;
        strokePaint.Color = SKColor.Parse("#2F80ED");

        using var glowStrokePaint = new SKPaint();
        glowStrokePaint.Style = SKPaintStyle.Stroke;
        glowStrokePaint.StrokeWidth = 7;
        glowStrokePaint.StrokeCap = SKStrokeCap.Round;
        glowStrokePaint.IsAntialias = true;
        glowStrokePaint.Color = SKColor.Parse("#4FA3FF").WithAlpha(70);
        glowStrokePaint.ImageFilter = SKImageFilter.CreateBlur(4, 4);

        using var fillPaint = new SKPaint();
        fillPaint.Style = SKPaintStyle.Fill;
        fillPaint.IsAntialias = true;
        fillPaint.Color = SKColor.Parse("#2F80ED").WithAlpha(35);

        using var wavePathBuilder = new SKPathBuilder();
        using var fillPathBuilder = new SKPathBuilder();

        wavePathBuilder.MoveTo(waveRect.Left, centerY - (_waveBuffer[0] * amplitude));
        fillPathBuilder.MoveTo(waveRect.Left, waveRect.Bottom);
        fillPathBuilder.LineTo(waveRect.Left, centerY - (_waveBuffer[0] * amplitude));

        for (var i = 1; i < pointCount; i++)
        {
            var t = i / (pointCount - 1f);
            var x = waveRect.Left + (waveRect.Width * t);
            var y = centerY - (_waveBuffer[i] * amplitude);

            wavePathBuilder.LineTo(x, y);
            fillPathBuilder.LineTo(x, y);
        }

        fillPathBuilder.LineTo(waveRect.Right, waveRect.Bottom);
        fillPathBuilder.Close();

        using var wavePath = wavePathBuilder.Detach();
        using var fillPath = fillPathBuilder.Detach();

        using var clipRoundRect = new SKRoundRect(rect, 20, 20);

        canvas.Save();
        canvas.ClipRoundRect(clipRoundRect, antialias: true);
        canvas.DrawPath(fillPath, fillPaint);
        canvas.DrawPath(wavePath, glowStrokePaint);
        canvas.DrawPath(wavePath, strokePaint);
        canvas.Restore();
    }
}

public class DashboardViewModel : IDisposable
{
    private readonly IDispatcherTimer _telemetryTimer;

    public DashboardViewModel()
    {
        var dispatcher = Application.Current?.Dispatcher ?? Dispatcher.GetForCurrentThread();

        if (dispatcher is null)
        {
            throw new InvalidOperationException("A UI dispatcher is required to run telemetry updates.");
        }

        _telemetryTimer = dispatcher.CreateTimer();
        _telemetryTimer.Interval = TimeSpan.FromMilliseconds(33);
        _telemetryTimer.IsRepeating = true;
        _telemetryTimer.Tick += OnTelemetryTick;
        _telemetryTimer.Start();
    }

    public event EventHandler? OnDataReceived;

    private void OnTelemetryTick(object? sender, EventArgs e)
    {
        OnDataReceived?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        _telemetryTimer.Tick -= OnTelemetryTick;

        if (_telemetryTimer.IsRunning)
        {
            _telemetryTimer.Stop();
        }
    }
}
