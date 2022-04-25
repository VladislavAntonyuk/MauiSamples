namespace MauiPaint.Platforms.Android.Services;

using global::Android;
using global::Android.App;
using global::Android.Content;
using global::Android.Graphics;
using global::Android.OS;
using global::Android.Util;
using global::Android.Views;
using global::Android.Widget;
using Java.IO;

public class FileDialog
{
	private AutoResetEvent? autoResetEvent;
	private const int FileOpen = 0;
	private const int FileSave = 1;
	private const int FolderChoose = 2;
	private readonly int selectType;
	private readonly string? mSdcardDirectory;
	private readonly Context? mContext;
	private TextView? mTitleView1;
	private TextView? mTitleView;
	private readonly string defaultFileName = "default";
	private string selectedFileName;
	private EditText? inputText;

	private string mDir = "";
	private List<string>? mSubdirs;
	private ArrayAdapter<string>? mListAdapter;
	private readonly bool mGoToUpper;
	private AlertDialog? dirsDialog;
	private string? result;

	//////////////////////////////////////////////////////
	// Callback interface for selected directory
	//////////////////////////////////////////////////////


	public FileDialog(Context? context, FileSelectionMode mode, string? fileExtension = ".png")
	{
		defaultFileName += fileExtension;
		selectedFileName = defaultFileName;
		switch (mode)
		{
			case FileSelectionMode.FileOpen:
				selectType = FileOpen;
				break;
			case FileSelectionMode.FileSave:
				selectType = FileSave;
				break;
			case FileSelectionMode.FolderChoose:
				selectType = FolderChoose;
				break;
			case FileSelectionMode.FileOpenRoot:
				selectType = FileOpen;
				mGoToUpper = true;
				break;
			case FileSelectionMode.FileSaveRoot:
				selectType = FileSave;
				mGoToUpper = true;
				break;
			case FileSelectionMode.FolderChooseRoot:
				selectType = FolderChoose;
				mGoToUpper = true;
				break;
			default:
				selectType = FileOpen;
				break;
		}


		mContext = context;
		mSdcardDirectory = Environment.RootDirectory.AbsolutePath;

		try
		{
			mSdcardDirectory = new File(mSdcardDirectory).CanonicalPath;
		}
		catch (IOException)
		{
		}
	}

	public enum FileSelectionMode
	{
		FileOpen,
		FileSave,
		FolderChoose,
		FileOpenRoot,
		FileSaveRoot,
		FolderChooseRoot
	}


	public async Task<string?> GetFileOrDirectoryAsync(string? dir)
	{
		if (dir is null)
		{
			return null;
		}

		File dirFile = new File(dir);
		while (!dirFile.Exists() || !dirFile.IsDirectory)
		{
			dir = dirFile.Parent;
			if (dir == null)
			{
				return null;
			}

			dirFile = new File(dir);
			Log.Debug("~~~~~", "dir=" + dir);
		}

		Log.Debug("~~~~~", "dir=" + dir);
		try
		{
			dir = new File(dir).CanonicalPath;
		}
		catch (IOException)
		{
			return result;
		}

		mDir = dir;
		mSubdirs = GetDirectories(dir);

		AlertDialog.Builder dialogBuilder = CreateDirectoryChooserDialog(dir, mSubdirs, (sender, args) =>
		{
			string mDirOld = mDir;
			string sel = "" + ((AlertDialog?)sender)?.ListView?.Adapter?.GetItem(args.Which);
			if (sel[^1] == '/')
				sel = sel.Substring(0, sel.Length - 1);

			// Navigate into the sub-directory
			if (sel.Equals(".."))
			{
				mDir = mDir.Substring(0, mDir.LastIndexOf("/", StringComparison.Ordinal));
				if ("".Equals(mDir))
				{
					mDir = "/";
				}
			}
			else
			{
				mDir += "/" + sel;
			}

			selectedFileName = defaultFileName;

			if ((new File(mDir).IsFile)) // If the selection is a regular file
			{
				mDir = mDirOld;
				selectedFileName = sel;
			}

			UpdateDirectory();
		});
		dialogBuilder.SetPositiveButton("OK", (_, _) =>
		{
			// Current directory chosen
			// Call registered listener supplied with the chosen directory

			{
				if (selectType == FileOpen || selectType == FileSave)
				{
					selectedFileName = inputText?.Text + "";
					result = mDir + "/" + selectedFileName;
					autoResetEvent?.Set();
				}
				else
				{
					result = mDir;
					autoResetEvent?.Set();
				}
			}

		});
		dialogBuilder.SetNegativeButton("Cancel", (_, _) => { });
		dirsDialog = dialogBuilder.Create();

		if (dirsDialog != null)
		{
			dirsDialog.CancelEvent += (_, _) => { autoResetEvent?.Set(); };
			dirsDialog.DismissEvent += (_, _) => { autoResetEvent?.Set(); };
			// Show directory chooser dialog
			autoResetEvent = new AutoResetEvent(false);
			dirsDialog.Show();
		}

		await Task.Run(() => { autoResetEvent?.WaitOne(); });

		return result;
	}


