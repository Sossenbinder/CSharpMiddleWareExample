namespace Middleware
{
	public delegate Task MiddlewareDelegate(Context context, Func<Task> next);

	public class BasicMiddlewareProvider
	{
		private readonly Queue<MiddlewareDelegate> _middleWares = new();

		public BasicMiddlewareProvider Use(MiddlewareDelegate step)
		{
			_middleWares.Enqueue(step);
			return this;
		}

		public async Task Run(Context context)
		{
			if (_middleWares.Any())
			{
				await RunInternal(context);
			}
		}

		private async Task RunInternal(Context context)
		{
			if (_middleWares.TryDequeue(out var middleware))
			{
				await middleware(context, () => RunInternal(context));
			}
		}
	}
}