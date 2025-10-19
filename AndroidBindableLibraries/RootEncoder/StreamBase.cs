namespace Com.Pedro.Library.Base;

using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Views;
using AndroidX.Annotations;
using Common;
using Encoder;
using Encoder.Input.Sources.Audio;
using Encoder.Input.Sources.Video;
using Encoder.Video;
using Java.Nio;
using VideoEncoder = Encoder.Video.VideoEncoder;
using AudioEncoder = Encoder.Audio.AudioEncoder;
using Com.Pedro.Encoder.Input.Audio;
using Encoder.Audio;
using Encoder.Utils;
using AudioSource = Encoder.Input.Sources.Audio.AudioSource;
using VideoSource = Encoder.Input.Sources.Video.VideoSource;

public abstract class StreamBase
{
	private readonly IGetMicrophoneData getMicrophoneData;
	private readonly VideoEncoder videoEncoder;
	private readonly VideoEncoder videoEncoderRecord;
	private readonly AudioEncoder audioEncoder;
	private readonly GlStreamInterface glInterface;
	private BaseRecordController recordController = new AndroidMuxerRecordController();
	private readonly FpsListener fpsListener = new FpsListener();
	private bool differentRecordResolution;
	private readonly PreviewCallback previewCallback;

	public bool IsStreaming { get; private set; }
	public bool IsOnPreview { get; private set; }
	public bool IsRecording => recordController.IsRunning;
	public VideoSource VideoSource { get; private set; }
	public AudioSource AudioSource { get; private set; }

	protected StreamBase(Context context, Com.Pedro.Encoder.Input.Sources.Video.VideoSource vSource, Com.Pedro.Encoder.Input.Sources.Audio.AudioSource aSource)
	{
		VideoSource = vSource;
		AudioSource = aSource;

		getMicrophoneData = new GetMicrophoneDataImpl(this);

		videoEncoder = new VideoEncoder(getVideoData);
		videoEncoderRecord = new VideoEncoder(getVideoDataRecord);
		audioEncoder = new AudioEncoder(getAacData);

		glInterface = new GlStreamInterface(context);

		previewCallback = new PreviewCallback(
			(surface, width, height) =>
			{
				if (!IsOnPreview) StartPreview(surface, width, height);
			},
			(width, height) =>
			{
				GetGlInterface().SetPreviewResolution(width, height);
			},
			() =>
			{
				if (IsOnPreview) StopPreview(true);
			});
	}

	// PrepareVideo method
	public bool PrepareVideo(
		int width,
		int height,
		int bitrate,
		int fps = 30,
		int iFrameInterval = 2,
		int rotation = 0,
		int profile = -1,
		int level = -1,
		int recordWidth = 0,
		int recordHeight = 0,
		int recordBitrate = 0)
	{
		if (IsStreaming || IsRecording || IsOnPreview)
			throw new InvalidOperationException("Stream, record and preview must be stopped before prepareVideo");

		differentRecordResolution = false;
		if (recordBitrate == 0) recordBitrate = bitrate;

		if (recordWidth > 0 && recordHeight > 0)
		{
			if (recordWidth / (double)recordHeight != width / (double)height)
				throw new ArgumentException("The aspect ratio of record and stream resolution must be the same");
			differentRecordResolution = true;
		}

		bool videoResult = VideoSource.Init(Math.Max(width, recordWidth), Math.Max(height, recordHeight), fps, rotation);

		if (videoResult)
		{
			if (differentRecordResolution)
			{
				if (rotation == 90 || rotation == 270)
					glInterface.SetEncoderRecordSize(recordHeight, recordWidth);
				else
					glInterface.SetEncoderRecordSize(recordWidth, recordHeight);
			}
			if (rotation == 90 || rotation == 270)
				glInterface.SetEncoderSize(height, width);
			else
				glInterface.SetEncoderSize(width, height);

			bool isPortrait = (rotation == 90 || rotation == 270);
			glInterface.SetIsPortrait(isPortrait);
			glInterface.SetCameraOrientation(rotation == 0 ? 270 : rotation - 90);
			glInterface.SetOrientationConfig(VideoSource.OrientationConfig);

			if (differentRecordResolution)
			{
				if (!videoEncoderRecord.PrepareVideoEncoder(recordWidth, recordHeight, fps, recordBitrate, rotation, iFrameInterval, FormatVideoEncoder.Surface, profile, level))
					return false;
			}
			bool result = videoEncoder.PrepareVideoEncoder(width, height, fps, bitrate, rotation, iFrameInterval, FormatVideoEncoder.Surface, profile, level);
			ForceFpsLimit(true);
			return result;
		}
		return false;
	}

