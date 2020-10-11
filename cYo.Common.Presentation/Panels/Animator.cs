using System;
using cYo.Common.Mathematics;
using cYo.Common.Runtime;

namespace cYo.Common.Presentation.Panels
{
	public class Animator
	{
		private volatile int delay;

		private volatile int time;

		private volatile int span = 1000;

		private volatile bool isRunning;

		private volatile float animationValue;

		private volatile float lastAnimationValue;

		private AnimationValueHandler animationValueGenerator = LinearRise;

		private AnimationHandler animationHandler;

		private long startTime;

		public int Delay
		{
			get
			{
				return delay;
			}
			set
			{
				delay = value;
			}
		}

		public int Time => time;

		public int Span
		{
			get
			{
				return span;
			}
			set
			{
				span = value;
			}
		}

		public bool HasBeenStarted => startTime != 0;

		public bool IsRunning => isRunning;

		public bool IsCompleted
		{
			get
			{
				if (HasBeenStarted)
				{
					return time >= span;
				}
				return false;
			}
		}

		public float AnimationValue => animationValue;

		public float LastAnimationValue => lastAnimationValue;

		public AnimationValueHandler AnimationValueGenerator
		{
			get
			{
				return animationValueGenerator;
			}
			set
			{
				animationValueGenerator = value;
			}
		}

		public AnimationHandler AnimationHandler
		{
			get
			{
				return animationHandler;
			}
			set
			{
				animationHandler = value;
			}
		}

		public static long Now => Machine.Ticks;

		public event EventHandler Started;

		public void Start()
		{
			isRunning = true;
			startTime = Now;
			time = 0;
			animationValue = GetAnimationValue().Clamp(0f, 1f);
			OnStarted();
		}

		public void Stop()
		{
			isRunning = false;
		}

		public virtual bool Animate(OverlayPanel panel)
		{
			if (!IsRunning || IsCompleted)
			{
				return false;
			}
			time = (int)Math.Max(0L, Now - startTime - Delay);
			lastAnimationValue = animationValue;
			animationValue = GetAnimationValue().Clamp(0f, 1f);
			OnAnimate(panel);
			return true;
		}

		protected virtual float GetAnimationValue()
		{
			if (animationValueGenerator == null)
			{
				return 1f;
			}
			return animationValueGenerator(Time, Span);
		}

		protected virtual void OnAnimate(OverlayPanel panel)
		{
			if (animationHandler != null)
			{
				animationHandler(panel, AnimationValue, AnimationValue - LastAnimationValue);
			}
		}

		protected virtual void OnStarted()
		{
			if (this.Started != null)
			{
				this.Started(this, EventArgs.Empty);
			}
		}

		public static float Constant1(int time, int span)
		{
			return 1f;
		}

		public static float LinearRise(int time, int span)
		{
			return (float)time / (float)span;
		}

		public static float SinusRise(int time, int span)
		{
			float num = (float)time / (float)span;
			if (num <= 0f || num >= 1f)
			{
				return num;
			}
			return (float)Math.Sin((double)num * Math.PI / 2.0);
		}

		public static float LinearDrop(int time, int span)
		{
			return 1f - LinearRise(time, span);
		}

		public static AnimationValueHandler CreateBouncer(int inTime, int stayTime, int outTime, AnimationValueHandler inHandler, AnimationValueHandler stayHandler, AnimationValueHandler outHandler)
		{
			return delegate(int time, int span)
			{
				if (time < inTime)
				{
					return inHandler(time, inTime);
				}
				time -= inTime;
				if (time < stayTime)
				{
					return stayHandler(time, stayTime);
				}
				time -= stayTime;
				return (time < outTime) ? outHandler(time, outTime) : 0f;
			};
		}

		public static AnimationValueHandler CreateLinearBouncer(int inTime, int stayTime, int outTime)
		{
			return CreateBouncer(inTime, stayTime, outTime, LinearRise, Constant1, LinearDrop);
		}
	}
}
