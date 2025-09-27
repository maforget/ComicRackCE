using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookMetadataManager
	{
		private static readonly Lazy<ComicBookMetadataManager> manager = new Lazy<ComicBookMetadataManager>(() => new ComicBookMetadataManager());

		private readonly ComicBookMetadataCollection collection;
		public static ComicBookMetadataCollection Collection => manager.Value?.collection;

		private ComicBookMetadataManager()
		{
			collection = new ComicBookMetadataCollection();
		}

		// Take the list of IColumn and create the Collection
		public static void Create(IEnumerable<IColumn> columns)
		{
			if (columns is null)
				return;

			Collection.Clear();
			foreach (var column in columns)
			{
				if (column.ColumnSorter is null && column.ColumnGrouper is null)
					continue;

				if (Collection.Any(m => m.Id == column.Id))
					return;

				ComicBookMetadata metadata = new ComicBookMetadata(column.Id, column.Name, column.ColumnSorter, column.ColumnGrouper);
				Collection.Add(metadata);
			}
		}


		#region Generics Comparers
		// Converts the sortKey to a list of ComicBookMetadata & returns the lists of comparers associated with it, returns null if the first one is null
		private static IEnumerable<IComparer<T>> GetGenericComparers<T>(string sortKey)
		{
			var columns = ConvertKeyToMetadata(sortKey);
			var comparers = columns.Where(m => m != null);
			if (comparers?.FirstOrDefault()?.GetComparer<T>() == null)
				return null;

			return comparers.Where(m => m.GetComparer<T>() != null).Select(m => m.GetComparer<T>());
		}

		// Gets the first generic comparer for the given comma separated sort key
		private static IComparer<T> GetGenericFirstComparer<T>(string sortKey) => GetGenericComparers<T>(sortKey)?.FirstOrDefault();

		// Gets an array of comparers for the given comma separated sort key
		private static IComparer<T>[] GetGenericChainedComparer<T>(string sortKey)
		{
			var comparer = GetGenericComparers<T>(sortKey);
			return comparer is null ? null : comparer.TakeWhile(x => x != null).ToArray();
		}
		#endregion


		#region Comparers
		// Gets the first comparer for the given comma separated sort key
		public static IComparer<ComicBook> GetComparer(string sortKey) => GetGenericFirstComparer<ComicBook>(sortKey);

		// Gets an array of comparers for the given comma separated sort key
		public static IComparer<ComicBook>[] GetComparers(string sortKey) => GetGenericChainedComparer<ComicBook>(sortKey);

		// Gets the first comparer for the given comma separated sort key
		public static IComparer<IViewableItem> GetIViewableItemComparer(string sortKey) => GetGenericFirstComparer<IViewableItem>(sortKey);

		// Gets an array of comparers for the given comma separated sort key
		public static IComparer<IViewableItem>[] GetIViewableItemComparers(string sortKey) => GetGenericChainedComparer<IViewableItem>(sortKey);
		#endregion


		#region Generic Groupers
		// Converts the sortKey to a list of ComicBookMetadata & returns the lists of groupers associated with it, returns null if the first one is null
		private static IEnumerable<IGrouper<T>> GetGenericGrouper<T>(string sortKey)
		{
			var columns = ConvertKeyToMetadata(sortKey);
			var groupers = columns.Where(m => m != null);
			if (groupers?.FirstOrDefault()?.GetGrouper<T>() == null)
				return null;

			return groupers?.Where(m => m.GetGrouper<T>() != null).Select(m => m.GetGrouper<T>());
		}

		// Gets the first generic grouper for the given comma separated sort key
		private static IGrouper<T> GetGenericFirstGrouper<T>(string sortKey) => GetGenericGrouper<T>(sortKey)?.FirstOrDefault();

		// Gets a chained generic grouper for the given comma separated sort key
		private static IGrouper<T> GetGenericCompoundGroupers<T>(string sortKey)
		{
			var groupers = GetGenericGrouper<T>(sortKey);
			return groupers is null ? null : new CompoundSingleGrouper<T>(GetGenericGrouper<T>(sortKey).TakeWhile(x => x != null).ToArray());
		}
		#endregion


		#region Groupers
		// Gets the first grouper for the given comma separated sort key
		public static IGrouper<ComicBook> GetGrouper(string sortKey) => GetGenericFirstGrouper<ComicBook>(sortKey);

		// Gets a chained grouper for the given comma separated sort key
		public static IGrouper<ComicBook> GetGroupers(string sortKey) => GetGenericCompoundGroupers<ComicBook>(sortKey);

		// Gets the first grouper for the given comma separated sort key
		public static IGrouper<IViewableItem> GetIViewableItemGrouper(string sortKey) => GetGenericFirstGrouper<IViewableItem>(sortKey);

		// Gets a chained grouper for the given comma separated sort key
		public static IGrouper<IViewableItem> GetIViewableItemGroupers(string sortKey) => GetGenericCompoundGroupers<IViewableItem>(sortKey); 
		#endregion


		//takes the key and outputs the corresponding ComicBookMetadata from the Manager Collection
		private static IEnumerable<ComicBookMetadata> ConvertKeyToMetadata(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				yield break;
			}
			string[] array = key.Split(',');
			foreach (string s in array)
			{
				if (int.TryParse(s, out var result))
				{
					ComicBookMetadata metadata = Collection.FindById(result);
					if (metadata != null)
						yield return metadata;
				}
			}
		}
	}
}