	// PrepareAudio method
	public bool PrepareAudio(int sampleRate, bool isStereo, int bitrate, bool echoCanceler = false, bool noiseSuppressor = false)
	{
		if (IsStreaming || IsRecording)
			throw new InvalidOperationException("Stream and record must be stopped before prepareAudio");

		bool audioResult = AudioSource.Init(sampleRate, isStereo, echoCanceler, noiseSuppressor);
		if (audioResult)
		{
			OnAudioInfoImp(sampleRate, isStereo);
			return audioEncoder.PrepareAudioEncoder(bitrate, sampleRate, isStereo);
		}
		return false;
	}

	// StartStream
	public void StartStream(string endPoint)
	{
		if (IsStreaming)
			throw new InvalidOperationException("Stream already started, stopStream before startStream again");
		IsStreaming = true;
		StartStreamImp(endPoint);
		if (!IsRecording)
			StartSources();
		else
			RequestKeyframe();
	}

	// RequestKeyframe
	public void RequestKeyframe()
	{
		if (videoEncoder.IsRunning) videoEncoder.RequestKeyframe();
		if (videoEncoderRecord.IsRunning) videoEncoderRecord.RequestKeyframe();
	}

	// SetVideoBitrateOnFly
	public void SetVideoBitrateOnFly(int bitrate)
	{
		videoEncoder.SetVideoBitrateOnFly(bitrate);
	}

	// ForceFpsLimit
	public void ForceFpsLimit(bool enabled)
	{
		int fps = enabled ? videoEncoder.Fps : 0;
		videoEncoder.SetForceFps(fps);
		videoEncoderRecord.SetForceFps(fps);
		glInterface.ForceFpsLimit(fps);
	}

	// ForceCodecType
	public void ForceCodecType(CodecUtil.CodecType codecTypeVideo, CodecUtil.CodecType codecTypeAudio)
	{
		videoEncoder.ForceCodecType(codecTypeVideo);
		videoEncoderRecord.ForceCodecType(codecTypeVideo);
		audioEncoder.ForceCodecType(codecTypeAudio);
	}

	// StopStream
	public bool StopStream()
	{
		IsStreaming = false;
		StopStreamImp();
		if (!IsRecording)
		{
			StopSources();
			return PrepareEncoders();
		}
		return true;
	}

	// StartRecord
	public void StartRecord(string path, RecordController.RecordTracks? tracks, RecordController.IListener listener)
	{
		if (IsRecording)
			throw new InvalidOperationException("Record already started, stopRecord before startRecord again");

		var usedTracks = tracks ?? (VideoSource is NoVideoSource ? RecordController.RecordTracks.Audio :
			(AudioSource is NoAudioSource ? RecordController.RecordTracks.Video :
				RecordController.RecordTracks.All));

		recordController.StartRecord(path, listener, usedTracks);

		if (!IsStreaming)
			StartSources();
		else
		{
			videoEncoder.RequestKeyframe();
			videoEncoderRecord.RequestKeyframe();
		}
	}

	// StopRecord
	public bool StopRecord()
	{
		recordController.StopRecord();
		if (!IsStreaming)
		{
			StopSources();
			return PrepareEncoders();
		}
		return true;
	}

	// PauseRecord
	public void PauseRecord()
	{
		recordController.PauseRecord();
	}

	// ResumeRecord
	public void ResumeRecord()
	{
		recordController.ResumeRecord();
	}

