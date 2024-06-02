using ContactBook.Core.Entities;
using ContactBook.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ContactBook.Core.Dtos;
using Image = System.Drawing.Image;

namespace ContactBook.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        /// <summary>
        /// create file pdf
        /// </summary>
        /// <param name="contacts">list of contact</param>
        /// <returns></returns>
        public byte[] CreatePdf(IEnumerable<ContactDto> contacts)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(8));

                    page.Header().Height(50).AlignCenter().Text("CONTACT BOOK")
                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(50);  
                            columns.ConstantColumn(60);  
                            columns.RelativeColumn();    
                            columns.RelativeColumn();    
                            columns.RelativeColumn();    
                            columns.RelativeColumn();    
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("ID").FontColor(Colors.White);
                            header.Cell().Element(CellStyle).Text("Image").FontColor(Colors.White);
                            header.Cell().Element(CellStyle).Text("First Name").FontColor(Colors.White);
                            header.Cell().Element(CellStyle).Text("Last Name").FontColor(Colors.White);
                            header.Cell().Element(CellStyle).Text("Email").FontColor(Colors.White);
                            header.Cell().Element(CellStyle).Text("Phone").FontColor(Colors.White);

                            static IContainer CellStyle(IContainer container) => container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).PaddingHorizontal(5).Background(Colors.Black);
                        });

                        // Add rows
                        foreach (var contact in contacts)
                        {
                            var imagePath = GetImage(contact.ContactPicture).Result;
                            var circularImagePath = MakeCircularImage(imagePath).Result;
                            table.Cell().Element(CellStyle).Text(contact.Id);
                            table.Cell().Element(CellStyle).AlignCenter().Height(20).Width(20).Image(circularImagePath, ImageScaling.FitArea);
                            table.Cell().Element(CellStyle).Text(contact.FirstName);
                            table.Cell().Element(CellStyle).Text(contact.LastName);
                            table.Cell().Element(CellStyle).Text(contact.Email);
                            table.Cell().Element(CellStyle).Text(contact.PhoneNumber);

                            static IContainer CellStyle(IContainer container) => container.PaddingVertical(5).PaddingHorizontal(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Copyright © ");
                        x.Span(DateTime.Now.Year.ToString());
                        x.Span(" MyCompany - Contact Book");
                    });
                });
            });

            using var memoryStream = new MemoryStream();
            document.GeneratePdf(memoryStream);
            return memoryStream.ToArray();
        }

        private async Task<string> GetImageAsync(string imageUrl)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(imageUrl);

            if (response.IsSuccessStatusCode)
            {
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                var tempFilePath = Path.GetTempFileName() + ".jpg";
                await File.WriteAllBytesAsync(tempFilePath, imageBytes);
                return tempFilePath;
            }
            return string.Empty;
        }

        private Task<string> GetImage(string imageUrl)
        {
            return GetImageAsync(imageUrl);
        }

        private async Task<string> MakeCircularImage(string imagePath)
        {
            using var srcImage = Image.FromFile(imagePath);
            var diameter = Math.Min(srcImage.Width, srcImage.Height);
            var destRect = new Rectangle(0, 0, diameter, diameter);

            using var destImage = new Bitmap(diameter, diameter);
            using (var g = Graphics.FromImage(destImage))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(destRect);
                    g.SetClip(path);
                    g.DrawImage(srcImage, destRect, new Rectangle((srcImage.Width - diameter) / 2, (srcImage.Height - diameter) / 2, diameter, diameter), GraphicsUnit.Pixel);
                }
            }

            var tempFilePath = Path.GetTempFileName() + ".png";
            destImage.Save(tempFilePath);
            return tempFilePath;
        }
    }
}
