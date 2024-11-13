namespace WebApplicationDemo.Routing.Concepts
{
    public static class RoutingConceptsExtension
    {
        public static void UseEndpointConcepts(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "My API V1");
                });
            }

            app.Use(async (context, next) =>
            {
                var currentEndpoint = context.GetEndpoint();

                if (currentEndpoint is null)
                {
                    await next(context);
                    return;
                }

                Console.WriteLine($"Endpoint: {currentEndpoint.DisplayName}");

                if (currentEndpoint is RouteEndpoint routeEndpoint)
                {
                    Console.WriteLine($"  - Route Pattern: {routeEndpoint.RoutePattern}");
                }

                foreach (var endpointMetadata in currentEndpoint.Metadata)
                {
                    Console.WriteLine($"  - Metadata: {endpointMetadata}");
                }

                await next(context);
            });

            app.MapGet("/", () => "Inspect Endpoint.");
        }

        public static void UseTerminalMiddlewareConcepts(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "My API V1");
                });
            }
            // Approach 1: Terminal Middleware.
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    await context.Response.WriteAsync("Terminal Middleware.");
                    return;
                }

                await next(context);
            });

            app.UseRouting();

            // Approach 2: Routing.
            app.MapGet("/Routing", () => "Routing.");
        }

        public static void UseRoutingConcepts(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "My API V1");
                });
            }

            // Location 1: before routing runs, endpoint is always null here.
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"1. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
                await next(context);
            });

            app.UseRouting();

            // Location 2: after routing runs, endpoint will be non-null if routing found a match.
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"2. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
                await next(context);
            });

            // Location 3: runs when this endpoint matches
            app.MapGet("/Hello", (HttpContext context) =>
            {
                Console.WriteLine($"3. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
                return "Hello World!";
            });

            app.MapPut("/Hello", (HttpContext context) =>
            {
                Console.WriteLine($"3. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
                return "Hello World!";
            });

            app.MapPost("/Hello", (HttpContext context) =>
            {
                Console.WriteLine($"3. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
                return "Hello World!";
            });

            app.MapDelete("/Hello", (HttpContext context) =>
            {
                Console.WriteLine($"3. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
                return "Hello World!";
            });

            app.UseEndpoints(_ => { });

            // Location 4: runs after UseEndpoints - will only run if there was no match.
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"4. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
                await next(context);
            });
        }
    }
}
