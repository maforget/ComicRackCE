using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using cYo.Common.Cryptography;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.IO.Network;
using cYo.Projects.ComicRack.Viewer.Dialogs;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class RemoteConnectionView : SubView
	{
		private Thread thread;

		private bool cancelConnection;

		private bool openNow;

		private bool autoOpen;

		private Image oldImage;

		private string textConnect = TR.Default["Connect", "Connect"];

		private string textCancel = TR.Default["Cancel", "Cancel"];

		public MainView View
		{
			get;
			private set;
		}

		public ComicLibraryClient Client
		{
			get;
			private set;
		}

		private TabBar.TabBarItem Tab => base.Tag as TabBar.TabBarItem;

		private Image TabImage
		{
			get
			{
				if (Tab == null)
				{
					return null;
				}
				return Tab.Image;
			}
			set
			{
				if (Tab != null)
				{
					Tab.Image = value;
				}
			}
		}

		public RemoteConnectionView(MainView view, ComicLibraryClient client, MainView.AddRemoteLibraryOptions options)
		{
			InitializeComponent();
			View = view;
			Client = client;
			openNow = options.HasFlag(MainView.AddRemoteLibraryOptions.Open);
			autoOpen = openNow && options.HasFlag(MainView.AddRemoteLibraryOptions.Auto);
			base.AutoScrollMinSize = panelCenter.Size;
			lblServerName.Text = client.ShareInformation.Name;
			lblServerDescription.Text = client.ShareInformation.Comment;
		}

		private void Connect()
		{
			cancelConnection = false;
			connectionAnimation.Visible = true;
			btConnect.Text = textCancel;
			oldImage = TabImage;
			TabImage = Resources.SmallBallAnimation;
			thread = ThreadUtility.RunInBackground("Connect to remote library", ConnectToLibrary, ThreadPriority.Normal);
		}

		private void ConnectToLibrary()
		{
			try
			{
				ComicLibrary cl = null;
				bool firstTime = true;
				if (Client.ShareInformation.IsProtected)
				{
					string passwordFromCache = Program.Settings.GetPasswordFromCache(Client.ShareInformation.Name);
					if (!string.IsNullOrEmpty(passwordFromCache))
					{
						Client.Password = passwordFromCache;
					}
				}
				SetMessage(TR.Messages["ConnectServerText", "Opening connection to the remote Server"]);
				while (!cancelConnection && !Client.Connect() && !cancelConnection)
				{
					if (autoOpen)
					{
						cancelConnection = true;
						autoOpen = false;
					}
					else
					{
						this.Invoke(delegate
						{
							using (PasswordDialog passwordDialog = new PasswordDialog())
							{
								if (firstTime)
								{
									passwordDialog.Description = StringUtility.Format(TR.Messages["PasswordNeeded", "A password is needed for the remote Library '{0}':"], Client.ShareInformation.Name);
								}
								else
								{
									passwordDialog.Description = StringUtility.Format(TR.Messages["WrongPassword", "The specified password for the Library'{0}' is not correct. Please try again:"], Client.ShareInformation.Name);
								}
								if (passwordDialog.ShowDialog(this) == DialogResult.Cancel)
								{
									cancelConnection = true;
								}
								else
								{
									Client.Password = Password.CreateHash(passwordDialog.Password);
									if (passwordDialog.RememberPassword)
									{
										Program.Settings.AddPasswordToCache(Client.ShareInformation.Name, Client.Password);
									}
								}
							}
						});
					}
					firstTime = false;
				}
				if (cancelConnection)
				{
					throw new Exception();
				}
				SetMessage(TR.Messages["GetServerLibraryText", "Retrieving the shared Library from the Server"]);
				cl = Client.GetRemoteLibrary();
				if (cl == null || cancelConnection)
				{
					throw new Exception();
				}
				InvokeAction(delegate
				{
					ComicListLibraryBrowser cllb = new ComicListLibraryBrowser(cl);
					TabBar.TabBarItem tsb = base.Tag as TabBar.TabBarItem;
					ComicExplorerView ev = View.AddExplorerView(cl, cllb, tsb, Program.Settings.GetRemoteExplorerViewSetting(cl.Id));
					ev.Main = base.Main;
					ev.Tag = Client.ShareInformation.Uri;
					cllb.RefreshLists += View.OnRefreshRemoteLists;
					cllb.LibraryChanged += delegate
					{
						ev.ComicBrowser.Library = cllb.Library;
					};
					cllb.Tag = Client;
					ev.ComicBrowser.ComicEditMode = cl.EditMode;
					View.RefreshView();
					Dispose();
				});
			}
			catch (Exception)
			{
				OnConnectionCancelled(TR.Messages["FailedRetrieveDatabase", "Failed to retrieve the Database from the Server!"]);
			}
		}

		private void InvokeAction(Action action)
		{
			BeginInvoke(action);
		}

		private void SetMessage(string text)
		{
			InvokeAction(delegate
			{
				lblMessage.Text = text;
				lblMessage.Visible = true;
			});
		}

		private void OnConnectionCancelled(string message = null)
		{
			if (cancelConnection)
			{
				message = null;
			}
			InvokeAction(delegate
			{
				lblMessage.Text = message;
				lblMessage.Visible = message != null;
				connectionAnimation.Visible = false;
				TabImage = oldImage;
				btConnect.Text = textConnect;
				thread = null;
			});
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (panelCenter != null)
			{
				if (base.ClientRectangle.Height > panelCenter.Height)
				{
					panelCenter.Top = (base.ClientRectangle.Height - panelCenter.Height) / 2;
				}
				if (base.ClientRectangle.Width > panelCenter.Width)
				{
					panelCenter.Left = (base.ClientRectangle.Width - panelCenter.Width) / 2;
				}
			}
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			if (openNow)
			{
				InvokeAction(Connect);
			}
		}

		private void btConnect_Click(object sender, EventArgs e)
		{
			Thread thread = this.thread;
			if (thread == null)
			{
				Connect();
				return;
			}
			TabImage = oldImage;
			connectionAnimation.Visible = false;
			Update();
			cancelConnection = true;
			using (new WaitCursor(this))
			{
				if (!thread.Join(2000))
				{
					thread.Abort();
					thread.Join();
				}
			}
		}
	}
}
