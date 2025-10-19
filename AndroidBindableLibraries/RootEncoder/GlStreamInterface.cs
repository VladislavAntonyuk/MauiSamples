namespace Com.Pedro.Library.Base;

using Android.Content;
using Android.Graphics;
using Android.Views;
using Com.Pedro.Encoder.Input.GL;
using Com.Pedro.Encoder.Input.GL.Render;
using Com.Pedro.Encoder.Input.GL.Render.Filters;
using Com.Pedro.Encoder.Input.Sources;
using Com.Pedro.Encoder.Input.Video;
using Com.Pedro.Encoder.Utils;
using Com.Pedro.Encoder.Utils.GL;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Android.Widget;
using Point = Microsoft.Maui.Graphics.Point;

public class GlStreamInterface : Java.Lang.Object, SurfaceTexture.IOnFrameAvailableListener, IGlInterface
{
	private TakePhotoCallback takePhotoCallback;
	private volatile bool running;
	private SurfaceManager surfaceManager = new SurfaceManager();
	private SurfaceManager surfaceManagerEncoder = new SurfaceManager();
	private SurfaceManager surfaceManagerEncoderRecord = new SurfaceManager();
	private SurfaceManager surfaceManagerPhoto = new SurfaceManager();
	private SurfaceManager surfaceManagerPreview = new SurfaceManager();
	private MainRender mainRender = new MainRender();
	private int encoderWidth;
	private int encoderHeight;
	private int encoderRecordWidth;
	private int encoderRecordHeight;
	private int streamOrientation;
	private int previewWidth;
	private int previewHeight;
	private bool isPortrait;
	private bool isPortraitPreview;
	private OrientationForced orientationForced = OrientationForced.None;
	private BlockingCollection<Filter> filterQueue = new BlockingCollection<Filter>();
	private BlockingCollection<Action> threadQueue = new BlockingCollection<Action>();
	private bool muteVideo;
	private bool isPreviewHorizontalFlip;
	private bool isPreviewVerticalFlip;
	private bool isStreamHorizontalFlip;
	private bool isStreamVerticalFlip;
	private AspectRatioMode aspectRatioMode = AspectRatioMode.Adjust;
	private CancellationTokenSource cancellationTokenSource;
	private readonly FpsLimiter fpsLimiter = new FpsLimiter();
	private readonly ForceRenderer forceRender = new ForceRenderer();
	private bool autoHandleOrientation = false;
	private bool shouldHandleOrientation = true;
	private RenderErrorCallback renderErrorCallback;
	private ViewPort previewViewPort;
	private ViewPort streamViewPort;
	private readonly SensorRotationManager sensorRotationManager;

	public GlStreamInterface(Context context)
	{
		sensorRotationManager = new SensorRotationManager(context, true, true, (orientation, portrait) =>
		{
			if (autoHandleOrientation && shouldHandleOrientation)
			{
				SetCameraOrientation(orientation);
				SetIsPortrait(portrait);
			}
		});
	}

	public void SetEncoderSize(int width, int height)
	{
		encoderWidth = width;
		encoderHeight = height;
	}

	public void SetEncoderRecordSize(int width, int height)
	{
		encoderRecordWidth = width;
		encoderRecordHeight = height;
	}

	public Point GetEncoderSize()
	{
		return new Point(encoderWidth, encoderHeight);
	}

	public void MuteVideo()
	{
		muteVideo = true;
	}

	public void UnMuteVideo()
	{
		muteVideo = false;
	}

	public bool IsVideoMuted() => muteVideo;

	public void SetForceRender(bool enabled, int fps)
	{
		forceRender.SetEnabled(enabled, fps);
	}

	public void SetForceRender(bool enabled)
	{
		SetForceRender(enabled, 5);
	}

	public bool IsRunning() => running;

	public void SetRenderErrorCallback(RenderErrorCallback callback)
	{
		renderErrorCallback = callback;
	}

