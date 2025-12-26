using GestionLocationVehicule.Areas.Admin.Data;
using GestionLocationVehicule.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionLocationVehicule.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VehiculesController : Controller
    {
        private readonly ApplicationDbContext context;

        public VehiculesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vehicules = context.Vehicules
                                .Include(v => v.VehicleCategory)
                                .ToList();
            //ViewBag.vehicules = vehicules;
            return View(vehicules);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var VehicleCategories = context.VehicleCategories.ToList();
            ViewBag.VehicleCategories = VehicleCategories;
            return View();
        }

        [HttpPost]
        public IActionResult Create(VehiculeFormModel vehiculeForm)
        {
            try
            {
                if (vehiculeForm.ImageFile == null)
                {
                    ModelState.AddModelError("ImageFile", "L'image du véhicule est requise.");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Le formulaire contient des erreurs. Veuillez vérifier les champs.";
                    return View(vehiculeForm);
                }
                var categoryExists = context.VehicleCategories.Any(VehicleCategory => VehicleCategory.Id == vehiculeForm.VehicleCategoryId);
                if (!categoryExists)
                {
                    ModelState.AddModelError("VehicleCategoryId", "La catégorie sélectionnée n'existe pas.");
                    return View(vehiculeForm);
                }

                Vehicule v = new Vehicule();
                v.Titre = vehiculeForm.Titre;
                v.Description = vehiculeForm.Description;
                v.Matricule = vehiculeForm.Matricule;
                v.Marque = vehiculeForm.Marque;
                v.Modele = vehiculeForm.Modele;
                v.Annee = vehiculeForm.Annee;
                v.Kilometrage = vehiculeForm.Kilometrage;
                v.Prix = vehiculeForm.Prix;
                v.Statut = vehiculeForm.Statut;

                v.VehicleCategoryId = vehiculeForm.VehicleCategoryId;

                if (vehiculeForm.ImageFile != null)
                {
                    var fileName = Path.GetFileName(vehiculeForm.ImageFile.FileName);

                    var imgFullPath = Path.Combine("wwwroot", "images", "admin", "vehicules");

                    var filePath = Path.Combine(imgFullPath, fileName);
                    v.ImagePath = "/" + Path.Combine("images", "admin", "vehicules", fileName).Replace("\\", "/");
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        vehiculeForm.ImageFile.CopyTo(stream);
                    }
                }

                ViewBag.success = "Véhicule Bien Sauvgarder.";
                context.Vehicules.Add(v);
                context.SaveChanges();
            }
            catch(Exception ex) 
            {
                ViewBag.ErrorMessage = "Une erreur est survenue : " + ex.Message;
            }
            return View(new VehiculeFormModel());
        }
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            try
            {
                if (Id != 0)
                {
                    var element = context.Vehicules.Find(Id);
                    if (element != null)
                    {
                        context.Vehicules.Remove(element);
                        context.SaveChanges();
                    }

                }
                ViewBag.success = "Véhicule supprimé avec succès.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Une erreur est survenue : " + ex.Message;
            }

            return RedirectToAction("Index", "Vehicules");
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (Id != 0)
            {
                var element = context.Vehicules.Find(Id);
                //ViewBag.element = element;
                //Console.WriteLine(element);
                if (element != null) {

                    var VehiculeFormModel = new VehiculeFormModel();
                    VehiculeFormModel.Id = element.Id;
                    VehiculeFormModel.Titre = element.Titre;
                    VehiculeFormModel.Description = element.Description;
                    VehiculeFormModel.Matricule = element.Matricule;
                    VehiculeFormModel.Marque = element.Marque;
                    VehiculeFormModel.Modele = element.Modele;
                    VehiculeFormModel.Annee = element.Annee;
                    VehiculeFormModel.Kilometrage = element.Kilometrage;
                    VehiculeFormModel.Statut = element.Statut;
                    VehiculeFormModel.Prix = element.Prix;
                    //ViewData["ImageFile"] = element?.ImagePath;
                    VehiculeFormModel.ImagePath = element.ImagePath;

                    //ViewData["VehiculeId"] = element?.Id;

                    VehiculeFormModel.VehicleCategoryId = element.VehicleCategoryId;

                    var VehicleCategories = context.VehicleCategories.ToList();
                    ViewBag.VehicleCategories = VehicleCategories;

                    return View(VehiculeFormModel);
                }
            }
            return RedirectToAction("Index", "Vehicules");
            //return View();
        }

        [HttpPost]
        public IActionResult Update(VehiculeFormModel vehiculeForm)
        {

            var element = context.Vehicules.Find(vehiculeForm.Id);
            if (element == null)
            {
                ModelState.AddModelError("", "Véhicule introuvable.");
                return View("Edit", vehiculeForm);
            }
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Le formulaire contient des erreurs. Veuillez vérifier les champs.";
                return View("Edit", vehiculeForm);
            }
            var categoryExists = context.VehicleCategories.Any(VehicleCategory => VehicleCategory.Id == vehiculeForm.VehicleCategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("VehicleCategoryId", "La catégorie sélectionnée n'existe pas.");
                return View("Edit", vehiculeForm);
            }

            //element.Matricule = vehiculeForm.Matricule ?? element.Matricule;
            //element.Marque = vehiculeForm.Marque ?? element.Marque;
            //element.Modele = vehiculeForm.Modele ?? element.Modele;
            //element.Annee = vehiculeForm.Annee ?? element.Annee;
            //element.Kilometrage = vehiculeForm.Kilometrage ?? element.Kilometrage;
            //element.Prix = vehiculeForm.Prix;
            //element.Statut = vehiculeForm.Statut;


            if (vehiculeForm.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(element.ImagePath))
                {
                    var oldImageFullPath = Path.Combine("wwwroot", element.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImageFullPath))
                    {
                        System.IO.File.Delete(oldImageFullPath);
                    }
                }

                var fileName = Path.GetFileName(vehiculeForm.ImageFile.FileName);

                var imgFullPath = Path.Combine("wwwroot", "images", "admin", "vehicules");

                var filePath = Path.Combine(imgFullPath, fileName);
                element.ImagePath = "/" + Path.Combine("images", "admin", "vehicules", fileName).Replace("\\", "/");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    vehiculeForm.ImageFile.CopyTo(stream);
                }
            }

            element.Marque = vehiculeForm.Marque;
            element.Modele = vehiculeForm.Modele;
            element.Titre = vehiculeForm.Titre;
            element.Annee = vehiculeForm.Annee;
            element.Kilometrage = vehiculeForm.Kilometrage;
            element.Prix = vehiculeForm.Prix;
            element.Statut = vehiculeForm.Statut;
            element.VehicleCategoryId = vehiculeForm.VehicleCategoryId;
            element.UpdatedAt = DateTime.Now;

            //context.Vehicules.Update(element);
            context.SaveChanges();

            return RedirectToAction("Index", "Vehicules");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var vehicule = context.Vehicules
                .Include(v => v.VehicleCategory)
                .FirstOrDefault(v => v.Id == id);

            if (vehicule == null)
            {
                TempData["Error"] = "Véhicule introuvable.";
                return RedirectToAction("Index", "Vehicules");
            }
            return View(vehicule);
        }

    }

}
