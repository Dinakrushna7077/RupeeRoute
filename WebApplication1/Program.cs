var builder = WebApplication.CreateBuilder(args);

// ===============================
// Add services to the container
// ===============================

// MVC controllers + views
builder.Services.AddControllersWithViews();

// HttpClient (used by ApiService)
builder.Services.AddHttpClient();

// Session (for login persistence – userId, email later)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ===============================
// Build app
// ===============================

var app = builder.Build();

// ===============================
// Configure the HTTP request pipeline
// ===============================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ❌ IMPORTANT
// Do NOT use HTTPS redirection
// because API is running on HTTP
//
// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// ===============================
// MVC Routing
// ===============================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
