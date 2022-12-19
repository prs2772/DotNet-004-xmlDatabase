using System.Globalization;
using System.Text;

public static class EncryptEfe
{
  ///<summary>
  ///Agrega f y repite la vocal después de cada vocal ejemplo: efejefemplofo
  ///Fuente de que rayos es eso de F: https://aldeadelasletras.blogspot.com/2010/08/el-idioma-de-la-efe-tefe-afacueferdafas.html
  ///Para no complicar con diptongos, se toma la regla sencilla: https://youtu.be/oW-CK8PYutM
  ///<summary>
  public static string EncryptBPP(this string? cadena)
  {
    StringBuilder encrypted = new StringBuilder();
    string[] vowels = new string[] { "a", "e", "i", "o", "u" };

    if (cadena != null && cadena.Length > 0)
    {
      string cadenaSinTildes = cadena.SinTildes();
      foreach (char letra in cadenaSinTildes.ToCharArray())
      {
        encrypted.Append(letra.ToString());
        if (Array.Find<string>(vowels, chr => chr.Equals(letra.ToString().ToLower())) != null)
        {
          encrypted.Append($"f{letra.ToString()}");
        }
      }
    }

    return encrypted.ToString();
  }

  public static string SinTildes(this string texto) =>
    new String(
      texto.Normalize(NormalizationForm.FormD)
      .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
      .ToArray()
    )
    .Normalize(NormalizationForm.FormC);
}