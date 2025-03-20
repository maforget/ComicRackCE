using System;
using System.Collections.Generic;
using cYo.Common.Text;
using System.IO;
using System.Security.Cryptography;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	public class ComicPageInfoCollection : List<ComicPageInfo>
	{
		public ComicPageInfo FindByPage(int page)
		{
			using (ItemMonitor.Lock(this))
			{
				return (page < 0 || page >= base.Count) ? ComicPageInfo.Empty : base[page];
			}
		}

		public ComicPageInfo FindByImageIndex(int imageIndex)
		{
			return Find((ComicPageInfo cpi) => cpi.ImageIndex == imageIndex);
		}

		public bool PagesAreEqual(ComicPageInfoCollection pages)
		{
			if (base.Count != pages.Count)
			{
				return false;
			}
			for (int i = 0; i < base.Count; i++)
			{
				if (!object.Equals(base[i], pages[i]))
				{
					return false;
				}
			}
			return true;
		}

		public void ResetPageSequence()
		{
			Sort((ComicPageInfo a, ComicPageInfo b) => a.ImageIndex.CompareTo(b.ImageIndex));
		}

		public void Consolidate()
		{
			if (base.Count < 2)
			{
				return;
			}
			HashSet<int> hashSet = new HashSet<int>();
			using (ItemMonitor.Lock(this))
			{
				for (int i = 0; i < base.Count; i++)
				{
					ComicPageInfo comicPageInfo = base[i];
					if (hashSet.Contains(comicPageInfo.ImageIndex))
					{
						RemoveAt(i--);
					}
					else
					{
						hashSet.Add(comicPageInfo.ImageIndex);
					}
				}
			}
		}

		public int SeekBookmark(int page, int direction)
		{
			direction = Math.Sign(direction);
			while (page >= 0 && page < base.Count)
			{
				if (base[page].IsBookmark)
				{
					return page;
				}
				page += direction;
			}
			return -1;
		}

		public string CreatePageHash()
		{
			using (MemoryStream output = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(output))
				{
					foreach (ComicPageInfo image in this)
					{
						binaryWriter.Write(image.ImageIndex + 1); //ComicInfo.ImageIndex is 0 based, while ProviderImageInfo starts at 1
						binaryWriter.Write((long)image.ImageFileSize); //ComicInfo.ImageFileSize is an int, while ProviderImageInfo.Size is a long
					}
					binaryWriter.Flush();
					binaryWriter.Seek(0, SeekOrigin.Begin);
					return Base32.ToBase32String(new SHA1Managed().ComputeHash(binaryWriter.BaseStream));
				}
			}
		}
	}
}
