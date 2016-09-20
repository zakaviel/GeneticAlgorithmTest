using GeneticAlgorithm.ViewModel.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeneticAlgorithm.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(IndexViewModel model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    //model.CargarDatos(CargarDatosContext(), model.FacturaId);
                    TryUpdateModel(model);
                    return View(model);
                }

                var newModel = new IndexViewModel();
                model.CargarDatos();

                int val = model.BitsX;


            }
            catch (Exception ex)
            {


                TryUpdateModel(model);


            }

            return View(model);
        }


        
    }
}