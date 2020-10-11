using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using LinqToTwitter;

namespace cYo.Projects.ComicRack.Viewer
{
	public static class Twitter
	{
		private const int TweetLength = 144;

		private const int TweetHashSize = 25;

		private const string TweetComicRackTag = "#comicrack";

		public static void Tweet(IWin32Window parent, string text, Bitmap thumbnail)
		{
			if (!Program.Settings.HasTwitterAccess)
			{
				MessageBox.Show(parent, TR.Messages["TwitterAuth", "A Browser window will open to authorize ComicRack to post on Twitter."], TR.Messages["Attention", "Attention"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			PinAuthorizer pinAuthorizer = new PinAuthorizer
			{
				Credentials = new InMemoryCredentials
				{
					ConsumerKey = "lc7K38IqLGD9tlvj9DP42w",
					ConsumerSecret = "RgiRdp7tjUg1F3120P1N7cWB2AN7tSD3HjjOVuQtaW4",
					OAuthToken = Program.Settings.TwitterOAuthToken,
					AccessToken = Program.Settings.TwitterAccessToken,
					UserId = Program.Settings.TwitterUserId,
					ScreenName = Program.Settings.TwitterScreenName
				},
				UseCompression = true,
				AuthAccessType = AuthAccessType.NoChange,
				GoToTwitterAuthorization = delegate(string pageLink)
				{
					Process.Start(pageLink);
				},
				GetPin = () => TwitterPinDialog.GetPin(parent)
			};
			try
			{
				pinAuthorizer.Authorize();
				TwitterContext twitterContext = new TwitterContext(pinAuthorizer);
				if (thumbnail == null)
				{
					twitterContext.UpdateStatus(text);
				}
				else
				{
					Media media = new Media();
					media.ContentType = MediaContentType.Jpeg;
					media.Data = thumbnail.ImageToBytes(ImageFormat.Jpeg);
					twitterContext.TweetWithMedia(text, possiblySensitive: false, cYo.Common.Collections.ListExtensions.AsEnumerable<Media>(media).ToList());
				}
				Program.Settings.TwitterOAuthToken = pinAuthorizer.Credentials.OAuthToken;
				Program.Settings.TwitterAccessToken = pinAuthorizer.Credentials.AccessToken;
				Program.Settings.TwitterUserId = pinAuthorizer.Credentials.UserId;
				Program.Settings.TwitterScreenName = pinAuthorizer.Credentials.ScreenName;
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("duplicate"))
				{
					return;
				}
				MessageBox.Show(parent, ex.Message, TR.Messages["Error", "Error"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				if (ex.Message.Contains("auth"))
				{
					bool hasTwitterAccess = Program.Settings.HasTwitterAccess;
					Program.Settings.ResetTwitter();
					if (hasTwitterAccess)
					{
						Tweet(parent, text, thumbnail);
					}
				}
			}
		}

		public static void Tweet(IWin32Window parent, ComicBook cb, Bitmap thumbnail)
		{
			Tweet(parent, CreateReviewTweet(cb), thumbnail);
		}

		public static string CreateTweet(string seriesHashTag, string text, float rating)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (rating > 0f)
			{
				stringBuilder.Append(CreateRatingText(rating, symbolic: true));
				stringBuilder.Append(" ");
			}
			stringBuilder.Append("#");
			stringBuilder.Append(seriesHashTag);
			stringBuilder.Append(" ");
			stringBuilder.Append("#comicrack");
			stringBuilder.Append(" ");
			if (!string.IsNullOrEmpty(text))
			{
				int num = 143 - stringBuilder.Length;
				if (text.Length > num)
				{
					text = text.Substring(0, num - 3) + "...";
				}
				stringBuilder.Insert(0, " ");
				stringBuilder.Insert(0, text);
			}
			return stringBuilder.ToString();
		}

		public static string CreateReviewTweet(ComicBook cb)
		{
			string text = CreateSeriesHashTag(cb, includeVolume: false);
			if (text == null)
			{
				return null;
			}
			return CreateTweet(text, cb.Review, cb.Rating);
		}

		public static string CreateRatingText(float rating, bool symbolic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = (int)rating;
			if (!symbolic)
			{
				stringBuilder.Append(num);
			}
			else
			{
				for (int i = 0; i < num; i++)
				{
					stringBuilder.Append("*");
				}
			}
			rating -= (float)num;
			if (rating >= 0.75f)
			{
				stringBuilder.Append("¾");
			}
			else if (rating >= 0.5f)
			{
				stringBuilder.Append("½");
			}
			else if (rating >= 0.25f)
			{
				stringBuilder.Append("¼");
			}
			return stringBuilder.ToString();
		}

		public static string CreateSeriesHashTag(ComicBook cb, bool includeVolume)
		{
			if (string.IsNullOrEmpty(cb.ShadowSeries))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(cb.ShadowSeries.ShortenText(25));
			if (!string.IsNullOrEmpty(cb.ShadowNumber))
			{
				if (char.IsDigit(stringBuilder[stringBuilder.Length - 1]))
				{
					stringBuilder.Append("_");
				}
				stringBuilder.Append(cb.ShadowNumber.ShortenText());
			}
			if (includeVolume && cb.ShadowVolume > 0)
			{
				stringBuilder.Append("V");
				stringBuilder.Append(cb.ShadowVolume);
			}
			return stringBuilder.ToString();
		}
	}
}