	public SurfaceTexture GetSurfaceTexture()
	{
		return mainRender.SurfaceTexture;
	}

	public Surface GetSurface()
	{
		return mainRender.Surface;
	}

	public void AddMediaCodecSurface(Surface surface)
	{
		if (surfaceManager.IsReady)
		{
			surfaceManagerEncoder.Release();
			surfaceManagerEncoder.EglSetup(surface, surfaceManager);
		}
	}

	public void RemoveMediaCodecSurface()
	{
		threadQueue.Clear();
		surfaceManagerEncoder.Release();
	}

	public void AddMediaCodecRecordSurface(Surface surface)
	{
		if (surfaceManager.IsReady)
		{
			surfaceManagerEncoderRecord.Release();
			surfaceManagerEncoderRecord.EglSetup(surface, surfaceManager);
		}
	}

	public void RemoveMediaCodecRecordSurface()
	{
		threadQueue.Clear();
		surfaceManagerEncoderRecord.Release();
	}

	public void TakePhoto(TakePhotoCallback callback)
	{
		takePhotoCallback = callback;
	}

	public void Start()
	{
		threadQueue.Clear();
		cancellationTokenSource?.Cancel();
		cancellationTokenSource = new CancellationTokenSource();

		int width = Math.Max(encoderWidth, encoderRecordWidth);
		int height = Math.Max(encoderHeight, encoderRecordHeight);

		surfaceManager.Release();
		surfaceManager.EglSetup();

		surfaceManagerPhoto.Release();
		surfaceManagerPhoto.EglSetup(width, height, surfaceManager);

		sensorRotationManager.Start();

		running = true;

		Task.Run(() => RunRenderLoop(cancellationTokenSource.Token));
	}

	private void RunRenderLoop(CancellationToken token)
	{
		try
		{
			surfaceManager.MakeCurrent();
			mainRender.InitGl(mainRender.Context, Math.Max(encoderWidth, encoderRecordWidth),
			                  Math.Max(encoderHeight, encoderRecordHeight), Math.Max(encoderWidth, encoderRecordWidth),
			                  Math.Max(encoderHeight, encoderRecordHeight));
			mainRender.SurfaceTexture.SetOnFrameAvailableListener(this);

			forceRender.Start(() =>
			{
				Task.Run(() =>
				{
					try
					{
						Draw(true);
					}
					catch (Exception e)
					{
						renderErrorCallback?.OnRenderError(e);
						throw;
					}
				});
			});

			while (!token.IsCancellationRequested)
			{
				Thread.Sleep(10);
			}
		}
		catch (Exception e)
		{
			renderErrorCallback?.OnRenderError(e);
			throw;
		}
	}

	public void Stop()
	{
		running = false;
		threadQueue.Clear();
		cancellationTokenSource?.Cancel();
		cancellationTokenSource = null;
		forceRender.Stop();
		sensorRotationManager.Stop();
		surfaceManagerPhoto.Release();
		surfaceManagerEncoder.Release();
		surfaceManagerEncoderRecord.Release();
		surfaceManager.Release();
		mainRender.Release();
	}

