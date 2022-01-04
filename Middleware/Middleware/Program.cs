using Middleware;

await new BasicMiddlewareProvider()
	.Use(async (ctx, next) =>
	{
		ctx.DynamicString = "Hello World";
		await next();
	})
	.Use(async (ctx, next) =>
	{
		if (ctx.DynamicString == "Hello World")
		{
			Console.WriteLine("Hello World indeed");
		}

		await next();
	})
	.Use((_, _) =>
	{
		Console.WriteLine("Let's stop here");
		return Task.CompletedTask;
	})
	.Use((_, _) =>
	{
		Console.WriteLine("Do I still live?");
		return Task.CompletedTask;
	})
	.Run(new Context());

Console.WriteLine();
Console.WriteLine("---");
Console.WriteLine();

await new TypedMiddlewareProvider()
	.Use(new ConcreteMiddleware(1))
	.Use(new ConcreteMiddleware(2))
	.Use(new ConcreteMiddleware(2))
	.Use(new ConcreteMiddleware(3))
	.Run(new Context());