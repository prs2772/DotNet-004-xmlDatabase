using System.Data;
using System.Text.Json.Serialization;

internal class Program
{
  public static readonly string bdPath = "CoevFive.xml";
  public enum Tables
  {
    Historical = 0
  }
  public enum ColumnsEnum
  {
    Chain = 0,
    Encrypt
  }

  public static string[][] Columns { get; } = new string[][]
  {
		//Tables[0] (Historical)
    new string[]{"chain", "encrypt"}
  };

  private static void Main(string[] args)
  {
    //Si no existe lo crea
    if (!File.Exists(bdPath))
    {
      DataSet ds = Generate();
      ds.WriteXml(bdPath);
    }

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
      app.UseExceptionHandler("/Home/Error");
      app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
  }

  private static DataSet Generate()
  {
    DataSet database = new DataSet("encrypt");
    DataTable historical = new DataTable("historical");
    database.Tables.Add(historical);

    foreach (string column in Columns[(int)Tables.Historical])
    {
      DataColumn dcDud = new DataColumn(column);
      database.Tables[(int)Tables.Historical].Columns.Add(dcDud);
    }

    DataRow registro = database.Tables[(int)Tables.Historical].NewRow();
    foreach (string columna in Program.Columns[(int)Tables.Historical])
    {
      registro[columna] =
      columna.Equals(Program.Columns[(int)Tables.Historical][(int)ColumnsEnum.Chain]) ? "" :
      columna.Equals(Program.Columns[(int)Tables.Historical][(int)ColumnsEnum.Encrypt]) ? "" :
      "0";
    }
    database.Tables[(int)Tables.Historical].Rows.Add(registro);

    return database;
  }
}