/*
* Copyright (C) 2009 The Android Open Source Project
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

#nullable disable

using System.IO;
using System.Runtime.CompilerServices;
using Android.Bluetooth;
using Android.OS;
using Android.Util;
using Java.Lang;
using Java.Util;

namespace com.xamarin.samples.bluetooth.bluetoothchat;

using System.Text;
using Android.Content;
using Android.Widget;
using Thread = Java.Lang.Thread;

public static class Constants
{
	public const int MESSAGE_STATE_CHANGE = 1;
	public const int MESSAGE_READ = 2;
	public const int MESSAGE_WRITE = 3;
	public const int MESSAGE_DEVICE_NAME = 4;
	public const int MESSAGE_TOAST = 5;

	public const string DEVICE_NAME = "device_name";
	public const string TOAST = "toast";
}

public class BluetoothDiscoveryModeArgs : EventArgs
{
	public BluetoothDiscoveryModeArgs(bool inDiscoveryMode)
	{
		InDiscoveryMode = inDiscoveryMode;
	}
	public bool InDiscoveryMode { get; private set; }
}


/// <summary>
/// Listen for when the device goes in and out of Bluetooth discoverability
/// mode, and will raise an Event.
/// </summary>
public class DiscoverableModeReceiver : BroadcastReceiver
{
	public event EventHandler<BluetoothDiscoveryModeArgs> BluetoothDiscoveryModeChanged;


	public override void OnReceive(Context context, Intent intent)
	{
		var currentScanMode = intent.GetIntExtra(BluetoothAdapter.ExtraScanMode, -1);
		var previousScanMode = intent.GetIntExtra(BluetoothAdapter.ExtraPreviousScanMode, -1);


		bool inDiscovery = currentScanMode == (int)ScanMode.ConnectableDiscoverable;

		BluetoothDiscoveryModeChanged?.Invoke(this, new BluetoothDiscoveryModeArgs(inDiscovery));

	}
}

/// <summary>
/// <para>This class does all the work for setting up and managing Bluetooth
/// connections with other devices. It has a thread that listens for
/// incoming connections, a thread for connecting with a device, and a
/// thread for performing data transmissions when connected.</para>
/// <para>Note that this isn't a real Android service class; this is
/// a wrapper which manages the various threads used to connect, send, and
/// receive messages via BT.</para>
/// </summary>
class BluetoothChatService
{
	/// <summary>
	/// Handles messages that come back from the ChatService.
	/// </summary>
	class ChatHandler : Handler
	{
#pragma warning disable CS0618
		public ChatHandler()
#pragma warning restore CS0618
		{
				
		}
		public override void HandleMessage(Message msg)
		{
			switch (msg.What)
			{
				case Constants.MESSAGE_STATE_CHANGE:
					switch (msg.What)
					{
						case BluetoothChatService.STATE_CONNECTED:
							//chatFrag.SetStatus(chatFrag.GetString(Resource.String.title_connected_to, chatFrag.connectedDeviceName));
							//chatFrag.conversationArrayAdapter.Clear();
							break;
						case BluetoothChatService.STATE_CONNECTING:
							//chatFrag.SetStatus(Resource.String.title_connecting);
							break;
						case BluetoothChatService.STATE_LISTEN:
							//chatFrag.SetStatus(Resource.String.not_connected);
							break;
						case BluetoothChatService.STATE_NONE:
							//chatFrag.SetStatus(Resource.String.not_connected);
							break;
					}
					break;
				case Constants.MESSAGE_WRITE:
					var writeBuffer = (byte[])msg.Obj;
					var writeMessage = Encoding.ASCII.GetString(writeBuffer);
					//chatFrag.conversationArrayAdapter.Add($"Me:  {writeMessage}");
					break;
				case Constants.MESSAGE_READ:
					var readBuffer = (byte[])msg.Obj;
					var readMessage = Encoding.ASCII.GetString(readBuffer);
					//chatFrag.conversationArrayAdapter.Add($"{chatFrag.connectedDeviceName}: {readMessage}");
					break;
				case Constants.MESSAGE_DEVICE_NAME:
					//chatFrag.connectedDeviceName = msg.Data.GetString(Constants.DEVICE_NAME);
					//if (chatFrag.Activity != null)
					//{
					//	Toast.MakeText(chatFrag.Activity, $"Connected to {chatFrag.connectedDeviceName}.", ToastLength.Short).Show();
					//}
					break;
				case Constants.MESSAGE_TOAST:
					break;
			}
		}
	}

	const string TAG = "BluetoothChatService";

	const string NAME_SECURE = "BluetoothChatSecure";
	const string NAME_INSECURE = "BluetoothChatInsecure";

	static UUID MY_UUID_SECURE = UUID.FromString("fa87c0d0-afac-11de-8a39-0800200c9a66");
	static UUID MY_UUID_INSECURE = UUID.FromString("8ce255c0-200a-11e0-ac64-0800200c9a66");

	BluetoothAdapter btAdapter;
	Handler handler;
	AcceptThread secureAcceptThread;
	AcceptThread insecureAcceptThread;
	ConnectThread connectThread;
	ConnectedThread connectedThread;
	int state;
	int newState;

	public const int STATE_NONE = 0;       // we're doing nothing
	public const int STATE_LISTEN = 1;     // now listening for incoming connections
	public const int STATE_CONNECTING = 2; // now initiating an outgoing connection
	public const int STATE_CONNECTED = 3;  // now connected to a remote device

	/// <summary>
	/// Constructor. Prepares a new BluetoothChat session.
	/// </summary>
	/// <param name='handler'>
	/// A Handler to send messages back to the UI Activity.
	/// </param>
	public BluetoothChatService(Handler handler)
	{
		var manager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Context.BluetoothService)!;
		btAdapter = manager.Adapter;
		state = STATE_NONE;
		newState = state;
		this.handler = handler;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	void UpdateUserInterfaceTitle()
	{
		state = GetState();
		newState = state;
		handler.ObtainMessage(Constants.MESSAGE_STATE_CHANGE, newState, -1).SendToTarget();
	}

	/// <summary>
	/// Return the current connection state.
	/// </summary>
	[MethodImpl(MethodImplOptions.Synchronized)]
	public int GetState()
	{
		return state;
	}

	// Start the chat service. Specifically start AcceptThread to begin a
	// session in listening (server) mode. Called by the Activity onResume()
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void Start()
	{
		if (connectThread != null)
		{
			connectThread.Cancel();
			connectThread = null;
		}

		if (connectedThread != null)
		{
			connectedThread.Cancel();
			connectedThread = null;
		}

		if (secureAcceptThread == null)
		{
			secureAcceptThread = new AcceptThread(this, true);
			secureAcceptThread.Start();
		}
		if (insecureAcceptThread == null)
		{
			insecureAcceptThread = new AcceptThread(this, false);
			insecureAcceptThread.Start();
		}

		UpdateUserInterfaceTitle();
	}



	/// <summary>
	/// Start the ConnectThread to initiate a connection to a remote device.
	/// </summary>
	/// <param name='device'>
	/// The BluetoothDevice to connect.
	/// </param>
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void Connect(BluetoothDevice device, bool secure)
	{
		if (state == STATE_CONNECTING)
		{
			if (connectThread != null)
			{
				connectThread.Cancel();
				connectThread = null;
			}
		}

		// Cancel any thread currently running a connection
		if (connectedThread != null)
		{
			connectedThread.Cancel();
			connectedThread = null;
		}

		// Start the thread to connect with the given device
		connectThread = new ConnectThread(device, this, secure);
		connectThread.Start();

		UpdateUserInterfaceTitle();
	}

	/// <summary>
	/// Start the ConnectedThread to begin managing a Bluetooth connection
	/// </summary>
	/// <param name='socket'>
	/// The BluetoothSocket on which the connection was made.
	/// </param>
	/// <param name='device'>
	/// The BluetoothDevice that has been connected.
	/// </param>
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void Connected(BluetoothSocket socket, BluetoothDevice device, string socketType)
	{
		// Cancel the thread that completed the connection
		if (connectThread != null)
		{
			connectThread.Cancel();
			connectThread = null;
		}

		// Cancel any thread currently running a connection
		if (connectedThread != null)
		{
			connectedThread.Cancel();
			connectedThread = null;
		}


		if (secureAcceptThread != null)
		{
			secureAcceptThread.Cancel();
			secureAcceptThread = null;
		}

		if (insecureAcceptThread != null)
		{
			insecureAcceptThread.Cancel();
			insecureAcceptThread = null;
		}

		// Start the thread to manage the connection and perform transmissions
		connectedThread = new ConnectedThread(socket, this, socketType);
		connectedThread.Start();

		// Send the name of the connected device back to the UI Activity
		var msg = handler.ObtainMessage(Constants.MESSAGE_DEVICE_NAME);
		Bundle bundle = new Bundle();
		bundle.PutString(Constants.DEVICE_NAME, device.Name);
		msg.Data = bundle;
		handler.SendMessage(msg);

		UpdateUserInterfaceTitle();
	}

	/// <summary>
	/// Stop all threads.
	/// </summary>
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void Stop()
	{
		if (connectThread != null)
		{
			connectThread.Cancel();
			connectThread = null;
		}

		if (connectedThread != null)
		{
			connectedThread.Cancel();
			connectedThread = null;
		}

		if (secureAcceptThread != null)
		{
			secureAcceptThread.Cancel();
			secureAcceptThread = null;
		}

		if (insecureAcceptThread != null)
		{
			insecureAcceptThread.Cancel();
			insecureAcceptThread = null;
		}

		state = STATE_NONE;
		UpdateUserInterfaceTitle();
	}

	/// <summary>
	/// Write to the ConnectedThread in an unsynchronized manner
	/// </summary>
	/// <param name='out'>
	/// The bytes to write.
	/// </param>
	public void Write(byte[] @out)
	{
		// Create temporary object
		ConnectedThread r;
		// Synchronize a copy of the ConnectedThread
		lock (this)
		{
			if (state != STATE_CONNECTED)
			{
				return;
			}
			r = connectedThread;
		}
		// Perform the write unsynchronized
		r.Write(@out);
	}

	/// <summary>
	/// Indicate that the connection attempt failed and notify the UI Activity.
	/// </summary>
	void ConnectionFailed()
	{
		state = STATE_LISTEN;

		var msg = handler.ObtainMessage(Constants.MESSAGE_TOAST);
		var bundle = new Bundle();
		bundle.PutString(Constants.TOAST, "Unable to connect device");
		msg.Data = bundle;
		handler.SendMessage(msg);
		Start();
	}

	/// <summary>
	/// Indicate that the connection was lost and notify the UI Activity.
	/// </summary>
	public void ConnectionLost()
	{
		var msg = handler.ObtainMessage(Constants.MESSAGE_TOAST);
		var bundle = new Bundle();
		bundle.PutString(Constants.TOAST, "Unable to connect device.");
		msg.Data = bundle;
		handler.SendMessage(msg);

		state = STATE_NONE;
		UpdateUserInterfaceTitle();
		this.Start();
	}

	/// <summary>
	/// This thread runs while listening for incoming connections. It behaves
	/// like a server-side client. It runs until a connection is accepted
	/// (or until cancelled).
	/// </summary>
	class AcceptThread : Thread
	{
		// The local server socket
		BluetoothServerSocket serverSocket;
		string socketType;
		BluetoothChatService service;

		public AcceptThread(BluetoothChatService service, bool secure)
		{
			BluetoothServerSocket tmp = null;
			socketType = secure ? "Secure" : "Insecure";
			this.service = service;

			try
			{
				if (secure)
				{
					tmp = service.btAdapter.ListenUsingRfcommWithServiceRecord(NAME_SECURE, MY_UUID_SECURE);
				}
				else
				{
					tmp = service.btAdapter.ListenUsingInsecureRfcommWithServiceRecord(NAME_INSECURE, MY_UUID_INSECURE);
				}

			}
			catch (Java.IO.IOException e)
			{
				Log.Error(TAG, "listen() failed", e);
			}
			serverSocket = tmp;
			service.state = STATE_LISTEN;
		}

		public override void Run()
		{
			Name = $"AcceptThread_{socketType}";
			BluetoothSocket socket = null;

			while (service.GetState() != STATE_CONNECTED)
			{
				try
				{
					socket = serverSocket.Accept();
				}
				catch (Java.IO.IOException e)
				{
					Log.Error(TAG, "accept() failed", e);
					break;
				}

				if (socket != null)
				{
					lock (this)
					{
						switch (service.GetState())
						{
							case STATE_LISTEN:
							case STATE_CONNECTING:
								// Situation normal. Start the connected thread.
								service.Connected(socket, socket.RemoteDevice, socketType);
								break;
							case STATE_NONE:
							case STATE_CONNECTED:
								try
								{
									socket.Close();
								}
								catch (Java.IO.IOException e)
								{
									Log.Error(TAG, "Could not close unwanted socket", e);
								}
								break;
						}
					}
				}
			}
		}

		public void Cancel()
		{
			try
			{
				serverSocket.Close();
			}
			catch (Java.IO.IOException e)
			{
				Log.Error(TAG, "close() of server failed", e);
			}
		}
	}

	/// <summary>
	/// This thread runs while attempting to make an outgoing connection
	/// with a device. It runs straight through; the connection either
	/// succeeds or fails.
	/// </summary>
	protected class ConnectThread : Thread
	{
		BluetoothSocket socket;
		BluetoothDevice device;
		BluetoothChatService service;
		string socketType;

		public ConnectThread(BluetoothDevice device, BluetoothChatService service, bool secure)
		{
			this.device = device;
			this.service = service;
			BluetoothSocket tmp = null;
			socketType = secure ? "Secure" : "Insecure";

			try
			{
				if (secure)
				{
					tmp = device.CreateRfcommSocketToServiceRecord(MY_UUID_SECURE);
				}
				else
				{
					tmp = device.CreateInsecureRfcommSocketToServiceRecord(MY_UUID_INSECURE);
				}

			}
			catch (Java.IO.IOException e)
			{
				Log.Error(TAG, "create() failed", e);
			}
			socket = tmp;
			service.state = STATE_CONNECTING;
		}

		public override void Run()
		{
			Name = $"ConnectThread_{socketType}";

			// Always cancel discovery because it will slow down connection
			service.btAdapter.CancelDiscovery();

			// Make a connection to the BluetoothSocket
			try
			{
				// This is a blocking call and will only return on a
				// successful connection or an exception
				socket.Connect();
			}
			catch (Java.IO.IOException)
			{
				// Close the socket
				try
				{
					socket.Close();
				}
				catch (Java.IO.IOException e2)
				{
					Log.Error(TAG, $"unable to close() {socketType} socket during connection failure.", e2);
				}

				// Start the service over to restart listening mode
				service.ConnectionFailed();
				return;
			}

			// Reset the ConnectThread because we're done
			lock (this)
			{
				service.connectThread = null;
			}

			// Start the connected thread
			service.Connected(socket, device, socketType);
		}

		public void Cancel()
		{
			try
			{
				socket.Close();
			}
			catch (Java.IO.IOException e)
			{
				Log.Error(TAG, "close() of connect socket failed", e);
			}
		}
	}

	/// <summary>
	/// This thread runs during a connection with a remote device.
	/// It handles all incoming and outgoing transmissions.
	/// </summary>
	class ConnectedThread : Thread
	{
		BluetoothSocket socket;
		Stream inStream;
		Stream outStream;
		BluetoothChatService service;

		public ConnectedThread(BluetoothSocket socket, BluetoothChatService service, string socketType)
		{
			Log.Debug(TAG, $"create ConnectedThread: {socketType}");
			this.socket = socket;
			this.service = service;
			Stream tmpIn = null;
			Stream tmpOut = null;

			// Get the BluetoothSocket input and output streams
			try
			{
				tmpIn = socket.InputStream;
				tmpOut = socket.OutputStream;
			}
			catch (Java.IO.IOException e)
			{
				Log.Error(TAG, "temp sockets not created", e);
			}

			inStream = tmpIn;
			outStream = tmpOut;
			service.state = STATE_CONNECTED;
		}

		public override void Run()
		{
			Log.Info(TAG, "BEGIN mConnectedThread");
			byte[] buffer = new byte[1024];
			int bytes;

			// Keep listening to the InputStream while connected
			while (service.GetState() == STATE_CONNECTED)
			{
				try
				{
					// Read from the InputStream
					bytes = inStream.Read(buffer, 0, buffer.Length);

					// Send the obtained bytes to the UI Activity
					service.handler
					       .ObtainMessage(Constants.MESSAGE_READ, bytes, -1, buffer)
					       .SendToTarget();
				}
				catch (Java.IO.IOException e)
				{
					Log.Error(TAG, "disconnected", e);
					service.ConnectionLost();
					break;
				}
			}
		}

		/// <summary>
		/// Write to the connected OutStream.
		/// </summary>
		/// <param name='buffer'>
		/// The bytes to write
		/// </param>
		public void Write(byte[] buffer)
		{
			try
			{
				outStream.Write(buffer, 0, buffer.Length);

				// Share the sent message back to the UI Activity
				service.handler
				       .ObtainMessage(Constants.MESSAGE_WRITE, -1, -1, buffer)
				       .SendToTarget();
			}
			catch (Java.IO.IOException e)
			{
				Log.Error(TAG, "Exception during write", e);
			}
		}

		public void Cancel()
		{
			try
			{
				socket.Close();
			}
			catch (Java.IO.IOException e)
			{
				Log.Error(TAG, "close() of connect socket failed", e);
			}
		}
	}
}