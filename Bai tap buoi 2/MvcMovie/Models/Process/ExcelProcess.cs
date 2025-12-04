using System.Data;
using OfficeOpenXml;

namespace MvcMovie.Models.Process
{
    public class ExcelProcess
    {
        [Obsolete]
        public DataTable ExcelToDataTable(string strPath)
        {
            ExcelPackage.License.SetNonCommercialPersonal("MINH"); 


            var dt = new DataTable();
            var fi = new FileInfo(strPath);

            using (var excel = new ExcelPackage(fi))
            {
                var ws = excel.Workbook.Worksheets[0];

                if (ws.Dimension == null)
                    return dt;

                List<string> colNames = new List<string>();
                int currentCol = 1;

                foreach (var cell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    string columnName = cell.Text.Trim();

                    if (cell.Start.Column != currentCol)
                    {
                        colNames.Add("Header_" + currentCol);
                        dt.Columns.Add("Header_" + currentCol);
                        currentCol++;
                    }

                    colNames.Add(columnName);
                    int occurrences = colNames.Count(x => x == columnName);

                    if (occurrences > 1 || string.IsNullOrWhiteSpace(columnName))
                    {
                        columnName = (string.IsNullOrWhiteSpace(columnName) ? "Header" : columnName)
                                      + "_" + occurrences;
                    }

                    dt.Columns.Add(columnName);
                    currentCol++;
                }

                for (int i = 2; i <= ws.Dimension.End.Row; i++)
                {
                    var row = ws.Cells[i, 1, i, ws.Dimension.End.Column];
                    DataRow newRow = dt.NewRow();

                    foreach (var cell in row)
                    {
                        newRow[cell.Start.Column - 1] = cell.Text;
                    }

                    dt.Rows.Add(newRow);
                }
            }

            return dt;
        }
    }
}
