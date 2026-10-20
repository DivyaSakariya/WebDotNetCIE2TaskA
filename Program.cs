var builder = WebApplication.CreateBuilder(args);

// Registers MVC controllers and Razor views.
builder.Services.AddControllersWithViews();

// Registers memory caching for three-minute doctor-list caching.
builder.Services.AddMemoryCache();

// Registers session services for storing specialization.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Keeps session data available for 20 minutes.
    options.IdleTimeout = TimeSpan.FromMinutes(20);

    // Prevents client-side JavaScript from accessing the session cookie.
    options.Cookie.HttpOnly = true;

    // Allows the session cookie only over HTTPS when available.
    options.Cookie.IsEssential = true;
});

// Registers HttpClient for asynchronous external API requests.
builder.Services.AddHttpClient();

// Registers response caching middleware services.
builder.Services.AddResponseCaching();

var app = builder.Build();

// Displays a friendly error page outside development mode.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirects HTTP requests to HTTPS.
// HTTPS redirection temporarily disabled for local HTTP testing.
if (app.Environment.IsDevelopment())
{
    // Configure an HTTPS URL in launchSettings.json before enabling this.
}
else
{
    app.UseHttpsRedirection();
}

// Serves CSS, JavaScript and other static files.
app.UseStaticFiles();

// Enables routing to MVC controller actions.
app.UseRouting();

// Enables session before controller actions execute.
app.UseSession();

// Enables HTTP response caching.
app.UseResponseCaching();

// Configures the default controller and action.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Doctors}/{action=Index}/{id?}");

// Starts the web application.
app.Run();