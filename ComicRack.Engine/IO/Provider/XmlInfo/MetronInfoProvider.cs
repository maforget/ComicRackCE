using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.Threading;
using cYo.Common.Xml;

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
					Publisher = metronInfo.Publisher.Name,
					Imprint = metronInfo.Publisher.Imprint.Value,
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
					Letterer = string.Join(delimiter, metronInfo.Credits.SelectMany(c => 
						c.Roles.Where(r =>
							r.Value == RoleValues.Letterer
						).Select(r => c.Creator.Value))),
					CoverArtist = string.Join(delimiter, metronInfo.Credits.SelectMany(c => 
						c.Roles.Where(r =>
							r.Value == RoleValues.Cover
						).Select(r => c.Creator.Value))),
					Series = metronInfo.Series.Name,
					Number = metronInfo.Number,
					Count = metronInfo.Series.IssueCount,
					AlternateSeries = metronInfo.Arcs.FirstOrDefault()?.Name,
					AlternateNumber = metronInfo.Arcs.FirstOrDefault()?.Number.ToString(),
					StoryArc = metronInfo.Stories.FirstOrDefault()?.Value,
					Summary = metronInfo.Summary,
					Volume = metronInfo.Series?.Volume ?? -1,
					Year = metronInfo.CoverDate.Year,
					Month = metronInfo.CoverDate.Month,
					Day = metronInfo.CoverDate.Day,
					Notes = metronInfo.Notes,
					Genre = string.Join(delimiter, metronInfo.Genres.Select(g => g.Value)),
					Web = metronInfo.UrLs.Where(u => u.Primary)?.FirstOrDefault()?.Value ?? metronInfo.UrLs.FirstOrDefault()?.Value,
					PageCount = metronInfo.PageCount,
					LanguageISO = metronInfo.Series.Lang,
					AgeRating = metronInfo.AgeRating.ToString(),
					Characters = string.Join(delimiter, metronInfo.Characters.Select(c => c.Value)),
					Teams = string.Join(delimiter, metronInfo.Teams.Select(t => t.Value)),
					Locations = string.Join(delimiter, metronInfo.Locations.Select(t => t.Value)),
					Tags = string.Join(delimiter, metronInfo.Tags.Select(t => t.Value)),
				};

				return comicInfo;
			}
		}
	}
}
