using ClosedXML.Excel;
using QuestPDF.Fluent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DesktopCodOperacional.Services.Export
{
    public class ExportService
    {
        public async Task PrintAsync<T>(IEnumerable<T> data,string title)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var properties =
                        typeof(T).GetProperties(
                            BindingFlags.Public |
                            BindingFlags.Instance);

                    var document = new FlowDocument
                    {
                        PagePadding = new Thickness(40),
                        FontFamily = new FontFamily("Segoe UI"),
                        FontSize = 12
                    };

                    // TITLE
                    document.Blocks.Add(new Paragraph(new Run(title))
                    {
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 20)
                    });

                    // TABLE
                    var table = new Table();

                    foreach (var _ in properties)
                    {
                        table.Columns.Add(new TableColumn());
                    }

                    // HEADER
                    var headerGroup = new TableRowGroup();
                    var headerRow = new TableRow();

                    foreach (var property in properties)
                    {
                        headerRow.Cells.Add(
                            new TableCell(
                                new Paragraph(
                                    new Run(property.Name)))
                            {
                                FontWeight = FontWeights.Bold,
                                Background = Brushes.LightGray,
                                Padding = new Thickness(6)
                            });
                    }

                    headerGroup.Rows.Add(headerRow);
                    table.RowGroups.Add(headerGroup);

                    // DATA
                    var dataGroup = new TableRowGroup();

                    foreach (var item in data)
                    {
                        var row = new TableRow();

                        foreach (var property in properties)
                        {
                            var value = property.GetValue(item);

                            string text;

                            if (value == null)
                                text = "-";
                            else if (value is DateTime dt)
                                text = dt.ToString("dd/MM/yyyy HH:mm");
                            else
                                text = value.ToString() ?? "-";

                            row.Cells.Add(new TableCell(new Paragraph(new Run(text)))
                                {
                                    Padding = new Thickness(6)
                                });
                        }

                        dataGroup.Rows.Add(row);
                    }

                    table.RowGroups.Add(dataGroup);
                    document.Blocks.Add(table);

                    // PRINT
                    var dialog = new PrintDialog();

                    if (dialog.ShowDialog() == true)
                    {
                        IDocumentPaginatorSource idpSource = document;
                        dialog.PrintDocument(idpSource.DocumentPaginator, title);
                    }
                });
            });
        }
        public async Task ExportToPdfAsync<T>(IEnumerable<T> data, string title)
        {
            try
            {
                var properties =
                    typeof(T).GetProperties(
                        BindingFlags.Public |
                        BindingFlags.Instance);

                var folder =
                    Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.Desktop),
                        "Exportaciones");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var fullPath =
                    Path.Combine(
                        folder,
                        $"{title}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);

                        page.Header()
                            .Text(title)
                            .FontSize(20)
                            .Bold();

                        page.Content()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    foreach (var _ in properties)
                                    {
                                        columns.RelativeColumn();
                                    }
                                });

                                table.Header(header =>
                                {
                                    foreach (var property in properties)
                                    {
                                        header.Cell()
                                              .Padding(5)
                                              .Text(property.Name)
                                              .Bold();
                                    }
                                });

                                foreach (var item in data)
                                {
                                    foreach (var property in properties)
                                    {
                                        var value =
                                            property.GetValue(item);

                                        table.Cell()
                                             .Padding(5)
                                             .Text(value?.ToString() ?? "-");
                                    }
                                }
                            });
                    });
                })
                .GeneratePdf(fullPath);

                Process.Start(new ProcessStartInfo
                {
                    FileName = fullPath,
                    UseShellExecute = true
                });

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "PDF ERROR");
            }
        }

        public async Task ExportToExcelAsync<T>(IEnumerable<T> data, string fileName)
        {
            await Task.Run(() =>
            {
                using var workbook = new XLWorkbook();

                var worksheet =
                    workbook.Worksheets.Add("Datos");

                var properties =
                    typeof(T).GetProperties(
                        BindingFlags.Public |
                        BindingFlags.Instance);

                // HEADERS
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = properties[i].Name;
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                }

                // DATA
                int row = 2;

                foreach (var item in data)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        var value = properties[col].GetValue(item);

                        worksheet.Cell(row, col + 1).Value = value?.ToString();
                    }

                    row++;
                }

                worksheet.Columns().AdjustToContents();

                // PATH
                var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Exportaciones");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var fullPath = Path.Combine(folder, $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");

                workbook.SaveAs(fullPath);

                Process.Start(new ProcessStartInfo
                {
                    FileName = fullPath,
                    UseShellExecute = true
                });
            });
        }
    }
}
