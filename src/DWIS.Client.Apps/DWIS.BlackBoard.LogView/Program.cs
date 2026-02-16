using DWIS.BlackBoard.LogView;
using DWIS.BlackBoard.LogView.Components;
using DWIS.Client.ReferenceImplementation;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddSingleton<IOPCUADWISClient, DWIS.Client.ReferenceImplementation.OPCFoundation.DWISClientOPCF>();
builder.Services.AddSingleton<IDWISClientConfiguration>(new DefaultDWISClientConfiguration());
builder.Services.AddSingleton<AppControl>(new AppControl());


var app = builder.Build();

var client = app.Services.GetRequiredService<IOPCUADWISClient>();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