	private void Draw(bool forced)
	{
		if (!IsRunning())
			return;

		bool limitFps = fpsLimiter.LimitFPS();
		if (!forced)
			forceRender.FrameAvailable();

		if (filterQueue.Count > 0 && mainRender.IsReady())
		{
			if (surfaceManager.MakeCurrent())
			{
				if (filterQueue.TryTake(out Filter filter, Timeout.Infinite))
				{
					mainRender.SetFilterAction(filter.FilterAction, filter.Position, filter.BaseFilterRender);
				}
			}
		}

		if (surfaceManager.IsReady && mainRender.IsReady())
		{
			if (!surfaceManager.MakeCurrent())
				return;
			mainRender.UpdateFrame();
			mainRender.DrawSource();
			surfaceManager.SwapBuffer();
		}

		bool orientation = orientationForced switch
		{
			OrientationForced.Portrait => true,
			OrientationForced.Landscape => false,
			_ => isPortrait
		};
		bool orientationPreview = orientationForced switch
		{
			OrientationForced.Portrait => true,
			OrientationForced.Landscape => false,
			_ => isPortraitPreview
		};

		if (surfaceManagerEncoder.IsReady || surfaceManagerEncoderRecord.IsReady || surfaceManagerPhoto.IsReady)
		{
			mainRender.DrawFilters(false);
		}

		if (surfaceManagerEncoder.IsReady && mainRender.IsReady() && !limitFps)
		{
			int w = muteVideo ? 0 : encoderWidth;
			int h = muteVideo ? 0 : encoderHeight;
			if (surfaceManagerEncoder.MakeCurrent())
			{
				mainRender.DrawScreenEncoder(w, h, orientation, streamOrientation, isStreamVerticalFlip,
				                             isStreamHorizontalFlip, streamViewPort);
				surfaceManagerEncoder.SwapBuffer();
			}
		}

		if (surfaceManagerEncoderRecord.IsReady && mainRender.IsReady() && !limitFps)
		{
			int w = muteVideo ? 0 : encoderRecordWidth;
			int h = muteVideo ? 0 : encoderRecordHeight;
			if (surfaceManagerEncoderRecord.MakeCurrent())
			{
				mainRender.DrawScreenEncoder(w, h, orientation, streamOrientation, isStreamVerticalFlip,
				                             isStreamHorizontalFlip, streamViewPort);
				surfaceManagerEncoderRecord.SwapBuffer();
			}
		}

		if (takePhotoCallback != null && surfaceManagerPhoto.IsReady && mainRender.IsReady())
		{
			if (surfaceManagerPhoto.MakeCurrent())
			{
				mainRender.DrawScreen(encoderWidth, encoderHeight, AspectRatioMode.None, streamOrientation,
				                      isStreamVerticalFlip, isStreamHorizontalFlip, streamViewPort);
				var bitmap = GlUtil.GetBitmap(encoderWidth, encoderHeight);
				takePhotoCallback.OnTakePhoto(bitmap);
				takePhotoCallback = null;
				surfaceManagerPhoto.SwapBuffer();
			}
		}

		if (surfaceManagerPreview.IsReady && mainRender.IsReady() && !limitFps)
		{
			int w = previewWidth == 0 ? encoderWidth : previewWidth;
			int h = previewHeight == 0 ? encoderHeight : previewHeight;

			if (surfaceManager.MakeCurrent())
			{
				mainRender.DrawFilters(true);
				surfaceManager.SwapBuffer();
			}

			if (surfaceManagerPreview.MakeCurrent())
			{
				mainRender.DrawScreenPreview(w, h, orientationPreview, aspectRatioMode, 0, isPreviewVerticalFlip,
				                             isPreviewHorizontalFlip, previewViewPort);
				surfaceManagerPreview.SwapBuffer();
			}
		}
	}

	public void OnFrameAvailable(SurfaceTexture surfaceTexture)
	{
		if (!IsRunning())
			return;

		Task.Run(() =>
		{
			try
			{
				Draw(false);
			}
			catch (Exception e)
			{
				renderErrorCallback?.OnRenderError(e);
				throw;
			}
		});
	}

	public void SetOrientationConfig(OrientationConfig orientationConfig)
	{
		switch (orientationConfig.Forced)
		{
			case OrientationForced.Portrait:
			case OrientationForced.Landscape:
				ForceOrientation(orientationConfig.Forced);
				break;
			case OrientationForced.None:
				if (orientationConfig.IsPortrait == null && orientationConfig.CameraOrientation == null)
				{
					ForceOrientation(orientationConfig.Forced);
				}
				else
				{
					if (orientationConfig.IsPortrait.HasValue)
						SetIsPortrait(orientationConfig.IsPortrait.Value);
					if (orientationConfig.CameraOrientation.HasValue)
						SetCameraOrientation(orientationConfig.CameraOrientation.Value);
					shouldHandleOrientation = false;
					orientationForced = orientationConfig.Forced;
				}

				break;
		}
	}

