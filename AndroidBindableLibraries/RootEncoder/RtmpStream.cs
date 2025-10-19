namespace Com.Pedro.Library.Base
{
	using System;
	using Android.Content;
	using Android.Media;
	using Android.OS;
	using Android.Runtime;
	using AndroidX.Annotations;
	using Com.Pedro.Common;
	using Com.Pedro.Encoder.Input.Sources.Audio;
	using Com.Pedro.Encoder.Input.Sources.Video;
	using Com.Pedro.Rtmp.Rtmp;
	using Java.Nio;
	using AudioSource = Encoder.Input.Sources.Audio.AudioSource;
	using VideoSource = Encoder.Input.Sources.Video.VideoSource;

	public class RtmpStream : StreamBase
	{
		private readonly Rtmp.Rtmp.RtmpClient rtmpClient;
		private readonly IStreamClientListener streamClientListener;

		public RtmpStream(Context context, IConnectChecker connectChecker, VideoSource videoSource, AudioSource audioSource)
			: base(context, videoSource, audioSource)
		{
			rtmpClient = new Rtmp.Rtmp.RtmpClient(connectChecker);
			streamClientListener = new StreamClientListenerImpl(this);
		}

		public RtmpStream(Context context, IConnectChecker connectChecker)
			: this(context, connectChecker, new Camera2Source(context), new MicrophoneSource())
		{
		}

		public override RtmpStreamClient GetStreamClient()
		{
			return new RtmpStreamClient(rtmpClient, streamClientListener);
		}

		protected override void SetVideoCodecImp(VideoCodec codec)
		{
			rtmpClient.SetVideoCodec(codec);
		}

		protected override void SetAudioCodecImp(AudioCodec codec)
		{
			rtmpClient.SetAudioCodec(codec);
		}

		protected override void OnAudioInfoImp(int sampleRate, bool isStereo)
		{
			rtmpClient.SetAudioInfo(sampleRate, isStereo);
		}

		protected override void StartStreamImp(string endPoint)
		{
			var resolution = base.GetVideoResolution();
			rtmpClient.SetVideoResolution(resolution.Width, resolution.Height);
			rtmpClient.SetFps(base.GetVideoFps());
			rtmpClient.Connect(endPoint);
		}

		protected override void StopStreamImp()
		{
			rtmpClient.Disconnect();
		}

		protected override void OnVideoInfoImp(ByteBuffer sps, ByteBuffer pps, ByteBuffer vps)
		{
			rtmpClient.SetVideoInfo(sps, pps, vps);
		}

		protected override void GetVideoDataImp(ByteBuffer videoBuffer, MediaCodec.BufferInfo info)
		{
			rtmpClient.SendVideo(videoBuffer, info);
		}

		protected override void GetAudioDataImp(ByteBuffer audioBuffer, MediaCodec.BufferInfo info)
		{
			rtmpClient.SendAudio(audioBuffer, info);
		}

		private class StreamClientListenerImpl : IStreamClientListener
		{
			private readonly RtmpStream rtmpStream;

			public StreamClientListenerImpl(RtmpStream rtmpStream)
			{
				this.rtmpStream = rtmpStream;
			}

			public void OnRequestKeyframe()
			{
				rtmpStream.RequestKeyframe();
			}
		}
	}