	// StartPreview overloads
	public void StartPreview(TextureView textureView, bool autoHandle = false)
	{
		if (autoHandle)
		{
			previewCallback.SetTextureView(textureView);
			if (textureView.IsAvailable && !IsOnPreview) StartPreview(textureView);
		}
		else
		{
			StartPreview(new Surface(textureView.SurfaceTexture), textureView.Width, textureView.Height);
		}
	}

	public void StartPreview(SurfaceView surfaceView, bool autoHandle = false)
	{
		if (autoHandle)
		{
			previewCallback.SetSurfaceView(surfaceView);
			if (surfaceView.Holder.Surface.IsValid && !IsOnPreview) StartPreview(surfaceView);
		}
		else
		{
			StartPreview(surfaceView.Holder.Surface, surfaceView.Width, surfaceView.Height);
		}
	}

	public void StartPreview(SurfaceTexture surfaceTexture, int width, int height)
	{
		StartPreview(new Surface(surfaceTexture), width, height);
	}

	public void StartPreview(Surface surface, int width, int height)
	{
		if (!surface.IsValid) throw new ArgumentException("Make sure the Surface is valid");
		if (IsOnPreview) throw new InvalidOperationException("Preview already started, stopPreview before startPreview again");
		IsOnPreview = true;
		if (!glInterface.IsRunning) glInterface.Start();
		if (!VideoSource.IsRunning) VideoSource.Start(glInterface.SurfaceTexture);
		glInterface.AttachPreview(surface);
		glInterface.SetPreviewResolution(width, height);
	}

	// StopPreview
	public void StopPreview(bool removeCallbacks = false)
	{
		IsOnPreview = false;
		if (!IsStreaming && !IsRecording) VideoSource.Stop();
		glInterface.DeAttachPreview();
		if (!IsStreaming && !IsRecording) glInterface.Stop();
		if (removeCallbacks) previewCallback.RemoveCallbacks();
	}

	// ChangeVideoSource
	public void ChangeVideoSource(VideoSource source)
	{
		bool wasRunning = VideoSource.IsRunning;
		bool wasCreated = VideoSource.Created;

		if (wasCreated)
		{
			int width = videoEncoder.Width;
			int height = videoEncoder.Height;

			if (differentRecordResolution)
			{
				width = Math.Max(width, videoEncoderRecord.Width);
				height = Math.Max(height, videoEncoderRecord.Height);
			}

			source.Init(width, height, videoEncoder.Fps, videoEncoder.Rotation);
		}

		VideoSource.Stop();
		VideoSource.Release();
		glInterface.SurfaceTexture.TryClear();

		if (wasRunning) source.Start(glInterface.SurfaceTexture);
		glInterface.SetOrientationConfig(source.OrientationConfig);
		VideoSource = source;
	}

	// ChangeAudioSource
	public void ChangeAudioSource(AudioSource source)
	{
		bool wasRunning = AudioSource.IsRunning;
		bool wasCreated = AudioSource.Created;

		if (wasCreated)
			source.Init(AudioSource.SampleRate, AudioSource.Stereo, AudioSource.EchoCanceler, AudioSource.NoiseSuppressor);

		AudioSource.Stop();
		AudioSource.Release();

		if (wasRunning) source.Start(getMicrophoneData);
		AudioSource = source;
	}

	// SetTimestampMode
	public void SetTimestampMode(TimestampMode timestampModeVideo, TimestampMode timestampModeAudio)
	{
		videoEncoder.SetTimestampMode(timestampModeVideo);
		videoEncoderRecord.SetTimestampMode(timestampModeVideo);
		audioEncoder.SetTimestampMode(timestampModeAudio);
	}

	// SetEncoderErrorCallback
	public void SetEncoderErrorCallback(ICodecErrorCallback encoderErrorCallback)
	{
		videoEncoder.SetEncoderErrorCallback(encoderErrorCallback);
		videoEncoderRecord.SetEncoderErrorCallback(encoderErrorCallback);
		audioEncoder.SetEncoderErrorCallback(encoderErrorCallback);
	}

	// SetFpsListener
	public void SetFpsListener(FpsListener.ICallback callback)
	{
		fpsListener.SetCallback(callback);
	}