	public void ForceOrientation(OrientationForced forced)
	{
		switch (forced)
		{
			case OrientationForced.Portrait:
				SetCameraOrientation(90);
				shouldHandleOrientation = false;
				break;
			case OrientationForced.Landscape:
				SetCameraOrientation(0);
				shouldHandleOrientation = false;
				break;
			case OrientationForced.None:
				int orientation = CameraHelper.GetCameraOrientation(mainRender.Context);
				SetCameraOrientation(orientation == 0 ? 270 : orientation - 90);
				shouldHandleOrientation = true;
				break;
		}

		orientationForced = forced;
	}

	public void AttachPreview(Surface surface)
	{
		if (surfaceManager.IsReady)
		{
			surfaceManagerPreview.Release();
			surfaceManagerPreview.EglSetup(surface, surfaceManager);
		}
	}

	public void DeAttachPreview()
	{
		surfaceManagerPreview.Release();
	}

	public void SetStreamRotation(int orientation)
	{
		streamOrientation = orientation;
	}

	public void SetPreviewResolution(int width, int height)
	{
		previewWidth = width;
		previewHeight = height;
	}

	public void SetIsPortrait(bool isPortrait)
	{
		SetPreviewIsPortrait(isPortrait);
		SetStreamIsPortrait(isPortrait);
	}

	public void SetPreviewIsPortrait(bool isPortrait)
	{
		isPortraitPreview = isPortrait;
	}

	public void SetStreamIsPortrait(bool isPortrait)
	{
		this.isPortrait = isPortrait;
	}

	public void SetCameraOrientation(int orientation)
	{
		mainRender.SetCameraRotation(orientation);
	}

	public void SetFilter(int filterPosition, BaseFilterRender baseFilterRender)
	{
		filterQueue.Add(new Filter(FilterAction.SetIndex, filterPosition, baseFilterRender));
	}

	public void AddFilter(BaseFilterRender baseFilterRender)
	{
		filterQueue.Add(new Filter(FilterAction.Add, 0, baseFilterRender));
	}

	public void AddFilter(int filterPosition, BaseFilterRender baseFilterRender)
	{
		filterQueue.Add(new Filter(FilterAction.AddIndex, filterPosition, baseFilterRender));
	}

	public void ClearFilters()
	{
		filterQueue.Add(new Filter(FilterAction.Clear, 0, new NoFilterRender()));
	}

	public void RemoveFilter(int filterPosition)
	{
		filterQueue.Add(new Filter(FilterAction.RemoveIndex, filterPosition, new NoFilterRender()));
	}

	public void RemoveFilter(BaseFilterRender baseFilterRender)
	{
		filterQueue.Add(new Filter(FilterAction.Remove, 0, baseFilterRender));
	}

	public int FiltersCount()
	{
		return mainRender.FiltersCount();
	}

	public void SetRotation(int rotation)
	{
		SetCameraOrientation(rotation);
	}

	public void ForceFpsLimit(int fps)
	{
		fpsLimiter.SetFPS(fps);
	}

	public void SetIsStreamHorizontalFlip(bool flip)
	{
		isStreamHorizontalFlip = flip;
	}

	public void SetIsStreamVerticalFlip(bool flip)
	{
		isStreamVerticalFlip = flip;
	}

	public void SetIsPreviewHorizontalFlip(bool flip)
	{
		isPreviewHorizontalFlip = flip;
	}

	public void SetIsPreviewVerticalFlip(bool flip)
	{
		isPreviewVerticalFlip = flip;
	}

	public void SetFilter(BaseFilterRender baseFilterRender)
	{
		filterQueue.Add(new Filter(FilterAction.Set, 0, baseFilterRender));
	}

	public void SetAspectRatioMode(AspectRatioMode mode)
	{


	}
}