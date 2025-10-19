using Android.Content;
using Android.Media;
using Android.OS;
using Com.Pedro.Common;
using Com.Pedro.Encoder.Input.Sources.Audio;
using Com.Pedro.Encoder.Input.Sources.Video;
using Java.Nio;

namespace Com.Pedro.Rtmp.Rtmp.Message
{

    // Metadata.xml XPath class reference: path="/api/package[@name='com.pedro.rtmp.rtmp.message']/class[@name='SetP]
    public partial class SetPeerBandwidth
    {
        /// <inheritdoc />
        public override MessageType Type { get; }
    }
}