	// SetOrientation
	public void SetOrientation(int orientation)
	{
		glInterface.SetCameraOrientation(orientation);
	}

	// GetGlInterface
	public GlStreamInterface GetGlInterface() => glInterface;

	// SetRecordController
	public void SetRecordController(BaseRecordController controller)
	{
		if (!IsRecording) recordController = controller;
	}

	// GetSurfaceTexture
	public SurfaceTexture GetSurfaceTexture()
	{
		if (!(VideoSource is NoVideoSource))
			throw new InvalidOperationException("GetSurfaceTexture only available with VideoSource.DISABLED");

		return glInterface.SurfaceTexture;
	}

	protected Size GetVideoResolution() => new Size(videoEncoder.Width, videoEncoder.Height);

	protected int GetVideoFps() => videoEncoder.Fps;

	private void StartSources()
	{
		if (!glInterface.IsRunning) glInterface.Start();
		if (!VideoSource.IsRunning) VideoSource.Start(glInterface.SurfaceTexture);
		AudioSource.Start(getMicrophoneData);
		long startTs = TimeUtils.CurrentTimeMicro;
		videoEncoder.Start(startTs);
		if (differentRecordResolution) videoEncoderRecord.Start(startTs);
		audioEncoder.Start(startTs);
		glInterface.AddMediaCodecSurface(videoEncoder.InputSurface);
		if (differentRecordResolution) glInterface.AddMediaCodecRecordSurface(videoEncoderRecord.InputSurface);
	}

	private void StopSources()
	{
		if (!IsOnPreview) VideoSource.Stop();
		AudioSource.Stop();
		glInterface.RemoveMediaCodecSurface();
		glInterface.RemoveMediaCodecRecordSurface();
		if (!IsOnPreview) glInterface.Stop();
		videoEncoder.Stop();
		videoEncoderRecord.Stop();
		audioEncoder.Stop();
		if (!IsRecording) recordController.ResetFormats();
	}

	public void Release()
	{
		if (IsStreaming) StopStream();
		if (IsRecording) StopRecord();
		if (IsOnPreview) StopPreview();
		StopSources();
		VideoSource.Release();
		AudioSource.Release();
		glInterface.SurfaceTexture.TryClear();
	}

	public bool ResetVideoEncoder()
	{
		if (differentRecordResolution)
		{
			glInterface.RemoveMediaCodecRecordSurface();
			if (!videoEncoderRecord.Reset()) return false;
			glInterface.AddMediaCodecRecordSurface(videoEncoderRecord.InputSurface);
		}
		glInterface.RemoveMediaCodecSurface();
		if (!videoEncoder.Reset()) return false;
		glInterface.AddMediaCodecSurface(videoEncoder.InputSurface);
		return true;
	}

	public bool ResetAudioEncoder() => audioEncoder.Reset();

	private bool PrepareEncoders()
	{
		if (differentRecordResolution)
		{
			if (!videoEncoderRecord.PrepareVideoEncoder()) return false;
		}
		return videoEncoder.PrepareVideoEncoder() && audioEncoder.PrepareAudioEncoder();
	}

	private readonly IGetAudioData getAacData;
	private readonly IGetVideoData getVideoData;
	private readonly IGetVideoData getVideoDataRecord;

	// Initialization of delegates
	private class GetMicrophoneDataImpl : Java.Lang.Object, IGetMicrophoneData
	{
		private readonly StreamBase outer;
		public GetMicrophoneDataImpl(StreamBase outer) { this.outer = outer; }

		public void InputPCMData(Frame frame)
		{
			outer.audioEncoder.InputPCMData(frame);
		}
	}

	private IGetAudioData GetAacDataImpl() => new GetAudioDataImpl(this);
	private class GetAudioDataImpl : Java.Lang.Object, IGetAudioData
	{
		private readonly StreamBase outer;
		public GetAudioDataImpl(StreamBase outer) { this.outer = outer; }

