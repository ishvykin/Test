using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using TestMVCApp;
using TestMVCApp.Models;


namespace TestMVCApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string filter = null, string submitButton = null)
        {
            if (filter != null & filter != "")

            {
                TESTEntities db = new TESTEntities();
               
                    var ProcResult = db.SELECT_GOODS(filter);
                    ViewBag.ProcResult = ProcResult;
                    if (submitButton == "Excel")
                    {
                        string fileName = "Goods[" + DateTime.Now.ToShortDateString() + " - " +
                                          DateTime.Now.Hour + "--" + DateTime.Now.Minute + "].xlsx";
                        string outputDir = Server.MapPath("/");
                        using (ExcelPackage pck = new ExcelPackage())
                        {
                            var ws = pck.Workbook.Worksheets.Add("Goods");
                            ws.View.ShowGridLines = false;
                            ws.Cells["A1"].Value = "Код товара";
                            ws.Cells["B1"].Value = "Наименование";
                            ws.Cells["C1"].Value = "Количество";
                            ws.Cells["D1"].Value = "Ед. изм.";
                            ws.Cells["E1"].Value = "Цена";
                            ws.Cells["F1"].Value = "Дата создания";

                            ws.Cells["A1:F1"].Style.Font.Bold = true;

                            if (ProcResult != null)
                            {
                                int index = 2;
                                foreach (var someResult in ProcResult)
                                {
                                    ws.Cells["A" + index].Style.Numberformat.Format = "0";
                                    ws.Cells["C" + index].Style.Numberformat.Format = "0";
                                    ws.Cells["E" + index].Style.Numberformat.Format = "0";
                                    ws.Cells["F" + index].Style.Numberformat.Format = "mm/dd/yyyy hh:mm:ss";


                                    ws.Cells["A" + index].Value = someResult.id;
                                    ws.Cells["B" + index].Value = someResult.description;
                                    ws.Cells["C" + index].Value = someResult.quantity;
                                    ws.Cells["D" + index].Value = someResult.unit;
                                    ws.Cells["E" + index].Value = someResult.price;
                                    ws.Cells["F" + index].Value = someResult.creation_date;
                                    index++;
                                }
                            }
                            ws.Cells.AutoFitColumns();
                            var bin = pck.GetAsByteArray();
                            System.IO.File.WriteAllBytes(outputDir + fileName, bin);
                        }
                        return File((outputDir + fileName), MimeMapping.GetMimeMapping(outputDir + fileName), fileName);
                        ;
                    }

                
            }

            return View();

        }
        [HttpPost]
        [Route("home/NewAjaxRequest")]
        public void NewAjaxRequest(FormInformation info)
        {
            TESTEntities db = new TESTEntities();
          
                foreach (int id in info.Massiv)
                {
                    db.UPDATE_GOOD_PRICE(id, info.Price);
                }
            
        }
    }
}