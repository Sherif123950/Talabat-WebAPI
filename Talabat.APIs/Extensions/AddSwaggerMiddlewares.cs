namespace Talabat.APIs.Extensions
{
    public static class AddSwaggerExtensions
    {
        public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