		public void GetAudioData(ByteBuffer audioBuffer, MediaCodec.BufferInfo info)
		{
			outer.GetAudioDataImp(audioBuffer, info);
			outer.recordController.RecordAudio(audioBuffer, info);
		}

		public void OnAudioFormat(MediaFormat mediaFormat)
		{
			outer.recordController.SetAudioFormat(mediaFormat);
		}
	}

	private IGetVideoData GetVideoDataImpl() => new GetVideoDataImpl(this);
	private class GetVideoDataImpl : Java.Lang.Object, IGetVideoData
	{
		private readonly StreamBase outer;
		public GetVideoDataImpl(StreamBase outer) { this.outer = outer; }

		public void OnVideoInfo(ByteBuffer sps, ByteBuffer? pps, ByteBuffer? vps)
		{
			outer.OnVideoInfoImp(sps.Duplicate(), pps?.Duplicate(), vps?.Duplicate());
		}

		public void GetVideoData(ByteBuffer videoBuffer, MediaCodec.BufferInfo info)
		{
			outer.fpsListener.CalculateFps();
			outer.GetVideoDataImp(videoBuffer, info);
			if (!outer.differentRecordResolution) outer.recordController.RecordVideo(videoBuffer, info);
		}

		public void OnVideoFormat(MediaFormat mediaFormat)
		{
			if (!outer.differentRecordResolution) outer.recordController.SetVideoFormat(mediaFormat);
		}
	}

	private IGetVideoData GetVideoDataRecordImpl() => new GetVideoDataRecordImpl(this);
	private class GetVideoDataRecordImpl : Java.Lang.Object, IGetVideoData
	{
		private readonly StreamBase outer;
		public GetVideoDataRecordImpl(StreamBase outer) { this.outer = outer; }

		public void OnVideoInfo(ByteBuffer sps, ByteBuffer? pps, ByteBuffer? vps)
		{
			// no-op
		}

		public void GetVideoData(ByteBuffer videoBuffer, MediaCodec.BufferInfo info)
		{
			outer.recordController.RecordVideo(videoBuffer, info);
		}

		public void OnVideoFormat(MediaFormat mediaFormat)
		{
			bool isOnlyVideo = outer.AudioSource is NoAudioSource;
			outer.recordController.SetVideoFormat(mediaFormat);
		}
	}

	protected abstract void OnAudioInfoImp(int sampleRate, bool isStereo);
	protected abstract void StartStreamImp(string endPoint);
	protected abstract void StopStreamImp();
	protected abstract void OnVideoInfoImp(ByteBuffer sps, ByteBuffer? pps, ByteBuffer? vps);
	protected abstract void GetVideoDataImp(ByteBuffer videoBuffer, MediaCodec.BufferInfo info);
	protected abstract void GetAudioDataImp(ByteBuffer audioBuffer, MediaCodec.BufferInfo info);

	public abstract StreamBaseClient GetStreamClient();

	public void SetVideoCodec(VideoCodec codec)
	{
		SetVideoCodecImp(codec);
		recordController.SetVideoCodec(codec);
		string type = codec switch
		{
			VideoCodec.H264 => CodecUtil.H264Mime,
			VideoCodec.H265 => CodecUtil.H265Mime,
			VideoCodec.AV1 => CodecUtil.Av1Mime,
			_ => throw new ArgumentOutOfRangeException(nameof(codec), "Unsupported codec")
		};
		videoEncoder.Type = type;
		videoEncoderRecord.Type = type;
	}

	public void SetAudioCodec(AudioCodec codec)
	{
		SetAudioCodecImp(codec);
		recordController.SetAudioCodec(codec);
		string type = codec switch
		{
			AudioCodec.G711 => CodecUtil.G711Mime,
			AudioCodec.AAC => CodecUtil.AacMime,
			AudioCodec.OPUS => CodecUtil.OpusMime,
			_ => throw new ArgumentOutOfRangeException(nameof(codec), "Unsupported codec")
		};
		audioEncoder.Type = type;
	}

	protected abstract void SetVideoCodecImp(VideoCodec codec);
	protected abstract void SetAudioCodecImp(AudioCodec codec);
}