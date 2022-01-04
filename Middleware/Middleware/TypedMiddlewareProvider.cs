namespace Middleware
{
	public interface IMiddleware
	{
		Task Execute(Context context, Func<Task> next);
	}

	public class ConcreteMiddleware : IMiddleware
	{
		private readonly int _newContextValue;

		public ConcreteMiddleware(int newContextValue) => _newContextValue = newContextValue;

		public async Task Execute(Context context, Func<Task> next)
		{
			if (context.CurrentUserId == _newContextValue)
			{
				Console.WriteLine($"Typed middleware hit its end - {context.CurrentUserId}");
				return;
			}

			Console.WriteLine($"Typed middleware CurrentUserId {context.CurrentUserId}");
			context.CurrentUserId = _newContextValue;

			await next();
		}
	}

	public class TypedMiddlewareProvider
	{
		private readonly Queue<IMiddleware> _middlewares = new();

		public TypedMiddlewareProvider Use(IMiddleware middleware)
		{
			_middlewares.Enqueue(middleware);
			return this;
		}

		public async Task Run(Context context)
		{
			if (_middlewares.Any())
			{
				await RunInternal(context);
			}
		}

		private async Task RunInternal(Context context)
		{
			if (_middlewares.TryDequeue(out var middleware))
			{
				await middleware!.Execute(context, () => RunInternal(context));
			}
		}
	}
}