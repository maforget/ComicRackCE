using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using cYo.Common.Runtime;
using System.Runtime.CompilerServices;

namespace cYo.Common.Net
{
	public class GithubAPI(string sha1 = "", string headTag = "nightly")
	{
		const string apiURL = @"https://api.github.com/repos/maforget/ComicRackCE/compare/{0}...{1}";
		private GithubCompareAPI response = null;

		public string Commit { get; } = !string.IsNullOrEmpty(sha1) ? sha1 : GitVersion.CurrentCommit;
		public string UpdateMessage => GetCommitsMessages();
        public bool IsUpdateAvailable => CheckIfUpdateAvailable();

        private bool CheckIfUpdateAvailable()
		{
			if (response == null || response.status.Contains("Error"))
				return false;

			return response.status == "ahead"; //"ahead" means that we have a new update available. "identical" or "behind" are also possible values.
		}

		public async Task ExecuteAsync()
		{
			response = await Task.Run(() => GetResponse());
		}

		private GithubCompareAPI GetResponse()
		{
			try
			{
				string url = string.Format(apiURL, Commit, headTag);
				string rawJson = LoadText(url);
				GithubCompareAPI json = JsonSerializer.Deserialize<GithubCompareAPI>(rawJson);
				return json;
			}
			catch (Exception ex)
			{
				return new GithubCompareAPI()
				{
					status = $"Error - {ex.Message}"
				};
			}
		}

		private string GetCommitsMessages()
		{
			if (response == null)
				return string.Empty;

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < response.commits.Length; i++)
			{
				Commit item = response.commits[i];
				sb.AppendLine(item.commit.message);

				if(i < response.commits.Length - 1)
					sb.AppendLine(new string('-', 100));
			}
			return sb.ToString();
		}

		public static string LoadText(string url)
		{
			return HttpAccess.ReadText(url);
		}
	}


	#region JSON response object
	public class GithubCompareAPI
	{
		public string status { get; set; }
		public int ahead_by { get; set; }
		public int behind_by { get; set; }
		public int total_commits { get; set; }
		public Commit[] commits { get; set; }
	}

	public class Commit
	{
		public string sha { get; set; }
		public CommitDetail commit { get; set; }
		public string html_url { get; set; }
	}

	public class CommitDetail
	{
		public string message { get; set; }
	}
	#endregion

}