	private bool CreateSubDir(string newDir)
	{
		File newDirFile = new File(newDir);
		if (!newDirFile.Exists())
			return newDirFile.Mkdir();
		return false;
	}

	private List<string> GetDirectories(string dir)
	{
		List<string> dirs = new List<string>();
		try
		{
			File dirFile = new File(dir);

			// if directory is not the base sd card directory add ".." for going up one directory
			if ((mGoToUpper || !mDir.Equals(mSdcardDirectory)) && !"/".Equals(mDir))
			{
				dirs.Add("..");
			}

			Log.Debug("~~~~", "m_dir=" + mDir);
			if (!dirFile.Exists() || !dirFile.IsDirectory)
			{
				return dirs;
			}

			foreach (File file in dirFile.ListFiles() ?? Array.Empty<File>())
			{
				if (file.IsDirectory)
				{
					// Add "/" to directory names to identify them in the list
					dirs.Add(file.Name + "/");
				}
				else if (selectType == FileSave || selectType == FileOpen)
				{
					// Add file names to the list if we are doing a file save or file open operation
					dirs.Add(file.Name);
				}
			}
		}
		catch (Exception)
		{
			// ignored
		}

		dirs.Sort();
		return dirs;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////
	//////                                   START DIALOG DEFINITION                                    //////
	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	private AlertDialog.Builder CreateDirectoryChooserDialog(string title,
		List<string> listItems,
		EventHandler<DialogClickEventArgs> onClickListener)
	{
		AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(mContext);
		////////////////////////////////////////////////
		// Create title text showing file select type // 
		////////////////////////////////////////////////
		mTitleView1 = new TextView(mContext);
		mTitleView1.LayoutParameters =
			new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
		//m_titleView1.setTextAppearance(m_context, android.R.style.TextAppearance_Large);
		//m_titleView1.setTextColor( m_context.getResources().getColor(android.R.color.black) );

		if (selectType == FileOpen)
			mTitleView1.Text = "Open";
		if (selectType == FileSave)
			mTitleView1.Text = "Save As";
		if (selectType == FolderChoose)
			mTitleView1.Text = "Select folder";

		//need to make this a variable Save as, Open, Select Directory
		mTitleView1.Gravity = GravityFlags.CenterVertical;
		//_mTitleView1.SetBackgroundColor(Color.DarkGray); // dark gray 	-12303292
		mTitleView1.SetTextColor(Color.Black);
		mTitleView1.SetTextSize(ComplexUnitType.Dip, 18);
		mTitleView1.SetTypeface(null, TypefaceStyle.Bold);
		// Create custom view for AlertDialog title
		LinearLayout titleLayout1 = new LinearLayout(mContext);
		titleLayout1.Orientation = Orientation.Vertical;
		titleLayout1.AddView(mTitleView1);

		if (selectType == FileSave)
		{
			///////////////////////////////
			// Create New Folder Button  //
			///////////////////////////////
			Button newDirButton = new Button(mContext);
			newDirButton.LayoutParameters =
				new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
			newDirButton.Text = "New Folder";
			newDirButton.Click += (_, _) =>
			{
				EditText input = new EditText(mContext);
				new AlertDialog.Builder(mContext).SetTitle("New Folder Name")
				                                 ?.SetView(input)
				                                 ?.SetPositiveButton("OK", (_, _) =>
				                                 {
					                                 string? newDirName = input.Text;
					                                 // Create new directory
					                                 if (CreateSubDir(mDir + "/" + newDirName))
					                                 {
						                                 // Navigate into the new directory
						                                 mDir += "/" + newDirName;
						                                 UpdateDirectory();
					                                 }
					                                 else
					                                 {
						                                 Toast.MakeText(mContext,
						                                                "Failed to create '" + newDirName + "' folder",
						                                                ToastLength.Short)
						                                      ?.Show();
					                                 }
				                                 })
				                                 ?.SetNegativeButton("Cancel", (_, _) => { })
				                                 ?.Show();
			};
			titleLayout1.AddView(newDirButton);
		}

		/////////////////////////////////////////////////////
		// Create View with folder path and entry text box // 
		/////////////////////////////////////////////////////
		LinearLayout titleLayout = new LinearLayout(mContext);
		titleLayout.Orientation = Orientation.Vertical;



		var currentSelection = new TextView(mContext);
		currentSelection.LayoutParameters =
			new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
		currentSelection.SetTextColor(Color.Black);
		currentSelection.Gravity = GravityFlags.CenterVertical;
		currentSelection.Text = "Current selection:";
		currentSelection.SetTextSize(ComplexUnitType.Dip, 12);
		currentSelection.SetTypeface(null, TypefaceStyle.Bold);

		titleLayout.AddView(currentSelection);

		mTitleView = new TextView(mContext);
		mTitleView.LayoutParameters =
			new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
		mTitleView.SetTextColor(Color.Black);
		mTitleView.Gravity = GravityFlags.CenterVertical;
		mTitleView.Text = title;
		mTitleView.SetTextSize(ComplexUnitType.Dip, 10);
		mTitleView.SetTypeface(null, TypefaceStyle.Normal);

		titleLayout.AddView(mTitleView);

		if (selectType == FileOpen || selectType == FileSave)
		{
			inputText = new EditText(mContext);
			inputText.Text = defaultFileName;
			titleLayout.AddView(inputText);
		}

		//////////////////////////////////////////
		// Set Views and Finish Dialog builder  //
		//////////////////////////////////////////
		dialogBuilder.SetView(titleLayout);
		dialogBuilder.SetCustomTitle(titleLayout1);
		mListAdapter = CreateListAdapter(listItems);
		dialogBuilder.SetSingleChoiceItems(mListAdapter, -1, onClickListener);
		dialogBuilder.SetCancelable(true);
		return dialogBuilder;
	}


	private void UpdateDirectory()
	{
		mSubdirs?.Clear();
		mSubdirs?.AddRange(GetDirectories(mDir));
		if (mTitleView != null)
		{
			mTitleView.Text = mDir;
		}

		if (dirsDialog?.ListView != null)
		{
			dirsDialog.ListView.Adapter = null;
			dirsDialog.ListView.Adapter = CreateListAdapter(mSubdirs);
		}

		//#scorch
		if ((selectType == FileSave || selectType == FileOpen) && inputText != null)
		{
			inputText.Text = selectedFileName;
		}
	}

	private ArrayAdapter<string> CreateListAdapter(List<string>? items)
	{
		var adapter = new SimpleArrayAdapter(mContext ?? throw new InvalidOperationException(), Resource.Layout.SelectDialogItem,
		                                     Resource.Id.Text1, items ?? Array.Empty<string>().ToList());
		return adapter;
	}

	private class SimpleArrayAdapter : ArrayAdapter<string>
	{
		public SimpleArrayAdapter(Context context, int resource, int textViewResourceId, IList<string> objects) : base(
			context, resource, textViewResourceId, objects)
		{
		}

		public override View GetView(int position, View? convertView, ViewGroup parent)
		{
			View v = base.GetView(position, convertView, parent);
			if (v is TextView textView)
			{
				// Enable list item (directory) text wrapping
				if (textView.LayoutParameters != null)
				{
					textView.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent;
				}

				textView.Ellipsize = null;
			}

			return v;
		}
	}
}