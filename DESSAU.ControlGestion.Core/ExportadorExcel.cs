using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.Core
{
    public static class ExportadorExcel
    {

        /// <summary>
        /// Genera un Excel de un listado de objetos
        /// </summary>
        /// <param name="items">Lista con los datos a incluir en archivo Excel</param>
        /// <param name="ms">MemoryStream que contendrá el Excel</param>
        /// <param name="sheetName">Nombre de la hoja a crear donde se volcarán los datos</param>

        public static IFont f = null;
        public static ICellStyle estilo = null;
        public static void GeneraExcel<T>(IEnumerable<T> items, string sheetName, MemoryStream ms, bool withHeader) where T : class
        {
            if (items == null) throw new ArgumentNullException("items");
            if (sheetName == null) throw new ArgumentNullException("sheetName");
            if (ms == null) throw new ArgumentNullException("ms");

            if (sheetName.Trim() == String.Empty)
            {
                sheetName = "Hoja1";
            }
            XSSFWorkbook wb = new XSSFWorkbook();
            ISheet sheet = wb.CreateSheet(sheetName);
            f = wb.CreateFont();
            estilo = wb.CreateCellStyle();
            estilo.SetFont(f);
            estilo.BorderBottom = BorderStyle.Thin;
            estilo.BorderLeft = BorderStyle.Thin;
            estilo.BorderRight = BorderStyle.Thin;
            estilo.BorderTop = BorderStyle.Thin;
            PropertyInfo[] pis = typeof(T).GetProperties();
            T[] itemsArray = items.ToArray();

            if (withHeader)
            { // Encabezado
                IRow row = sheet.CreateRow(0);
                for (int j = 0; j < pis.Length; j++)
                {
                    ICell cell = row.CreateCell(j);
                    //object value = pis[j].Name;
                    cell.SetCellValue(GetPropertyName(pis[j]));
                    cell.CellStyle = estilo;
                }
            }

            for (int i = 0; i < itemsArray.Length; i++)
            {
                int nroFila = withHeader ? i + 1 : i;
                IRow row = sheet.CreateRow(nroFila);
                T item = itemsArray[i];

                for (int j = 0; j < pis.Length; j++)
                {
                    ICell cell = row.CreateCell(j);
                    object value = pis[j].GetValue(item, null);
                    if (value != null)
                    {
                        cell.SetCellValue(value.ToString());
                    }

                }
            }
            for (int i = 0; i < itemsArray.Length; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            wb.Write(ms);
        }
        //public static void GeneraExcelMatriz<T>(IEnumerable<T> items, IEnumerable<TipoTecnologia> itemCabezera, string sheetName, MemoryStream ms) where T : class
        //{
        //    if (items == null) throw new ArgumentNullException("items");
        //    if (sheetName == null) throw new ArgumentNullException("sheetName");
        //    if (ms == null) throw new ArgumentNullException("ms");

        //    if (sheetName.Trim() == String.Empty)
        //    {
        //        sheetName = "Hoja1";
        //    }

        //    XSSFWorkbook wb = new XSSFWorkbook();
        //    ISheet sheet = wb.CreateSheet(sheetName);
        //    f = wb.CreateFont();
        //    f.Boldweight = (short)FontBoldWeight.Bold; ;
        //    estilo = wb.CreateCellStyle();
        //    estilo.SetFont(f);
        //    estilo.BorderBottom = BorderStyle.Thin;
        //    estilo.BorderLeft = BorderStyle.Thin;
        //    estilo.BorderRight = BorderStyle.Thin;
        //    estilo.BorderTop = BorderStyle.Thin;
        //    PropertyInfo[] pis = typeof(T).GetProperties();
        //    T[] itemsArray = items.ToArray();

        //    // Encabezado Columnas
        //    IRow row = sheet.CreateRow(0);
        //    int z = 1;
        //    foreach (var item in itemCabezera)
        //    {
        //        ICell cell = row.CreateCell(z);
        //        cell.SetCellValue(item.Nombre);
        //        cell.CellStyle = estilo;
        //        z++;
        //    }

        //    //Filas
        //    for (int i = 0; i < itemsArray.Length; i++)
        //    {
        //        int nroFila = i + 1;
        //        row = sheet.CreateRow(nroFila);
        //        T item = itemsArray[i];

        //        for (int j = 0; j < pis.Length; j++)
        //        {
        //            ICell cell = row.CreateCell(j);
        //            object value = pis[j].GetValue(item, null);
        //            if (value != null)
        //            {
        //                cell.SetCellValue(value.ToString());
        //            }
        //        }
        //        int cont = 1;
        //        foreach (var itemTec in itemCabezera)
        //        {
        //            object value2 = pis[1].GetValue(item, null);
        //            List<int> arreglo = (List<int>)value2;
        //            bool igualdad = false;
        //            if (value2 != null)
        //            {
        //                for (int x = 0; x < arreglo.Count(); x++)
        //                {
        //                    igualdad = false;
        //                    int valor = arreglo.ElementAt(x);
        //                    if (itemTec.IdTipoTecnologia == valor)
        //                    {
        //                        igualdad = true;
        //                        x = arreglo.Count();
        //                    }
        //                }
        //            }
        //            ICell cell = row.CreateCell(cont);
        //            if (igualdad == true)
        //            {
        //                cell.SetCellValue("x");
        //            }
        //            else
        //            {
        //                cell.SetCellValue(" ");
        //            }
        //            cont++;

        //        }

        //    }
        //    for (int i = 0; i < itemsArray.Length; i++)
        //    {
        //        sheet.AutoSizeColumn(i);
        //    }
        //    wb.Write(ms);
        //}
        public static string GetPropertyName(PropertyInfo propertyInfo)
        {
            DisplayNameAttribute displayNameAttribute = propertyInfo
                .GetCustomAttributes(false)
                .OfType<DisplayNameAttribute>()
                .FirstOrDefault();
            return displayNameAttribute != null ? displayNameAttribute.DisplayName : propertyInfo.Name;
        }
    }
}
