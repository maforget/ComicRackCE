using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.Threading;
using cYo.Common.Windows;
using cYo.Common.Xml;
using cYo.Projects.ComicRack.Engine.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo
{
	[XmlInfoFile("MetronInfo.xml", 1)]
	public class MetronInfoProvider : XmlInfoProvider<MetronInfo>
	{
		public override ComicInfo ToComicInfo(MetronInfo metronInfo)
		{
			if (metronInfo == null)
				return null;

			const string delimiter = ", ";

			using (ItemMonitor.Lock(this))
			{
				ComicInfo comicInfo = new ComicInfo()
				{
					Publisher = metronInfo.Publisher?.Name ?? string.Empty,
					Imprint = metronInfo.Publisher?.Imprint?.Value ?? string.Empty,
					Writer = string.Join(delimiter, metronInfo.Credits.SelectMany(c =>
						c.Roles.Where(r =>
							r.Value == RoleValues.Writer ||
							r.Value == RoleValues.Plot
						).Select(r => c.Creator.Value))),
					Penciller = string.Join(delimiter, metronInfo.Credits.SelectMany(c =>
						c.Roles.Where(r =>
							r.Value == RoleValues.Penciller
						).Select(r => c.Creator.Value))),
					Inker = string.Join(delimiter, metronInfo.Credits.SelectMany(c =>
						c.Roles.Where(r =>
							r.Value == RoleValues.Inker ||
							r.Value == RoleValues.InkAssists
						).Select(r => c.Creator.Value))),
					Colorist = string.Join(delimiter, metronInfo.Credits.SelectMany(c =>
						c.Roles.Where(r =>
							r.Value.ToString().Contains("Color")
						).Select(r => c.Creator.Value))),
					Editor = string.Join(delimiter, metronInfo.Credits.SelectMany(c =>
						c.Roles.Where(r =>
							r.Value.ToString().Contains("Editor")
						).Select(r => c.Creator.Value))),
					Translator = string.Join(delimiter, metronInfo.Credits.SelectMany(c =>
						c.Roles.Where(r =>
							r.Value.ToString().Contains("Translator")
						).Select(r => c.Creator.Value))),
					Letterer = string.Join(delimiter, metronInfo.Credits.SelectMany(c =>
						c.Roles.Where(r =>
							r.Value == RoleValues.Letterer
						).Select(r => c.Creator.Value))),
					CoverArtist = string.Join(delimiter, metronInfo.Credits.SelectMany(c =>
						c.Roles.Where(r =>
							r.Value == RoleValues.Cover
						).Select(r => c.Creator.Value))),
					Series = metronInfo.Series?.Name ?? string.Empty,
					Number = metronInfo.Number ?? string.Empty,
					Count = metronInfo.Series?.IssueCountSpecified is null or false ? -1 : metronInfo.Series.IssueCount,
					AlternateSeries = metronInfo.Arcs?.FirstOrDefault()?.Name ?? string.Empty,
					AlternateNumber = metronInfo.Arcs?.FirstOrDefault()?.Number.ToString() ?? string.Empty,
					Title = metronInfo.Stories?.FirstOrDefault()?.Value ?? string.Empty, //Some files seem to set the Title as the 1st Story
					StoryArc = metronInfo.Stories?.Skip(1).FirstOrDefault()?.Value ?? string.Empty, //So we set the StoryArc as the 2nd Story
					Summary = metronInfo.Summary ?? string.Empty,
					Volume = metronInfo.Series is null ? -1 : metronInfo.Series.VolumeSpecified ? metronInfo.Series.Volume : int.TryParse(metronInfo.Series.StartYear, out int vol) ? vol : -1,// Use Volume, if not use StartYear
					Year = metronInfo.CoverDateSpecified ? metronInfo.CoverDate.Year : -1,
					Month = metronInfo.CoverDateSpecified ? metronInfo.CoverDate.Month : -1,
					Day = metronInfo.CoverDateSpecified ? metronInfo.CoverDate.Day : -1,
					Notes = metronInfo.Notes ?? string.Empty,
					Genre = string.Join(delimiter, metronInfo.Genres.Select(g => g.Value)),
					Web = (metronInfo.UrLs.Where(u => u.Primary)?.FirstOrDefault()?.Value ?? metronInfo.UrLs.FirstOrDefault()?.Value) ?? string.Empty,
					PageCount = metronInfo.PageCount,
					LanguageISO = metronInfo.Series?.Lang ?? string.Empty,
					AgeRating = metronInfo.AgeRating == AgeRatingType.Unknown ? string.Empty : LocalizeUtility.LocalizeEnum(typeof(AgeRatingType), (int)metronInfo.AgeRating),
					Characters = string.Join(delimiter, metronInfo.Characters.Select(c => c.Value)),
					Teams = string.Join(delimiter, metronInfo.Teams.Select(t => t.Value)),
					Locations = string.Join(delimiter, metronInfo.Locations.Select(t => t.Value)),
					Tags = string.Join(delimiter, metronInfo.Tags.Select(t => t.Value)),
					Format = metronInfo.Series?.FormatSpecified is null or false ? string.Empty : ParseFormat(metronInfo),
				};

				return comicInfo;
			}
		}

		private static string ParseFormat(MetronInfo metronInfo)
		{
			return (metronInfo.Series?.Format) switch
			{
				FormatType.TradePaperback => "TPB",
				_ => LocalizeUtility.LocalizeEnum(typeof(FormatType), (int)metronInfo.Series?.Format),
			};
		}
	}
}
