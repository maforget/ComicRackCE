namespace cYo.Projects.ComicRack.Engine
{
	public interface IEditRating
	{
		bool IsValid();

		void SetRating(float rating);

		float GetRating();

		bool QuickRatingAndReview();
	}
}
