namespace cYo.Common.Presentation.Panels
{
	public class FadeAnimator : Animator
	{
		public FadeAnimator(int fadeInTime, int visibilityTime, int fadeOutTime)
		{
			base.Span = fadeInTime + visibilityTime + fadeOutTime;
			base.AnimationValueGenerator = Animator.CreateLinearBouncer(fadeInTime, visibilityTime, fadeOutTime);
			base.AnimationHandler = delegate(OverlayPanel p, float x, float d)
			{
				p.Opacity += d;
			};
		}
	}
}
